/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 10 сентября 2026 10:07:08
 * Version: 1.0.156
 */

using System.Text;

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed class FileConversionOptions
{
    public required string InputPath { get; init; }

    public string? OutputPath { get; init; }

    public ConvertDirection Direction { get; init; } = ConvertDirection.Auto;

    public Encoding Encoding { get; init; } = TextEncodingResolver.Default;

    public bool Overwrite { get; init; }

    public bool SkipInvalid { get; init; } = true;
}
