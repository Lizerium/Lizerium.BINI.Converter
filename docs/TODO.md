# TODO / Roadmap

This file collects practical next steps for `Lizerium.BINI.Converter`. The goal is not to add noise, but to grow the project into a dependable toolkit for Freelancer modding.

## High Priority

- Add GitHub Actions:
  - build `Lizerium.BINI.Converter`;
  - build `Lizerium.BINI.Converter.App`;
  - run `Lizerium.BINI.Converter.Visual.Tester`;
  - run the console tester;
  - pack NuGet artifacts.
- Improve NuGet package polish:
  - XML documentation;
  - SourceLink;
  - symbols package `.snupkg`;
  - deterministic builds;
  - package release checklist.
- Add richer format errors:
  - custom `BiniFormatException`;
  - byte offset for BINI errors;
  - line/section/value context for text INI errors;
  - user-friendly messages for CLI/web/portal.
- Add CLI diagnostics:
  - `scan <folder>`;
  - count BINI/text INI files;
  - list failed files;
  - report largest files and unusual values.

## App And CLI

- Publish a .NET global tool:
  - `dotnet tool install --global Lizerium.BINI.Converter.Tool`;
  - `bini convert file.ini`;
  - `bini convert DATA --output converted`;
  - `bini web`;
  - `bini scan DATA`.
- Add batch conversion improvements:
  - clear progress output;
  - `--include` / `--exclude` masks;
  - dry-run mode;
  - preserve timestamps option;
  - report file-level stats.
- Add explicit conversion profiles:
  - `freelancer-vanilla`;
  - `freelancer-cp1251`;
  - `byte-preserving`;
  - `strict`.

## Web Overlay

- Support multiple files in one browser drop.
- Support ZIP output for batch conversion.
- Add a file table with statuses:
  - converted;
  - skipped;
  - failed;
  - output size;
  - conversion direction.
- Add side-by-side preview:
  - original text;
  - converted text;
  - binary output notice.
- Add roundtrip check:
  - `INI -> BINI -> INI`;
  - `BINI -> INI -> BINI -> INI`;
  - show semantic differences.

## GitHub Pages Portal

- Keep a static portal under `docs/portal`.
- Continue the JavaScript BINI implementation until it reaches feature parity with the .NET library.
- Add browser-only regression fixtures for:
  - CP1251 Cyrillic;
  - unsigned integer IDs;
  - negative zero;
  - quoted numeric-looking strings;
  - invalid BINI headers.
- Add a small in-page test runner for the portal.
- Add visual examples and a compact API reference for the JavaScript library.

## JavaScript Library

- Split the current browser library into smaller modules when it grows:
  - binary reader/writer;
  - INI parser;
  - string table;
  - encodings;
  - public API facade.
- Add TypeScript definitions:
  - `convertTextToBini(text, options)`;
  - `convertBiniToText(bytes, options)`;
  - `isBini(bytes)`.
- Add optional npm packaging later:
  - browser ESM build;
  - Node.js build;
  - no dependency core.

## Inspection Tools

- Add INI/BINI inspector:
  - sections count;
  - entries count;
  - values count;
  - string table size;
  - value type distribution;
  - suspicious values.
- Add CRC/hash helper views for Freelancer IDs.
- Add exportable JSON report for scans.

## Documentation

- Add examples for each common workflow:
  - library usage;
  - CLI usage;
  - web overlay;
  - GitHub Pages portal;
  - JavaScript API.
- Add troubleshooting:
  - CP1251 vs Latin-1;
  - unsigned 32-bit IDs;
  - comments are discarded by BINI;
  - formatting is canonicalized after BINI roundtrip.
- Add changelog and release notes template.

