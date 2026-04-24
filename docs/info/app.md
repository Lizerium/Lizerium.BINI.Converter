<h1 align="center">App, CLI, And Web Overlay</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>

  <a href="./app.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  |
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

`Lizerium.BINI.Converter.App` is the ready-to-run tool built on top of the library. It has two modes:

- CLI mode for files and folders.
- Local web overlay mode for drag-and-drop conversion in a browser.

## Run CLI Mode

Convert one file with automatic direction detection:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "file.ini"
```

Convert one file in place:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "file.ini" --overwrite
```

Convert a whole folder recursively:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "C:\Games\Freelancer\DATA" --output ".\converted"
```

## CLI Options

```text
Lizerium.BINI.Converter.App <file-or-folder> [options]
Lizerium.BINI.Converter.App --web [--urls <url>]
```

- `--mode auto|to-text|to-bini` - conversion direction. Default: `auto`.
- `--encoding cp1251|latin1|utf8` - text/string encoding. Default: `cp1251`.
- `--output <path>` or `-o <path>` - output file path or output folder for directory mode.
- `--overwrite` - replace source files in place.
- `--stop-on-error` - stop directory conversion after the first failed file.
- `--web` - start the local browser drag-and-drop converter.
- `--urls <url>` - web bind URL. Default: `http://localhost:5087`.
- `-h`, `--help` - show help.

## Output Rules

In file mode, when `--overwrite` and `--output` are not passed:

- BINI input becomes `*.text.ini`.
- Text INI input becomes `*.bini.ini`.

In folder mode, when `--overwrite` and `--output` are not passed:

- The app creates a sibling folder named `<input-folder>.converted`.
- Directory structure is preserved.
- Only `*.ini` files are processed.

## Run Web Overlay

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- --web
```

Open:

```text
http://localhost:5087
```

Use a custom URL:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- --web --urls http://localhost:5100
```

## Web Overlay Features

- Drag and drop an `.ini` file onto the page.
- Choose `Auto`, `BINI -> INI text`, or `INI text -> BINI`.
- Download the converted file.
- Preview the result directly when output is text INI.
- Switch interface language between English and Russian.
- The initial language follows the browser language.
- The selected language is saved in `localStorage`.
- Text encoding can be switched between Windows-1251, Latin-1, and UTF-8.

## Static GitHub Pages Portal

There is also a fully static portal under:

```text
docs
```

It includes a browser JavaScript port of the BINI converter and does not require the ASP.NET app or any backend. Publish `docs` as the GitHub Pages root.

## Web Frontend Files

The web UI is served from:

```text
app/Lizerium.BINI.Converter.App/wwwroot
```

Current structure:

- `index.html` - page markup.
- `css/app.css` - styles.
- `js/app.js` - browser logic, drag-and-drop, API calls, localization.
- `locales/en.json` - English strings.
- `locales/ru.json` - Russian strings.

The project file copies `wwwroot` into the build output, so the web overlay also works from `dotnet run` and compiled output.
