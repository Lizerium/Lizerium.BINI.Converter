/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:25:43
 * Version: 1.0.10
 */

using System.Text;

using Lizerium.BINI.Converter.Modules;

namespace Lizerium.BINI.Converter;

/// <summary>
/// Public facade for converting between Freelancer BINI bytes and editable INI text.
/// </summary>
public static partial class BiniConverter
{
    private static readonly Encoding DefaultEncoding = BiniEncodings.Windows1251;

    /// <summary>
    /// Checks the BINI magic and version byte without parsing the whole payload.
    /// </summary>
    public static bool IsBini(ReadOnlySpan<byte> data)
    {
        return data.Length >= 5 &&
               data[0] == (byte)'B' &&
               data[1] == (byte)'I' &&
               data[2] == (byte)'N' &&
               data[3] == (byte)'I' &&
               data[4] == 1;
    }

    public static byte[] ConvertTextToBini(string text)
    {
        return ConvertTextToBini(text, DefaultEncoding);
    }

    public static byte[] ConvertTextToBini(string text, Encoding? encoding)
    {
        ArgumentNullException.ThrowIfNull(text);

        var parser = new IniParser(text);
        var document = parser.Parse();
        return BiniWriter.Write(document, encoding ?? DefaultEncoding);
    }

    public static byte[] ConvertTextToBiniStrict(string text)
    {
        return ConvertTextToBiniStrict(text, DefaultEncoding);
    }

    public static byte[] ConvertTextToBiniStrict(string text, Encoding? encoding)
    {
        ArgumentNullException.ThrowIfNull(text);

        // Strict mode is useful for fixture validation because it disables Freelancer's loose INI extensions.
        var parser = new IniParser(text, allowFreelancerExtensions: false);
        var document = parser.Parse();
        return BiniWriter.Write(document, encoding ?? DefaultEncoding);
    }

    public static byte[] ConvertTextBytesToBini(byte[] textBytes, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(textBytes);

        // Decode and re-encode with the same encoding so the BINI string table uses the expected code page.
        var resolvedEncoding = encoding ?? DefaultEncoding;
        return ConvertTextToBini(resolvedEncoding.GetString(textBytes), resolvedEncoding);
    }

    public static string ConvertBiniToText(byte[] biniBytes)
    {
        return ConvertBiniToText(biniBytes, DefaultEncoding);
    }

    public static string ConvertBiniToText(byte[] biniBytes, Encoding? encoding)
    {
        ArgumentNullException.ThrowIfNull(biniBytes);

        return BiniReader.Read(biniBytes, encoding ?? DefaultEncoding);
    }

    public static byte[] ConvertBiniToTextBytes(byte[] biniBytes, Encoding? encoding = null)
    {
        ArgumentNullException.ThrowIfNull(biniBytes);

        var resolvedEncoding = encoding ?? DefaultEncoding;
        return resolvedEncoding.GetBytes(ConvertBiniToText(biniBytes, resolvedEncoding));
    }

    public static void ConvertTextFileToBiniFile(string inputPath, string outputPath, Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        var bytes = File.ReadAllBytes(inputPath);
        File.WriteAllBytes(outputPath, ConvertTextBytesToBini(bytes, encoding));
    }

    public static void ConvertBiniFileToTextFile(string inputPath, string outputPath, Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        var bytes = File.ReadAllBytes(inputPath);
        File.WriteAllBytes(outputPath, ConvertBiniToTextBytes(bytes, encoding));
    }
}
