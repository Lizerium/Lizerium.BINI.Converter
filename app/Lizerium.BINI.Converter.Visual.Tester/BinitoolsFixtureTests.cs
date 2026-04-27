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

public sealed class BinitoolsFixtureTests
{
    [Theory]
    [MemberData(nameof(ValidFixtures))]
    public void ValidFixtureIsIdempotent(string file)
    {
        var text = Encoding.Latin1.GetString(File.ReadAllBytes(file));
        var firstBini = BiniConverter.ConvertTextToBini(text, BiniEncodings.Latin1);
        var canonicalText = BiniConverter.ConvertBiniToText(firstBini, BiniEncodings.Latin1);
        var secondBini = BiniConverter.ConvertTextToBini(canonicalText, BiniEncodings.Latin1);

        Assert.Equal(firstBini, secondBini);
    }

    [Theory]
    [MemberData(nameof(InvalidFixtures))]
    public void InvalidFixtureIsRejectedByStrictParser(string file)
    {
        var text = Encoding.Latin1.GetString(File.ReadAllBytes(file));

        Assert.ThrowsAny<Exception>(() => BiniConverter.ConvertTextToBiniStrict(text, BiniEncodings.Latin1));
    }

    public static IEnumerable<object[]> ValidFixtures()
    {
        return EnumerateFixtureFiles("valid");
    }

    public static IEnumerable<object[]> InvalidFixtures()
    {
        return EnumerateFixtureFiles("invalid");
    }

    private static IEnumerable<object[]> EnumerateFixtureFiles(string group)
    {
        var root = Path.Combine(AppContext.BaseDirectory, "TestData", group);
        if (!Directory.Exists(root))
        {
            throw new DirectoryNotFoundException($"Fixture directory was not copied: {root}");
        }

        return Directory
            .EnumerateFiles(root, "*.ini")
            .OrderBy(static x => x, StringComparer.Ordinal)
            .Select(static x => new object[] { x });
    }
}
