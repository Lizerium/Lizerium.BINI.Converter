/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 20 мая 2026 10:03:55
 * Version: 1.0.42
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal interface IConversionReporter
{
    void Converted(ConvertDirection direction, string inputPath, string outputPath);

    void Skipped(string inputPath, string reason);

    void Failed(string inputPath, string reason);
}
