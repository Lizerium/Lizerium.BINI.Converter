/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 04 июня 2026 06:52:18
 * Version: 1.0.57
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal interface IConversionReporter
{
    void Converted(ConvertDirection direction, string inputPath, string outputPath);

    void Skipped(string inputPath, string reason);

    void Failed(string inputPath, string reason);
}
