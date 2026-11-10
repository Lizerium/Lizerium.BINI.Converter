/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 10 ноября 2026 11:43:00
 * Version: 1.0.216
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal interface IConversionReporter
{
    void Converted(ConvertDirection direction, string inputPath, string outputPath);

    void Skipped(string inputPath, string reason);

    void Failed(string inputPath, string reason);
}
