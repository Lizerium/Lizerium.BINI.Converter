/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:01
 * Version: 1.0.7
 */

using System.Text;

namespace Lizerium.BINI.Converter.App.Conversion;

internal static class TextEncodingResolver
{
    public static Encoding Default { get; } = BiniEncodings.Windows1251;

    public static Encoding Parse(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "1251" or "cp1251" or "windows-1251" or "win1251" => BiniEncodings.Windows1251,
            "latin1" or "latin-1" or "iso-8859-1" => BiniEncodings.Latin1,
            "utf8" or "utf-8" => Encoding.UTF8,
            _ => throw new ArgumentException($"Unknown encoding: {value}")
        };
    }
}
