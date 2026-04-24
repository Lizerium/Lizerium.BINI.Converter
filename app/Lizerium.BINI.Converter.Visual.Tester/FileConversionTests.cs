/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 24 апреля 2026 15:29:41
 * Version: 1.0.
 */

using System.Text;

using Xunit;

namespace Lizerium.BINI.Converter.Visual.Tester;

public sealed class FileConversionTests
{
    [Fact]
    public void ConvertTextFileToBiniFileWritesBiniFile()
    {
        using var workspace = TestWorkspace.Create();
        var input = workspace.WriteTextFile("input.ini", SampleIni.Text);
        var output = workspace.GetPath("output.ini");

        BiniConverter.ConvertTextFileToBiniFile(input, output);

        Assert.True(File.Exists(output));
        Assert.True(BiniConverter.IsBini(File.ReadAllBytes(output)));
    }

    [Fact]
    public void ConvertBiniFileToTextFileWritesTextFile()
    {
        using var workspace = TestWorkspace.Create();
        var input = workspace.WriteBytesFile("input.ini", BiniConverter.ConvertTextToBini(SampleIni.Text));
        var output = workspace.GetPath("output.text.ini");

        BiniConverter.ConvertBiniFileToTextFile(input, output);

        Assert.True(File.Exists(output));
        Assert.Contains("[Weapons]", File.ReadAllText(output, Encoding.Latin1), StringComparison.Ordinal);
    }
}
