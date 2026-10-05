/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 05 октября 2026 08:11:39
 * Version: 1.0.181
 */

using Lizerium.BINI.Converter.App.Cli;
using Lizerium.BINI.Converter.App.Web;

namespace Lizerium.BINI.Converter.App;

internal static class AppHost
{
    public static Task<int> RunAsync(string[] args)
    {
        if (WebOptions.TryParse(args, out var webOptions))
        {
            return WebOverlay.RunAsync(webOptions);
        }

        return Task.FromResult(CliApplication.Run(args));
    }
}
