/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:01
 * Version: 1.0.7
 */

using System.Text;

using Lizerium.BINI.Converter.Models;

namespace Lizerium.BINI.Converter.Modules;

public static class BiniWriter
{
    /// <summary>
    /// Serializes a parsed INI document into Freelancer's BINI binary layout.
    /// </summary>
    public static byte[] Write(BiniDocument document, Encoding defaultEncoding)
    {
        var strings = new StringTableBuilder();
        var structsLength = 12;

        // First pass: collect every unique string and compute the byte length of the structured record area.
        foreach (var section in document.Sections)
        {
            strings.Add(section.Name);
            structsLength += 4;

            foreach (var entry in section.Entries)
            {
                strings.Add(entry.Name);
                structsLength += 3 + entry.Values.Count * 5;

                foreach (var value in entry.Values)
                {
                    if (value.Type == BiniValueType.String)
                    {
                        strings.Add(value.Text!);
                    }
                }
            }
        }

        strings.FinalizeOffsets(defaultEncoding);

        // Second pass: write records with offsets into the string table, then append the table itself.
        using var output = new MemoryStream();
        WriteUInt32(output, 0x494e4942);
        WriteUInt32(output, 1);
        WriteUInt32(output, (uint)structsLength);

        foreach (var section in document.Sections)
        {
            WriteUInt16(output, strings.OffsetOf(section.Name));
            WriteUInt16(output, checked((ushort)section.Entries.Count));

            foreach (var entry in section.Entries)
            {
                WriteUInt16(output, strings.OffsetOf(entry.Name));
                output.WriteByte(checked((byte)entry.Values.Count));

                foreach (var value in entry.Values)
                {
                    output.WriteByte((byte)value.Type);
                    switch (value.Type)
                    {
                        case BiniValueType.Integer:
                            WriteUInt32(output, value.Integer);
                            break;
                        case BiniValueType.Float:
                            WriteUInt32(output, BitConverter.SingleToUInt32Bits(value.Floating));
                            break;
                        case BiniValueType.String:
                            WriteUInt32(output, strings.OffsetOf(value.Text!));
                            break;
                    }
                }
            }
        }

        strings.WriteTo(output, defaultEncoding);
        return output.ToArray();
    }

    private static void WriteUInt16(Stream stream, ushort value)
    {
        stream.WriteByte((byte)value);
        stream.WriteByte((byte)(value >> 8));
    }

    private static void WriteUInt32(Stream stream, uint value)
    {
        stream.WriteByte((byte)value);
        stream.WriteByte((byte)(value >> 8));
        stream.WriteByte((byte)(value >> 16));
        stream.WriteByte((byte)(value >> 24));
    }
}
