/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 26 апреля 2026 06:56:01
 * Version: 1.0.7
 */

using System.Text;
using Lizerium.BINI.Converter.App.Conversion;

namespace Lizerium.BINI.Converter.App.Cli;

internal sealed class ConvertOptions
{
    public required string InputPath { get; init; }

    public string? OutputPath { get; init; }

    public ConvertDirection Direction { get; init; } = ConvertDirection.Auto;

    public Encoding Encoding { get; init; } = TextEncodingResolver.Default;

    public bool Overwrite { get; init; }

    public bool SkipInvalid { get; init; } = true;

    public bool ShowHelp { get; init; }

    public bool HasError { get; init; }

    public FileConversionOptions ToFileConversionOptions()
    {
        return new FileConversionOptions
        {
            InputPath = InputPath,
            OutputPath = OutputPath,
            Direction = Direction,
            Encoding = Encoding,
            Overwrite = Overwrite,
            SkipInvalid = SkipInvalid
        };
    }

    public static ConvertOptions Parse(string[] args)
    {
        // Keep parsing dependency-free so the app can remain a small self-contained tool.
        if (args.Length == 0)
        {
            return new ConvertOptions { InputPath = "", ShowHelp = true, HasError = true };
        }

        string? inputPath = null;
        string? outputPath = null;
        var direction = ConvertDirection.Auto;
        var encoding = TextEncodingResolver.Default;
        var overwrite = false;
        var skipInvalid = true;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-h":
                case "--help":
                    return new ConvertOptions { InputPath = "", ShowHelp = true };
                case "--output":
                case "-o":
                    outputPath = ReadValue(args, ref i, arg);
                    break;
                case "--mode":
                case "-m":
                    direction = ConvertDirectionParser.Parse(ReadValue(args, ref i, arg));
                    break;
                case "--encoding":
                case "-e":
                    encoding = TextEncodingResolver.Parse(ReadValue(args, ref i, arg));
                    break;
                case "--overwrite":
                    overwrite = true;
                    break;
                case "--stop-on-error":
                    skipInvalid = false;
                    break;
                default:
                    if (arg.StartsWith("-", StringComparison.Ordinal))
                    {
                        throw new ArgumentException($"Unknown option: {arg}");
                    }

                    if (inputPath is not null)
                    {
                        throw new ArgumentException("Only one input path is supported.");
                    }

                    inputPath = arg;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            return new ConvertOptions { InputPath = "", ShowHelp = true, HasError = true };
        }

        if (!File.Exists(inputPath) && !Directory.Exists(inputPath))
        {
            throw new FileNotFoundException($"Input path was not found: {inputPath}", inputPath);
        }

        if (overwrite && outputPath is not null)
        {
            throw new ArgumentException("--overwrite cannot be combined with --output.");
        }

        return new ConvertOptions
        {
            InputPath = inputPath,
            OutputPath = outputPath,
            Direction = direction,
            Encoding = encoding,
            Overwrite = overwrite,
            SkipInvalid = skipInvalid
        };
    }

    private static string ReadValue(string[] args, ref int index, string option)
    {
        index++;
        if (index >= args.Length)
        {
            throw new ArgumentException($"{option} requires a value.");
        }

        return args[index];
    }
}
