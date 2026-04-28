/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:25:43
 * Version: 1.0.10
 */

using System.Text;

namespace Lizerium.BINI.Converter.Modules;

/// <summary>
/// Builds the shared NUL-terminated string table used by BINI records.
/// </summary>
public sealed class StringTableBuilder
{
    private readonly List<string> _strings = new();
    private readonly Dictionary<string, ushort> _offsets = new(StringComparer.Ordinal);

    public void Add(string value)
    {
        if (!_offsets.ContainsKey(value))
        {
            _offsets[value] = 0;
            _strings.Add(value);
        }
    }

    public void FinalizeOffsets(Encoding DefaultEncoding)
    {
        // Offsets are byte offsets, not character counts, so they depend on the chosen text encoding.
        var offset = 0;
        foreach (var value in _strings)
        {
            if (offset > ushort.MaxValue)
            {
                throw new InvalidOperationException("too many strings");
            }

            _offsets[value] = (ushort)offset;
            offset += DefaultEncoding.GetByteCount(value) + 1;
        }
    }

    public ushort OffsetOf(string value) => _offsets[value];

    public void WriteTo(Stream stream, Encoding defaultEncoding)
    {
        foreach (var value in _strings)
        {
            var bytes = defaultEncoding.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
            stream.WriteByte(0);
        }
    }
}
