# Использование NuGet-Пакета

`Lizerium.BINI.Converter` - переиспользуемый библиотечный пакет. Используй его, если нужна BINI-конвертация внутри другого .NET приложения, инструмента, лаунчера, редактора или pipeline.

## Установка

Из NuGet:

```powershell
dotnet add package Lizerium.BINI.Converter
```

Из локального пакета, собранного из этого репозитория:

```powershell
dotnet pack app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj -c Release
dotnet add package Lizerium.BINI.Converter --source app/Lizerium.BINI.Converter/bin/Release
```

## Конвертация BINI В Text INI

```csharp
using Lizerium.BINI.Converter;

byte[] bini = File.ReadAllBytes("market_commodities.ini");
string textIni = BiniConverter.ConvertBiniToText(bini);

File.WriteAllText("market_commodities.text.ini", textIni);
```

## Конвертация Text INI В BINI

```csharp
using Lizerium.BINI.Converter;

string textIni = File.ReadAllText("market_commodities.text.ini");
byte[] bini = BiniConverter.ConvertTextToBini(textIni);

File.WriteAllBytes("market_commodities.ini", bini);
```

## Работа С Byte Arrays

```csharp
using System.Text;
using Lizerium.BINI.Converter;

byte[] textBytes = File.ReadAllBytes("shiparch.text.ini");
byte[] biniBytes = BiniConverter.ConvertTextBytesToBini(textBytes, Encoding.Latin1);

byte[] restoredTextBytes = BiniConverter.ConvertBiniToTextBytes(biniBytes, Encoding.Latin1);
```

Если encoding не передан, byte/file API используют `BiniEncodings.Windows1251`, потому что многие Freelancer-файлы и локализованные моды хранят строки как Windows-1251. Передавай `BiniEncodings.Latin1`, когда нужно byte-preserving поведение для искусственных fixtures или legacy-данных, которые не являются настоящим CP1251-текстом.

## Работа С Файлами

```csharp
using Lizerium.BINI.Converter;

BiniConverter.ConvertBiniFileToTextFile(
    inputPath: "market_commodities.ini",
    outputPath: "market_commodities.text.ini");

BiniConverter.ConvertTextFileToBiniFile(
    inputPath: "market_commodities.text.ini",
    outputPath: "market_commodities.ini");
```

## Определение Формата

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

По умолчанию text parser разрешает Freelancer-style extensions. Используй strict mode, если нужна более строгая INI-валидация:

```csharp
using Lizerium.BINI.Converter;

byte[] bini = BiniConverter.ConvertTextToBiniStrict(File.ReadAllText("input.ini"));
```

## Public API

- `IsBini(ReadOnlySpan<byte> data)` - проверяет BINI signature.
- `ConvertTextToBini(string text)` - конвертирует text INI в BINI bytes.
- `ConvertTextToBini(string text, Encoding? encoding)` - конвертирует text INI в BINI bytes с явной encoding для string table.
- `ConvertTextToBiniStrict(string text)` - конвертирует text INI с более строгими parser rules.
- `ConvertTextToBiniStrict(string text, Encoding? encoding)` - strict conversion с явной encoding для string table.
- `ConvertTextBytesToBini(byte[] textBytes, Encoding? encoding = null)` - конвертирует encoded text bytes в BINI bytes.
- `ConvertBiniToText(byte[] biniBytes)` - конвертирует BINI bytes в text INI.
- `ConvertBiniToText(byte[] biniBytes, Encoding? encoding)` - конвертирует BINI bytes в text INI с явной encoding для string table.
- `ConvertBiniToTextBytes(byte[] biniBytes, Encoding? encoding = null)` - конвертирует BINI bytes в encoded text bytes.
- `ConvertTextFileToBiniFile(string inputPath, string outputPath, Encoding? encoding = null)` - конвертирует text INI file в BINI file.
- `ConvertBiniFileToTextFile(string inputPath, string outputPath, Encoding? encoding = null)` - конвертирует BINI file в text INI file.

## Ошибки

Некорректный text INI или поврежденный BINI input выбрасывает `FormatException`.

Text file со слишком большим количеством unique strings для BINI string table может выбросить `InvalidOperationException`.

## Нужен Готовый Инструмент?

Используй `Lizerium.BINI.Converter.App` для file conversion, recursive folder conversion и локального browser overlay.

Смотри [App, CLI и web overlay](app.ru.md).

## Нужна Browser-Only Версия?

В репозитории также есть статический JavaScript portal в `docs`. Он exposes `window.LizeriumBini` и может работать на GitHub Pages без backend.
