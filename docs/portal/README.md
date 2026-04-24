<h1 align="center">Lizerium BINI Portal</h1>

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

This folder is a static GitHub Pages portal. It contains a browser-only JavaScript implementation of the core BINI converter, so it can run without ASP.NET, .NET, Node.js, or a backend server.

## Files

- `index.html` - portal page.
- `css/app.css` - portal styles.
- `js/bini-converter.js` - standalone JavaScript BINI library.
- `js/app.js` - page behavior.

## Local Preview

Open `index.html` directly in a browser, or serve the folder with any static server.

For GitHub Pages, publish the repository with `docs` as the Pages root. The portal will be available under:

```text
/portal/
```

## JavaScript API

The library is exposed as `window.LizeriumBini`.

```js
const bytes = await file.arrayBuffer()

if (LizeriumBini.isBini(bytes)) {
	const text = LizeriumBini.convertBiniToText(bytes, { encoding: 'cp1251' })
}

const bini = LizeriumBini.convertTextToBini(text, { encoding: 'cp1251' })
```

Supported encodings:

- `cp1251`
- `latin1`
- `utf8`

## Current Notes

The JavaScript port is designed to mirror the .NET converter behavior for practical browser usage:

- BINI signature detection;
- BINI to text INI;
- text INI to BINI;
- CP1251, Latin-1, and UTF-8 text handling;
- unsigned 32-bit integer IDs;
- canonical text output.

The .NET implementation remains the reference implementation.
