/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 14 сентября 2026 09:51:06
 * Version: 1.0.160
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal interface IConversionReporter
{
    void Converted(ConvertDirection direction, string inputPath, string outputPath);

    void Skipped(string inputPath, string reason);

    void Failed(string inputPath, string reason);
}
