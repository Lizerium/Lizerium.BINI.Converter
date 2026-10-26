/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 октября 2026 11:59:15
 * Version: 1.0.201
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed record ConversionResult(ConvertDirection Direction, byte[] Bytes);
