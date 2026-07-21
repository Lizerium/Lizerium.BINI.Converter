/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 21 июля 2026 13:30:34
 * Version: 1.0.105
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
