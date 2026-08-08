/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 08 августа 2026 06:52:28
 * Version: 1.0.123
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed record ConversionResult(ConvertDirection Direction, byte[] Bytes);
