/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:01
 * Version: 1.0.7
 */

using System.Globalization;
using System.Text;

using Lizerium.BINI.Converter.Models;

namespace Lizerium.BINI.Converter.Modules;

public static class BiniReader
{
    /// <summary>
    /// Reads the binary BINI layout and emits canonical INI text.
    /// </summary>
    public static string Read(byte[] data, Encoding defaultEncoding)
    {
        if (data.Length < 12)
        {
            throw new FormatException($"input is too short: {data.Length} bytes");
        }

        var magic = ReadUInt32(data, 0);
        var version = ReadUInt32(data, 4);
        var textOffset = ReadUInt32(data, 8);

        if (magic != 0x494e4942)
        {
            throw new FormatException($"unknown input format (bad magic): 0x{magic:x8}");
        }

        if (version != 1)
        {
            throw new FormatException($"unknown input format (bad version): {version}");
        }

        if (textOffset > data.Length)
        {
            throw new FormatException($"unknown input format (bad text offset): {textOffset}");
        }

        if (textOffset < data.Length && data[^1] != 0)
        {
            throw new FormatException("invalid input (unterminated text segment)");
        }

        var output = new StringBuilder();
        var position = 12;
        var textStart = checked((int)textOffset);
        var textLength = data.Length - textStart;
        var firstSection = true;

        // BINI stores fixed-size section/entry/value records first, followed by a shared NUL-terminated string table.
        while (position < textStart - 3)
        {
            var sectionNameOffset = ReadUInt16(data, position);
            var entryCount = ReadUInt16(data, position + 2);

            if (sectionNameOffset >= textLength)
            {
                throw new FormatException("invalid section text offset, aborting");
            }

            if (!firstSection)
            {
                output.Append('\n');
            }

            firstSection = false;
            AppendSectionName(output, ReadText(data, textStart, textLength, sectionNameOffset, defaultEncoding));
            position += 4;

            for (var i = 0; i < entryCount; i++)
            {
                if (position > textStart - 3)
                {
                    throw new FormatException("truncated entry, aborting");
                }

                var nameOffset = ReadUInt16(data, position);
                var valueCount = data[position + 2];
                position += 3;

                if (nameOffset >= textLength)
                {
                    throw new FormatException("invalid entry text offset, aborting");
                }

                if (valueCount * 5 > textStart - position)
                {
                    throw new FormatException("truncated entry value, aborting");
                }

                AppendEntryName(output, ReadText(data, textStart, textLength, nameOffset, defaultEncoding));

                for (var j = 0; j < valueCount; j++)
                {
                    var type = data[position + j * 5];
                    var raw = ReadUInt32(data, position + j * 5 + 1);

                    output.Append(j == 0 ? ' ' : ", ");
                    switch ((BiniValueType)type)
                    {
                        case BiniValueType.Integer:
                            output.Append(raw.ToString(CultureInfo.InvariantCulture));
                            break;
                        case BiniValueType.Float:
                            output.Append(FormatFloat(BitConverter.UInt32BitsToSingle(raw)));
                            break;
                        case BiniValueType.String:
                            if (raw >= textLength)
                            {
                                throw new FormatException("invalid value text offset, aborting");
                            }

                            AppendValueString(output, ReadText(data, textStart, textLength, checked((int)raw), defaultEncoding));
                            break;
                        default:
                            throw new FormatException($"bad value type, {type}");
                    }
                }

                output.Append('\n');
                position += valueCount * 5;
            }
        }

        return output.ToString();
    }

    private static uint ReadUInt32(byte[] data, int offset)
    {
        return (uint)(data[offset] |
                        data[offset + 1] << 8 |
                        data[offset + 2] << 16 |
                        data[offset + 3] << 24);
    }

    private static ushort ReadUInt16(byte[] data, int offset)
    {
        return (ushort)(data[offset] | data[offset + 1] << 8);
    }

    private static string ReadText(byte[] data, int textStart, int textLength, int offset, Encoding defaultEncoding)
    {
        var absolute = textStart + offset;
        var end = absolute;
        var limit = textStart + textLength;

        while (end < limit && data[end] != 0)
        {
            end++;
        }

        return defaultEncoding.GetString(data, absolute, end - absolute);
    }

    private static void AppendSectionName(StringBuilder builder, string value)
    {
        builder.Append('[');
        AppendSpecial(builder, value, "\"[] \f\n\r\t\v");
        builder.Append("]\n");
    }

    private static void AppendEntryName(StringBuilder builder, string value)
    {
        AppendSpecial(builder, value, "\"=[] \f\n\r\t\v");
        builder.Append(" =");
    }

    private static void AppendValueString(StringBuilder builder, string value)
    {
        // Numeric-looking strings must be quoted, otherwise a text-to-BINI roundtrip would change their value type.
        if (LooksNumeric(value))
        {
            AppendSpecial(builder, value, null);
            return;
        }

        AppendSpecial(builder, value, "\", \f\n\r\t\v");
    }

    private static bool LooksNumeric(string value)
    {
        return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _) ||
                long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
    }

    private static void AppendSpecial(StringBuilder builder, string value, string? special)
    {
        var simple = value.Length > 0 && special is not null && value.IndexOfAny(special.ToCharArray()) < 0;
        if (simple)
        {
            builder.Append(value);
            return;
        }

        builder.Append('"');
        foreach (var c in value)
        {
            if (c == '"')
            {
                builder.Append('"');
            }

            builder.Append(c);
        }

        builder.Append('"');
    }

    private static string FormatFloat(float value)
    {
        // Preserve the distinct bit pattern for negative zero; plain formatting would collapse it to "0".
        if (BitConverter.SingleToUInt32Bits(value) == 0x80000000)
        {
            return "-0.0";
        }

        var text = value.ToString("G9", CultureInfo.InvariantCulture);
        // Prefer the shortest representation that parses back to the exact original float bits.
        for (var precision = 8; precision > 0; precision--)
        {
            var candidate = value.ToString("G" + precision.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            if (float.TryParse(candidate, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) &&
                BitConverter.SingleToUInt32Bits(parsed) == BitConverter.SingleToUInt32Bits(value) &&
                candidate.Length < text.Length)
            {
                text = candidate;
            }
        }

        return text.Contains('.') || text.Contains('E') || text.Contains('e') || float.IsNaN(value) || float.IsInfinity(value)
            ? text
            : text + ".0";
    }
}
