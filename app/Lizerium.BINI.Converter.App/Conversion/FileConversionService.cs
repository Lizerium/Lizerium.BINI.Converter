/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:25:43
 * Version: 1.0.10
 */

namespace Lizerium.BINI.Converter.App.Conversion;

internal sealed class FileConversionService(IConversionReporter reporter)
{
    /// <summary>
    /// Dispatches a conversion request to either single-file or recursive directory mode.
    /// </summary>
    public ConvertSummary Convert(FileConversionOptions options)
    {
        return File.GetAttributes(options.InputPath).HasFlag(FileAttributes.Directory)
            ? ConvertDirectory(options)
            : ConvertFile(options.InputPath, ResolveFileOutputPath(options.InputPath, options), options);
    }

    public ConvertSummary ConvertDirectory(FileConversionOptions options)
    {
        var files = Directory.EnumerateFiles(options.InputPath, "*.ini", SearchOption.AllDirectories);
        var summary = new ConvertSummary();
        var outputRoot = options.OutputPath;

        // Directory mode mirrors the input tree unless the user explicitly overwrites files in place.
        if (!options.Overwrite)
        {
            outputRoot ??= options.InputPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + ".converted";
            Directory.CreateDirectory(outputRoot);
        }

        foreach (var file in files)
        {
            var output = options.Overwrite
                ? file
                : Path.Combine(outputRoot!, Path.GetRelativePath(options.InputPath, file));

            var fileSummary = ConvertFile(file, output, options);
            summary.Add(fileSummary);

            if (!options.SkipInvalid && fileSummary.Failed > 0)
            {
                break;
            }
        }

        return summary;
    }

    public ConvertSummary ConvertFile(string inputPath, string outputPath, FileConversionOptions options)
    {
        var summary = new ConvertSummary();

        try
        {
            var inputBytes = File.ReadAllBytes(inputPath);
            var result = ConversionEngine.Convert(inputBytes, options.Direction, options.Encoding);

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPath))!);
            File.WriteAllBytes(outputPath, result.Bytes);

            reporter.Converted(result.Direction, inputPath, outputPath);
            summary.Converted++;
        }
        catch (FormatException ex) when (options.SkipInvalid)
        {
            reporter.Skipped(inputPath, ex.Message);
            summary.Skipped++;
        }
        catch (Exception ex)
        {
            reporter.Failed(inputPath, ex.Message);
            summary.Failed++;
        }

        return summary;
    }

    public static string ResolveFileOutputPath(string inputPath, FileConversionOptions options)
    {
        if (options.Overwrite)
        {
            return inputPath;
        }

        if (!string.IsNullOrWhiteSpace(options.OutputPath))
        {
            return options.OutputPath;
        }

        // Default names make the conversion direction visible while preserving the original extension.
        var bytes = File.ReadAllBytes(inputPath);
        var direction = ConversionEngine.ResolveDirection(bytes, options.Direction);
        var directory = Path.GetDirectoryName(inputPath) ?? "";
        var name = Path.GetFileNameWithoutExtension(inputPath);
        var extension = Path.GetExtension(inputPath);
        var suffix = direction == ConvertDirection.ToText ? ".text" : ".bini";

        return Path.Combine(directory, name + suffix + extension);
    }
}
