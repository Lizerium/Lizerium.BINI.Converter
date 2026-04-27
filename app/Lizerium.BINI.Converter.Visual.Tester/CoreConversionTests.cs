/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 27 апреля 2026 09:41:16
 * Version: 1.0.9
 */

using System.Text;

using Xunit;

namespace Lizerium.BINI.Converter.Visual.Tester;

public sealed class CoreConversionTests
{
    [Fact]
    public void IsBiniDetectsConvertedData()
    {
        var bini = BiniConverter.ConvertTextToBini(SampleIni.Text);

        Assert.True(BiniConverter.IsBini(bini));
        Assert.False(BiniConverter.IsBini(Encoding.Latin1.GetBytes(SampleIni.Text)));
    }

    [Fact]
    public void ConvertTextToBiniCreatesBiniPayload()
    {
        var bini = BiniConverter.ConvertTextToBini(SampleIni.Text);

        Assert.True(bini.Length > 12);
        Assert.Equal("BINI", Encoding.ASCII.GetString(bini, 0, 4));
    }

    [Fact]
    public void ConvertTextBytesToBiniAcceptsEncodedTextBytes()
    {
        var bytes = Encoding.Latin1.GetBytes(SampleIni.Text);
        var bini = BiniConverter.ConvertTextBytesToBini(bytes);

        Assert.True(BiniConverter.IsBini(bini));
    }

    [Fact]
    public void ConvertBiniToTextRestoresSectionsAndValues()
    {
        var bini = BiniConverter.ConvertTextToBini(SampleIni.Text);
        var text = BiniConverter.ConvertBiniToText(bini);

        Assert.Contains("[Commodities]", text, StringComparison.Ordinal);
        Assert.Contains("iron = 1.42", text, StringComparison.Ordinal);
        Assert.Contains("\"+1\"", text, StringComparison.Ordinal);
        Assert.Contains("-0.0", text, StringComparison.Ordinal);
    }

    [Fact]
    public void ConvertBiniToTextBytesReturnsEncodedTextBytes()
    {
        var bini = BiniConverter.ConvertTextToBini(SampleIni.Text);
        var textBytes = BiniConverter.ConvertBiniToTextBytes(bini);
        var text = Encoding.Latin1.GetString(textBytes);

        Assert.Contains("Laser Beam, \"\"Red\"\"", text, StringComparison.Ordinal);
    }

    [Fact]
    public void Windows1251TextRoundtripPreservesCyrillic()
    {
        const string text = """
                            [Character]
                            name = "РђР»РµРєСЃРµР№"
                            description = "Р СѓСЃСЃРєРёР№ С‚РµРєСЃС‚ Freelancer"
                            """;

        var sourceBytes = BiniEncodings.Windows1251.GetBytes(text);
        var bini = BiniConverter.ConvertTextBytesToBini(sourceBytes, BiniEncodings.Windows1251);
        var restoredBytes = BiniConverter.ConvertBiniToTextBytes(bini, BiniEncodings.Windows1251);
        var restoredText = BiniEncodings.Windows1251.GetString(restoredBytes);

        Assert.Contains("РђР»РµРєСЃРµР№", restoredText, StringComparison.Ordinal);
        Assert.Contains("Р СѓСЃСЃРєРёР№ С‚РµРєСЃС‚ Freelancer", restoredText, StringComparison.Ordinal);
    }

    [Fact]
    public void UnsignedIntegerLikeIdsDoNotBecomeFloats()
    {
        const string text = """
                            [locked_gates]
                            locked_gate = 2314954114
                            locked_gate = 2193365380
                            """;

        var bini = BiniConverter.ConvertTextToBini(text);
        var restoredText = BiniConverter.ConvertBiniToText(bini);

        Assert.Contains("locked_gate = 2314954114", restoredText, StringComparison.Ordinal);
        Assert.Contains("locked_gate = 2193365380", restoredText, StringComparison.Ordinal);
        Assert.DoesNotContain("E+09", restoredText, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void InvalidTextRaisesFormatException()
    {
        Assert.Throws<FormatException>(() => BiniConverter.ConvertTextToBini("[Broken\nkey = 1"));
    }

    [Fact]
    public void InvalidBiniRaisesFormatException()
    {
        Assert.Throws<FormatException>(() => BiniConverter.ConvertBiniToText(new byte[] { 1, 2, 3 }));
    }
}
