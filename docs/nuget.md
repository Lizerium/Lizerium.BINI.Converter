# NuGet Package Usage

`Lizerium.BINI.Converter` is the reusable library package. Use it when you want BINI conversion inside another .NET application, tool, launcher, editor, or pipeline.

## Install

From NuGet:

```powershell
dotnet add package Lizerium.BINI.Converter
```

From a local package built from this repository:

```powershell
dotnet pack app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj -c Release
dotnet add package Lizerium.BINI.Converter --source app/Lizerium.BINI.Converter/bin/Release
```

## Convert BINI To Text INI

```csharp
using Lizerium.BINI.Converter;

byte[] bini = File.ReadAllBytes("market_commodities.ini");
string textIni = BiniConverter.ConvertBiniToText(bini);

File.WriteAllText("market_commodities.text.ini", textIni);
```

## Convert Text INI To BINI

```csharp
using Lizerium.BINI.Converter;

string textIni = File.ReadAllText("market_commodities.text.ini");
byte[] bini = BiniConverter.ConvertTextToBini(textIni);

File.WriteAllBytes("market_commodities.ini", bini);
```

## Work With Byte Arrays

```csharp
using System.Text;
using Lizerium.BINI.Converter;

byte[] textBytes = File.ReadAllBytes("shiparch.text.ini");
byte[] biniBytes = BiniConverter.ConvertTextBytesToBini(textBytes, Encoding.Latin1);

byte[] restoredTextBytes = BiniConverter.ConvertBiniToTextBytes(biniBytes, Encoding.Latin1);
```

If no encoding is passed, byte/file APIs use `BiniEncodings.Windows1251`, because many Freelancer files and localized mods store strings as Windows-1251. Pass `BiniEncodings.Latin1` when you need byte-preserving behavior for artificial fixtures or legacy data that is not really CP1251 text.

## Work With Files

```csharp
using Lizerium.BINI.Converter;

BiniConverter.ConvertBiniFileToTextFile(
    inputPath: "market_commodities.ini",
    outputPath: "market_commodities.text.ini");

BiniConverter.ConvertTextFileToBiniFile(
    inputPath: "market_commodities.text.ini",
    outputPath: "market_commodities.ini");
```

## Detect Format

```csharp
using Lizerium.BINI.Converter;

byte[] data = File.ReadAllBytes("file.ini");

if (BiniConverter.IsBini(data))
{
    Console.WriteLine("Binary BINI");
}
else
{
    Console.WriteLine("Text INI");
}
```

## Strict Text Parsing

The default text parser allows Freelancer-style extensions. Use strict mode when you want tighter INI validation:

```csharp
using Lizerium.BINI.Converter;

byte[] bini = BiniConverter.ConvertTextToBiniStrict(File.ReadAllText("input.ini"));
```

## Public API

- `IsBini(ReadOnlySpan<byte> data)` - checks the BINI signature.
- `ConvertTextToBini(string text)` - converts text INI to BINI bytes.
- `ConvertTextToBini(string text, Encoding? encoding)` - converts text INI to BINI bytes with an explicit string table encoding.
- `ConvertTextToBiniStrict(string text)` - converts text INI with stricter parser rules.
- `ConvertTextToBiniStrict(string text, Encoding? encoding)` - strict conversion with an explicit string table encoding.
- `ConvertTextBytesToBini(byte[] textBytes, Encoding? encoding = null)` - converts encoded text bytes to BINI bytes.
- `ConvertBiniToText(byte[] biniBytes)` - converts BINI bytes to text INI.
- `ConvertBiniToText(byte[] biniBytes, Encoding? encoding)` - converts BINI bytes to text INI with an explicit string table encoding.
- `ConvertBiniToTextBytes(byte[] biniBytes, Encoding? encoding = null)` - converts BINI bytes to encoded text bytes.
- `ConvertTextFileToBiniFile(string inputPath, string outputPath, Encoding? encoding = null)` - converts a text INI file to a BINI file.
- `ConvertBiniFileToTextFile(string inputPath, string outputPath, Encoding? encoding = null)` - converts a BINI file to a text INI file.

## Errors

Invalid text INI or corrupted BINI input throws `FormatException`.

A text file with too many unique strings for the BINI string table can throw `InvalidOperationException`.

## Need A Ready-To-Run Tool?

Use `Lizerium.BINI.Converter.App` for file conversion, recursive folder conversion, and the local browser overlay.

See [App, CLI, and web overlay](app.md).

## Need A Browser-Only Version?

The repository also contains a static JavaScript portal in `docs/portal`. It exposes `window.LizeriumBini` and can run from GitHub Pages without a backend.
