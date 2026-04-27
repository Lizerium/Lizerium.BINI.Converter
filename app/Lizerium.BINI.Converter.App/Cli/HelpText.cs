/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 27 апреля 2026 09:41:16
 * Version: 1.0.9
 */

namespace Lizerium.BINI.Converter.App.Cli;

internal static class HelpText
{
    public static void Print()
    {
        Console.WriteLine("""
                          Lizerium.BINI.Converter.App

                          Usage:
                            Lizerium.BINI.Converter.App <file-or-folder> [options]
                            Lizerium.BINI.Converter.App --web [--urls <url>]

                          Options:
                            --mode auto|to-text|to-bini   Conversion direction. Default: auto.
                            --encoding cp1251|latin1|utf8 Text/string encoding. Default: cp1251.
                            --output <path>               Output file path or output folder for directory mode.
                            --overwrite                   Replace source files in place.
                            --stop-on-error               Stop on the first invalid file.
                            --web                         Start the browser drag-and-drop converter.
                            --urls <url>                  Web bind URL. Default: http://localhost:5087.
                            -h, --help                    Show help.

                          Auto mode reads the file signature:
                            BINI .ini -> text INI
                            text INI  -> BINI .ini

                          Without --overwrite and --output:
                            file mode creates *.text.ini or *.bini.ini beside the source file;
                            folder mode creates a sibling <folder>.converted copy with the same structure.
                          """);
    }
}
