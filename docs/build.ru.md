<h1 align="center">Сборка И Проверка</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>

  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇷🇺 Russian (current)
  </span>
  |
  <a href="./build.md" style="color: #F5F752; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

## Сборка Библиотеки

```powershell
dotnet build app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj
```

## Сборка App

```powershell
dotnet build app/Lizerium.BINI.Converter.App/Lizerium.BINI.Converter.App.csproj
```

## Запуск Tester

```powershell
dotnet run --project app/Lizerium.BINI.Converter.Tester/Lizerium.BINI.Converter.Tester.csproj
```

Tester проверяет поведение библиотеки на bundled valid и invalid binitools fixtures.

Если Freelancer установлен в:

```text
C:\Program Files (x86)\Freelancer
```

tester также копирует `.ini` файлы в:

```text
app/Lizerium.BINI.Converter.Tester/TestWork/Freelancer
```

и запускает roundtrip checks на этих копиях.

## Запуск xUnit Tests

```powershell
dotnet test app/Lizerium.BINI.Converter.Visual.Tester/Lizerium.BINI.Converter.Visual.Tester.csproj
```

`Lizerium.BINI.Converter.Visual.Tester` - xUnit project для Visual Studio Test Explorer, `dotnet test` и CI. Он подключает существующие binitools fixtures из `Lizerium.BINI.Converter.Tester/TestData`, поэтому fixture-файлы остаются в одном месте.

## Pack NuGet

```powershell
dotnet pack app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj -c Release
```

Пакет создается в:

```text
app/Lizerium.BINI.Converter/bin/Release
```
