<h1 align="center">Build And Verification</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>

  <a href="./build.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  |
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

## Build Library

```powershell
dotnet build app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj
```

## Build App

```powershell
dotnet build app/Lizerium.BINI.Converter.App/Lizerium.BINI.Converter.App.csproj
```

## Run Tester

```powershell
dotnet run --project app/Lizerium.BINI.Converter.Tester/Lizerium.BINI.Converter.Tester.csproj
```

The tester checks library behavior against bundled valid and invalid binitools fixtures.

If Freelancer is installed in:

```text
C:\Program Files (x86)\Freelancer
```

the tester also copies `.ini` files into:

```text
app/Lizerium.BINI.Converter.Tester/TestWork/Freelancer
```

and runs roundtrip checks on those copies.

## Run xUnit Tests

```powershell
dotnet test app/Lizerium.BINI.Converter.Visual.Tester/Lizerium.BINI.Converter.Visual.Tester.csproj
```

`Lizerium.BINI.Converter.Visual.Tester` is the xUnit project intended for Visual Studio Test Explorer, `dotnet test`, and CI. It links the existing binitools fixtures from `Lizerium.BINI.Converter.Tester/TestData`, so fixture files stay in one place.

## Pack NuGet

```powershell
dotnet pack app/Lizerium.BINI.Converter/Lizerium.BINI.Converter.csproj -c Release
```

The package is created under:

```text
app/Lizerium.BINI.Converter/bin/Release
```
