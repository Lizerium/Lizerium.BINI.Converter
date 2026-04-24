<h1 align="center">Lizerium.BINI.Converter</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>

  <a href="./README.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  |
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

<div align="center">

  <img alt="NuGet downloads" src="https://shields.dvurechensky.pro/nuget/dt/Lizerium.BINI.Converter?label=NuGet%20downloads">

</div>

---

> [!NOTE]
> This project is part of the **Lizerium** ecosystem and belongs to the following project:
>
> - [`Lizerium.Tools.Structs`](https://github.com/Lizerium/Lizerium.Tools.Structs)
>
> If you're looking for related engineering and support tools, start there.

---

`Lizerium.BINI.Converter` is a .NET 8 toolkit for Freelancer BINI files: unpack binary BINI into editable INI text and pack text INI back into the original game format.

## What It Does

![preview](media/bini_source.gif)

---

- Converts Freelancer `.ini` files both ways: `BINI -> text INI` and `text INI -> BINI`.
- Detects BINI automatically by file signature.
- Preserves Freelancer-specific value behavior, quoting rules, numeric values, and corrupted-file checks inspired by [skeeto/binitools](https://github.com/skeeto/binitools).
- Ships as a reusable library plus `Lizerium.BINI.Converter.App` for real file work.
- Includes a local web overlay: drag an `.ini` into a browser page, convert it, download the result, and preview text output.
- Includes a static GitHub Pages portal with a browser JavaScript BINI converter.
- Has a no-framework console tester with binitools fixtures and optional Freelancer folder roundtrip checks.
- Contains an xUnit project with tests. (546+ tests)
  - ![tests](media/tests.png)
- Example web page - https://lizerium.github.io/Lizerium.BINI.Converter/

## Projects

- `app/Lizerium.BINI.Converter` - the reusable `net8.0` library.
- `app/Lizerium.BINI.Converter.App` - CLI and local web overlay.
- `app/Lizerium.BINI.Converter.Tester` - console verification runner.
- `app/Lizerium.BINI.Converter.Visual.Tester` - xUnit test project for Test Explorer and CI.

## Documentation

All usage instructions live in `docs`:

- [NuGet package usage](docs/info/nuget.md)
- [App, CLI, and web overlay](docs/info/app.md)
- [Build and verification](docs/info/build.md)
- [Roadmap](docs/info/TODO.md)
- [Static portal](docs/README.md)

## Quick Taste

```csharp
using Lizerium.BINI.Converter;

byte[] bini = File.ReadAllBytes("market_commodities.ini");
string text = BiniConverter.ConvertBiniToText(bini);

byte[] packed = BiniConverter.ConvertTextToBini(text);
```

## Credits

Christopher Wellons - [skeeto/binitools](https://github.com/skeeto/binitools)
