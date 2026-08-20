/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 20 августа 2026 09:36:35
 * Version: 1.0.135
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal static class ConvertDirectionParser
{
    public static ConvertDirection Parse(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "auto" => ConvertDirection.Auto,
            "to-text" => ConvertDirection.ToText,
            "text" => ConvertDirection.ToText,
            "unbini" => ConvertDirection.ToText,
            "to-bini" => ConvertDirection.ToBini,
            "bini" => ConvertDirection.ToBini,
            _ => throw new ArgumentException($"Unknown mode: {value}")
        };
    }
}
