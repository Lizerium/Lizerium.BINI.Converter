/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 24 апреля 2026 15:29:41
 * Version: 1.0.
 */

using Lizerium.BINI.Converter.App.Conversion;

namespace Lizerium.BINI.Converter.App.Cli;

internal static class CliApplication
{
    public static int Run(string[] args)
    {
        var reporter = new ConsoleConversionReporter();

        try
        {
            var options = ConvertOptions.Parse(args);
            if (options.ShowHelp)
            {
                HelpText.Print();
                return options.HasError ? 1 : 0;
            }

            var converter = new FileConversionService(reporter);
            var result = converter.Convert(options.ToFileConversionOptions());

            reporter.PrintSummary(result);
            return result.Failed == 0 ? 0 : 2;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 2;
        }
    }
}
