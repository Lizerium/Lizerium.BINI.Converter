/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:01
 * Version: 1.0.7
 */

using System.Text;

namespace Lizerium.BINI.Converter.App.Conversion;

internal static class ConversionEngine
{
    /// <summary>
    /// Converts one in-memory payload using either an explicit direction or signature-based auto detection.
    /// </summary>
    public static ConversionResult Convert(byte[] inputBytes, ConvertDirection requestedDirection, Encoding? encoding = null)
    {
        var direction = ResolveDirection(inputBytes, requestedDirection);
        var resolvedEncoding = encoding ?? TextEncodingResolver.Default;

        var outputBytes = direction switch
        {
            ConvertDirection.ToText => BiniConverter.ConvertBiniToTextBytes(inputBytes, resolvedEncoding),
            ConvertDirection.ToBini => BiniConverter.ConvertTextBytesToBini(inputBytes, resolvedEncoding),
            _ => throw new InvalidOperationException("Direction was not resolved.")
        };

        return new ConversionResult(direction, outputBytes);
    }

    public static ConvertDirection ResolveDirection(ReadOnlySpan<byte> inputBytes, ConvertDirection requestedDirection)
    {
        // Auto mode treats anything without the BINI signature as editable text INI.
        return requestedDirection == ConvertDirection.Auto
            ? BiniConverter.IsBini(inputBytes) ? ConvertDirection.ToText : ConvertDirection.ToBini
            : requestedDirection;
    }
}
