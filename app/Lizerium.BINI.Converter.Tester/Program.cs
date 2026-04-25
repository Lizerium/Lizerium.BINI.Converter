/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 апреля 2026 08:10:59
 * Version: 1.0.6
 */

using System.Text;

using Lizerium.BINI.Converter;

var tests = new (string Name, Action Test)[]
{
    ("IsBini detects converted data", IsBiniDetectsConvertedData),
    ("ConvertTextToBini creates a BINI payload", ConvertTextToBiniCreatesBiniPayload),
    ("ConvertTextBytesToBini accepts encoded text bytes", ConvertTextBytesToBiniAcceptsBytes),
    ("ConvertBiniToText restores sections and values", ConvertBiniToTextRestoresText),
    ("ConvertBiniToTextBytes returns encoded text bytes", ConvertBiniToTextBytesReturnsBytes),
    ("ConvertTextFileToBiniFile writes a BINI file", ConvertTextFileToBiniFileWritesFile),
    ("ConvertBiniFileToTextFile writes a text file", ConvertBiniFileToTextFileWritesFile),
    ("binitools valid fixtures are idempotent", BinitoolsValidFixturesAreIdempotent),
    ("binitools invalid fixtures are rejected", BinitoolsInvalidFixturesAreRejected),
    ("Freelancer folder INI files roundtrip", FreelancerFolderIniFilesRoundtrip),
    ("Invalid text raises FormatException", InvalidTextRaisesFormatException),
    ("Invalid BINI raises FormatException", InvalidBiniRaisesFormatException)
};

var failed = 0;
foreach (var (name, test) in tests)
{
    try
    {
        test();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception ex)
    {
        failed++;
        Console.WriteLine($"FAIL {name}");
        Console.WriteLine($"     {ex.GetType().Name}: {ex.Message}");
    }
}

if (failed > 0)
{
    Environment.ExitCode = 1;
}

static void IsBiniDetectsConvertedData()
{
    var bini = BiniConverter.ConvertTextToBini(SampleIni());
    Assert(BiniConverter.IsBini(bini), "Converted payload was not detected as BINI.");
    Assert(!BiniConverter.IsBini(Encoding.Latin1.GetBytes(SampleIni())), "Plain text was detected as BINI.");
}

static void ConvertTextToBiniCreatesBiniPayload()
{
    var bini = BiniConverter.ConvertTextToBini(SampleIni());
    Assert(bini.Length > 12, "BINI payload is too small.");
    Assert(Encoding.ASCII.GetString(bini, 0, 4) == "BINI", "BINI magic is missing.");
}

static void ConvertTextBytesToBiniAcceptsBytes()
{
    var bytes = Encoding.Latin1.GetBytes(SampleIni());
    var bini = BiniConverter.ConvertTextBytesToBini(bytes);
    Assert(BiniConverter.IsBini(bini), "Byte conversion did not produce BINI.");
}

static void ConvertBiniToTextRestoresText()
{
    var bini = BiniConverter.ConvertTextToBini(SampleIni());
    var text = BiniConverter.ConvertBiniToText(bini);

    Assert(text.Contains("[Commodities]", StringComparison.Ordinal), "Section was not restored.");
    Assert(text.Contains("iron = 1.42", StringComparison.Ordinal), "Float value was not restored.");
    Assert(text.Contains("\"+1\"", StringComparison.Ordinal), "Numeric-looking string was not quoted.");
    Assert(text.Contains("-0.0", StringComparison.Ordinal), "Negative zero float was not preserved as a float.");
}

static void ConvertBiniToTextBytesReturnsBytes()
{
    var bini = BiniConverter.ConvertTextToBini(SampleIni());
    var textBytes = BiniConverter.ConvertBiniToTextBytes(bini);
    var text = Encoding.Latin1.GetString(textBytes);

    Assert(text.Contains("Laser Beam, \"\"Red\"\"", StringComparison.Ordinal), "Escaped quoted string was not restored.");
}

static void ConvertTextFileToBiniFileWritesFile()
{
    var directory = CreateTempDirectory();
    var input = Path.Combine(directory, "input.ini");
    var output = Path.Combine(directory, "output.ini");

    File.WriteAllText(input, SampleIni(), Encoding.Latin1);
    BiniConverter.ConvertTextFileToBiniFile(input, output);

    Assert(File.Exists(output), "Output file was not created.");
    Assert(BiniConverter.IsBini(File.ReadAllBytes(output)), "Output file is not BINI.");
}

static void ConvertBiniFileToTextFileWritesFile()
{
    var directory = CreateTempDirectory();
    var input = Path.Combine(directory, "input.ini");
    var output = Path.Combine(directory, "output.txt.ini");

    File.WriteAllBytes(input, BiniConverter.ConvertTextToBini(SampleIni()));
    BiniConverter.ConvertBiniFileToTextFile(input, output);

    Assert(File.Exists(output), "Output file was not created.");
    Assert(File.ReadAllText(output, Encoding.Latin1).Contains("[Weapons]", StringComparison.Ordinal), "Output text is missing a section.");
}

static void BinitoolsValidFixturesAreIdempotent()
{
    var validRoot = Path.Combine(AppContext.BaseDirectory, "TestData", "valid");
    Assert(Directory.Exists(validRoot), $"binitools valid fixtures were not copied: {validRoot}");

    var files = Directory.EnumerateFiles(validRoot, "*.ini").OrderBy(static x => x, StringComparer.Ordinal).ToArray();
    Assert(files.Length > 0, "No binitools valid fixtures were found.");

    var failures = new List<string>();
    foreach (var file in files)
    {
        try
        {
            var text = Encoding.Latin1.GetString(File.ReadAllBytes(file));
            var firstBini = BiniConverter.ConvertTextToBini(text, BiniEncodings.Latin1);
            var canonicalText = BiniConverter.ConvertBiniToText(firstBini, BiniEncodings.Latin1);
            var secondBini = BiniConverter.ConvertTextToBini(canonicalText, BiniEncodings.Latin1);

            Assert(firstBini.SequenceEqual(secondBini), $"Fixture is not idempotent: {Path.GetFileName(file)}");
        }
        catch (Exception ex)
        {
            failures.Add($"{Path.GetFileName(file)}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    Console.WriteLine($"binitools valid fixtures checked: {files.Length}");
    if (failures.Count > 0)
    {
        throw new InvalidOperationException("binitools valid fixture failures:\n" + string.Join('\n', failures.Take(20)));
    }
}

static void BinitoolsInvalidFixturesAreRejected()
{
    var invalidRoot = Path.Combine(AppContext.BaseDirectory, "TestData", "invalid");
    Assert(Directory.Exists(invalidRoot), $"binitools invalid fixtures were not copied: {invalidRoot}");

    var files = Directory.EnumerateFiles(invalidRoot, "*.ini").OrderBy(static x => x, StringComparer.Ordinal).ToArray();
    Assert(files.Length > 0, "No binitools invalid fixtures were found.");

    var accepted = new List<string>();
    foreach (var file in files)
    {
        try
        {
            var text = Encoding.Latin1.GetString(File.ReadAllBytes(file));
            _ = BiniConverter.ConvertTextToBiniStrict(text, BiniEncodings.Latin1);
            accepted.Add(Path.GetFileName(file));
        }
        catch (Exception)
        {
            // Expected: original binitools invalid fixtures must be rejected.
        }
    }

    Console.WriteLine($"binitools invalid fixtures checked: {files.Length}");
    if (accepted.Count > 0)
    {
        throw new InvalidOperationException("binitools invalid fixtures were accepted:\n" + string.Join('\n', accepted.Take(20)));
    }
}

static void FreelancerFolderIniFilesRoundtrip()
{
    const string sourceRoot = @"C:\Program Files (x86)\Freelancer";
    if (!Directory.Exists(sourceRoot))
    {
        Console.WriteLine($"SKIP Freelancer folder was not found: {sourceRoot}");
        return;
    }

    var workRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestWork", "Freelancer"));
    RecreateDirectory(workRoot);

    var files = Directory.EnumerateFiles(sourceRoot, "*.ini", SearchOption.AllDirectories).ToArray();
    var failures = new List<string>();
    var checkedCount = 0;
    var biniCount = 0;
    var textCount = 0;

    foreach (var source in files)
    {
        var relative = Path.GetRelativePath(sourceRoot, source);
        var copy = Path.Combine(workRoot, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(copy)!);
        File.Copy(source, copy, overwrite: true);

        try
        {
            var originalBytes = File.ReadAllBytes(copy);
            if (BiniConverter.IsBini(originalBytes))
            {
                biniCount++;
                var firstText = BiniConverter.ConvertBiniToText(originalBytes);
                var convertedBack = BiniConverter.ConvertTextToBini(firstText);
                var secondText = BiniConverter.ConvertBiniToText(convertedBack);
                Assert(firstText == secondText, $"BINI canonical text changed for {relative}.");
            }
            else
            {
                textCount++;
                var converted = BiniConverter.ConvertTextBytesToBini(originalBytes);
                var firstText = BiniConverter.ConvertBiniToText(converted);
                var convertedBack = BiniConverter.ConvertTextToBini(firstText);
                var secondText = BiniConverter.ConvertBiniToText(convertedBack);
                Assert(firstText == secondText, $"Text INI canonical text changed for {relative}.");
            }

            checkedCount++;
        }
        catch (Exception ex)
        {
            failures.Add($"{relative}: {ex.GetType().Name}: {ex.Message}");
        }
    }

    Console.WriteLine($"Freelancer INI checked: {checkedCount}, BINI: {biniCount}, text: {textCount}, copied to: {workRoot}");
    if (failures.Count > 0)
    {
        throw new InvalidOperationException("Freelancer roundtrip failures:\n" + string.Join('\n', failures.Take(20)));
    }
}

static void InvalidTextRaisesFormatException()
{
    AssertThrows<FormatException>(() => BiniConverter.ConvertTextToBini("[Broken\nkey = 1"));
}

static void InvalidBiniRaisesFormatException()
{
    AssertThrows<FormatException>(() => BiniConverter.ConvertBiniToText(new byte[] { 1, 2, 3 }));
}

static string SampleIni()
{
    return """""
           ; comments are discarded
           [Commodities]
           iron = 1.42, 300, icons\iron.bmp, "+1", -0

           [Weapons]
           red_laser_beam = 255, 0, 0, "Laser Beam, ""Red"""
           "==SPECIAL==" = 1.0
           """"";
}

static string CreateTempDirectory()
{
    var directory = Path.Combine(Path.GetTempPath(), "Lizerium.BINI.Converter.Tester", Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(directory);
    return directory;
}

static void RecreateDirectory(string directory)
{
    if (Directory.Exists(directory))
    {
        Directory.Delete(directory, recursive: true);
    }

    Directory.CreateDirectory(directory);
}

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

static void AssertThrows<TException>(Action action)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
