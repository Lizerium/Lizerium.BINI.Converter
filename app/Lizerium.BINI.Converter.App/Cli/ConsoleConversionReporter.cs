/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:25:43
 * Version: 1.0.10
 */

using Lizerium.BINI.Converter.App.Conversion;

namespace Lizerium.BINI.Converter.App.Cli;

internal sealed class ConsoleConversionReporter : IConversionReporter
{
    public void Converted(ConvertDirection direction, string inputPath, string outputPath)
    {
        Console.WriteLine($"{direction}: {inputPath} -> {outputPath}");
    }

    public void Skipped(string inputPath, string reason)
    {
        Console.WriteLine($"skip: {inputPath} ({reason})");
    }

    public void Failed(string inputPath, string reason)
    {
        Console.Error.WriteLine($"fail: {inputPath} ({reason})");
    }

    public void PrintSummary(ConvertSummary summary)
    {
        Console.WriteLine($"Converted: {summary.Converted}");
        Console.WriteLine($"Skipped:   {summary.Skipped}");
        Console.WriteLine($"Failed:    {summary.Failed}");
    }
}
