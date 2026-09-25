/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 сентября 2026 09:35:17
 * Version: 1.0.171
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed record ConversionResult(ConvertDirection Direction, byte[] Bytes);
