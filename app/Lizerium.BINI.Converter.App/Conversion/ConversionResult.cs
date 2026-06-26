/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 июня 2026 06:52:16
 * Version: 1.0.79
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed record ConversionResult(ConvertDirection Direction, byte[] Bytes);
