<h1 align="center">App, CLI И Web Overlay</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>

  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇷🇺 Russian (current)
  </span>
  |
  <a href="./app.md" style="color: #F5F752; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

`Lizerium.BINI.Converter.App` - готовый инструмент поверх библиотеки. У него два режима:

- CLI mode для файлов и папок.
- Локальный web overlay для drag-and-drop конвертации в браузере.

## Запуск CLI

Сконвертировать один файл с автоматическим определением направления:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "file.ini"
```

Сконвертировать один файл на месте:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "file.ini" --overwrite
```

Рекурсивно сконвертировать всю папку:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- "C:\Games\Freelancer\DATA" --output ".\converted"
```

## CLI Options

```text
Lizerium.BINI.Converter.App <file-or-folder> [options]
Lizerium.BINI.Converter.App --web [--urls <url>]
```

- `--mode auto|to-text|to-bini` - направление конвертации. По умолчанию: `auto`.
- `--encoding cp1251|latin1|utf8` - encoding для текста/string table. По умолчанию: `cp1251`.
- `--output <path>` или `-o <path>` - путь к output-файлу или output-папке в directory mode.
- `--overwrite` - заменить исходные файлы на месте.
- `--stop-on-error` - остановить обработку папки после первого failed-файла.
- `--web` - запустить локальный drag-and-drop конвертер в браузере.
- `--urls <url>` - URL для web-сервера. По умолчанию: `http://localhost:5087`.
- `-h`, `--help` - показать help.

## Правила Output

В file mode, если не переданы `--overwrite` и `--output`:

- BINI input становится `*.text.ini`.
- Text INI input становится `*.bini.ini`.

В folder mode, если не переданы `--overwrite` и `--output`:

- Приложение создает соседнюю папку `<input-folder>.converted`.
- Структура директорий сохраняется.
- Обрабатываются только `*.ini` файлы.

## Запуск Web Overlay

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- --web
```

Открой:

```text
http://localhost:5087
```

Использовать другой URL:

```powershell
dotnet run --project app/Lizerium.BINI.Converter.App -- --web --urls http://localhost:5100
```

## Возможности Web Overlay

- Drag-and-drop `.ini` файла на страницу.
- Выбор `Auto`, `BINI -> INI text` или `INI text -> BINI`.
- Скачивание сконвертированного файла.
- Предпросмотр результата, если output - текстовый INI.
- Переключение интерфейса между English и Russian.
- При первом запуске язык берется из browser language.
- Выбранный язык сохраняется в `localStorage`.
- Encoding текста можно переключать между Windows-1251, Latin-1 и UTF-8.

## Static GitHub Pages Portal

Также есть полностью статический portal:

```text
docs/portal
```

Внутри лежит browser JavaScript port BINI converter. Для него не нужен ASP.NET app или backend. При публикации GitHub Pages укажи `docs` как Pages root и открой `/portal/`.

## Файлы Web Frontend

Web UI отдается из:

```text
app/Lizerium.BINI.Converter.App/wwwroot
```

Текущая структура:

- `index.html` - разметка страницы.
- `css/app.css` - стили.
- `js/app.js` - browser logic, drag-and-drop, API calls, localization.
- `locales/en.json` - английские строки.
- `locales/ru.json` - русские строки.

Project file копирует `wwwroot` в build output, поэтому web overlay работает и через `dotnet run`, и из compiled output.
