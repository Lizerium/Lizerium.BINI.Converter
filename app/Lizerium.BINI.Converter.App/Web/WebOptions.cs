/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 09:56:42
 * Version: 1.0.8
 */

namespace Lizerium.BINI.Converter.App.Web;

internal sealed class WebOptions
{
    public string Urls { get; init; } = "http://localhost:5087";

    /// <summary>
    /// Detects whether the current command line is asking for the local web overlay.
    /// </summary>
    public static bool TryParse(string[] args, out WebOptions options)
    {
        options = new WebOptions();

        if (!args.Any(static x => x is "--web" or "web"))
        {
            return false;
        }

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--urls":
                    i++;
                    if (i >= args.Length)
                    {
                        throw new ArgumentException("--urls requires a value.");
                    }

                    options = new WebOptions { Urls = args[i] };
                    break;
            }
        }

        return true;
    }
}
