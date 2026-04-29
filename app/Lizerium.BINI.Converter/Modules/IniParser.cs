/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 29 апреля 2026 06:52:20
 * Version: 1.0.11
 */

using System.Globalization;
using System.Text;

using Lizerium.BINI.Converter.Models;

namespace Lizerium.BINI.Converter.Modules;

/// <summary>
/// Parses editable INI text into the in-memory shape expected by the BINI writer.
/// </summary>
public sealed class IniParser
{
    private readonly string _text;
    private readonly bool _allowFreelancerExtensions;
    private int _position;
    private int _line = 1;

    public IniParser(string text, bool allowFreelancerExtensions = true)
    {
        _text = text;
        _allowFreelancerExtensions = allowFreelancerExtensions;
    }

    public BiniDocument Parse()
    {
        var sections = new List<BiniSection>();

        while (true)
        {
            var section = ParseSection();
            if (section is null)
            {
                return new BiniDocument(sections);
            }

            sections.Add(section);
        }
    }

    private BiniSection? ParseSection()
    {
        // A null section marks EOF after whitespace or comments.
        if (!SkipSpace())
        {
            return null;
        }

        var c = Get();
        if (c != '[')
        {
            Error($"unexpected '{Printable(c)}', expected '['");
        }

        if (!SkipSectionNamePrefixSpace())
        {
            Error("unexpected end of file");
        }

        var name = ReadNameUntil(']');

        if (!SkipSectionNameSuffixSpace())
        {
            Error("unexpected end of file");
        }

        c = Get();
        if (c != ']')
        {
            Error($"unexpected '{Printable(c)}', expected ']'");
        }

        var entries = new List<BiniEntry>();
        while (true)
        {
            var entry = ParseEntry();
            if (entry is null)
            {
                return new BiniSection(name, entries);
            }

            if (entries.Count == ushort.MaxValue)
            {
                Error("too many entries in one section");
            }

            entries.Add(entry);
        }
    }

    private BiniEntry? ParseEntry()
    {
        // Entries stop when the next non-comment token starts a new section.
        if (!SkipSpace())
        {
            return null;
        }

        var c = Get();
        if (c == '[')
        {
            Unget();
            return null;
        }

        var name = c == '"' ? ReadQuotedBody() : ReadSimpleBody('=', alreadyRead: true);
        var values = new List<BiniValue>();

        if (!SkipBlank())
        {
            if (!_allowFreelancerExtensions)
            {
                Error("unexpected EOF in entry, expected '='");
            }

            return new BiniEntry(name, values);
        }

        c = Get();
        if (c is '\r' or '\n' or ';' or -1)
        {
            if (!_allowFreelancerExtensions)
            {
                Error("unexpected EOF in entry, expected '='");
            }

            if (c == '\r')
            {
                ConsumeOptionalLineFeed();
            }

            return new BiniEntry(name, values);
        }

        if (c != '=')
        {
            if (_allowFreelancerExtensions)
            {
                ConsumeLine(c);
                return new BiniEntry(name, values);
            }

            Error($"unexpected '{Printable(c)}', expected '='");
        }

        if (!SkipBlank())
        {
            return new BiniEntry(name, values);
        }

        c = Get();
        Unget();
        if (c == '\r')
        {
            ConsumeOptionalLineFeed();
            return new BiniEntry(name, values);
        }

        if (c == '\n' || c == ';')
        {
            return new BiniEntry(name, values);
        }

        while (true)
        {
            values.Add(ParseValue(out c));
            if (values.Count > byte.MaxValue)
            {
                Error("too many values in one entry");
            }

            if (c == '\r')
            {
                ConsumeOptionalLineFeed();
                return new BiniEntry(name, values);
            }

            if (c == '\n' || c == -1)
            {
                return new BiniEntry(name, values);
            }

            if (c == ';')
            {
                while ((c = Get()) != -1 && c != '\n')
                {
                }

                return new BiniEntry(name, values);
            }

            if (c != ',')
            {
                Error($"unexpected '{Printable(c)}', expected ','");
            }

            if (!SkipBlank())
            {
                Error("unexpected EOF, expected a value");
            }
        }
    }

    private BiniValue ParseValue(out int next)
    {
        // BINI values are typed, so unquoted tokens are interpreted before falling back to strings.
        var c = Get();
        if (c == '"')
        {
            var value = ReadQuotedBody();
            next = Get();
            return BiniValue.FromString(value);
        }

        if (c == ',')
        {
            if (!_allowFreelancerExtensions)
            {
                Error("missing/empty value");
            }

            next = c;
            return BiniValue.FromString(string.Empty);
        }

        if (c == '\r')
        {
            if (!_allowFreelancerExtensions)
            {
                Error("missing/empty value");
            }

            ConsumeOptionalLineFeed();
            next = '\n';
            return BiniValue.FromString(string.Empty);
        }

        if (c == '\n')
        {
            if (!_allowFreelancerExtensions)
            {
                Error("missing/empty value");
            }

            next = c;
            return BiniValue.FromString(string.Empty);
        }

        if (c == '\r' || c == '\n')
        {
            Error("missing/empty value");
        }

        Unget();
        var token = ReadSimpleBody(',');
        next = Get();

        if (token == "-0")
        {
            return BiniValue.FromFloat(-0.0f);
        }

        if (int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var integer))
        {
            return BiniValue.FromInteger(integer);
        }

        if (uint.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unsignedInteger))
        {
            return BiniValue.FromUnsignedInteger(unsignedInteger);
        }

        if (float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var floating))
        {
            return BiniValue.FromFloat(floating);
        }

        return BiniValue.FromString(token);
    }

    private string ReadNameUntil(char terminator)
    {
        var c = Get();
        if (c == '"')
        {
            return ReadQuotedBody();
        }

        Unget();
        return ReadSimpleBody(terminator, stopAtComment: !_allowFreelancerExtensions || terminator != ']');
    }

    private string ReadQuotedBody()
    {
        var builder = new StringBuilder();
        while (true)
        {
            var c = Get();
            if (c == -1)
            {
                Error("EOF in middle of string");
            }

            if (c == '"')
            {
                // Doubled quotes are the INI escape for a literal quote inside a quoted token.
                var next = Get();
                if (next != '"')
                {
                    if (next != -1)
                    {
                        Unget();
                    }

                    return builder.ToString();
                }
            }

            builder.Append((char)c);
        }
    }

    private string ReadSimpleBody(char terminator, bool alreadyRead = false, bool stopAtComment = true)
    {
        var start = alreadyRead ? _position - 1 : _position;
        while (true)
        {
            var c = Get();
            if (c == -1)
            {
                break;
            }

            if (c == terminator || c == '\n' || c == '\r' || (stopAtComment && c == ';'))
            {
                Unget();
                break;
            }
        }

        return TrimBiniWhiteSpace(_text[start.._position]);
    }

    private bool SkipSpace()
    {
        // Freelancer files commonly use semicolon comments, and some generated files also use '@' comment lines.
        while (true)
        {
            int c;
            for (c = Get(); c != -1 && IsWhiteSpace(c); c = Get())
            {
            }

            if (c == -1)
            {
                return false;
            }

            if (c == ';' || (_allowFreelancerExtensions && c == '@'))
            {
                for (c = Get(); c != -1 && c != '\n'; c = Get())
                {
                }

                if (c == -1)
                {
                    return false;
                }
            }
            else
            {
                Unget();
                return true;
            }
        }
    }

    private bool SkipBlank()
    {
        int c;
        for (c = Get(); c != -1 && IsPlainWhiteSpace(c); c = Get())
        {
        }

        if (c != -1)
        {
            Unget();
        }

        return c != -1;
    }

    private bool SkipSectionNamePrefixSpace()
    {
        return _allowFreelancerExtensions ? SkipWhiteSpaceOnly() : SkipSpace();
    }

    private bool SkipSectionNameSuffixSpace()
    {
        return SkipSpace();
    }

    private bool SkipWhiteSpaceOnly()
    {
        int c;
        for (c = Get(); c != -1 && IsWhiteSpace(c); c = Get())
        {
        }

        if (c != -1)
        {
            Unget();
        }

        return c != -1;
    }

    private int Get()
    {
        if (_position >= _text.Length)
        {
            return -1;
        }

        var c = _text[_position++];
        if (c == '\0')
        {
            Error("invalid NUL byte");
        }

        if (c == '\n')
        {
            _line++;
        }

        return c;
    }

    private void Unget()
    {
        _position--;
        if (_position >= 0 && _text[_position] == '\n')
        {
            _line--;
        }
    }

    private void ConsumeOptionalLineFeed()
    {
        var c = Get();
        if (c != '\n' && c != -1)
        {
            Unget();
        }
    }

    private void ConsumeLine(int current)
    {
        var c = current;
        while (c != -1 && c != '\n')
        {
            c = Get();
        }
    }

    private void Error(string message)
    {
        throw new FormatException($"stdin:{_line}: {message}");
    }

    private static bool IsWhiteSpace(char c) => IsWhiteSpace((int)c);

    private static string TrimBiniWhiteSpace(string value)
    {
        var start = 0;
        var end = value.Length;

        while (start < end && IsWhiteSpace(value[start]))
        {
            start++;
        }

        while (end > start && IsWhiteSpace(value[end - 1]))
        {
            end--;
        }

        return value[start..end];
    }

    private static bool IsWhiteSpace(int c)
    {
        return c is ' ' or '\f' or '\n' or '\r' or '\t' or '\v';
    }

    private static bool IsPlainWhiteSpace(int c)
    {
        return c is ' ' or '\f' or '\r' or '\t' or '\v';
    }

    private static string Printable(int c)
    {
        return c == -1 ? "EOF" : ((char)c).ToString();
    }
}
