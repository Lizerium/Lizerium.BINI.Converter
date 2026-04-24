## Language

- 🇺🇸 English (current)
- 🇷🇺 Russian: Russian version available on GitHub - https://github.com/Lizerium/Lizerium.BINI.Converter/blob/main/js/npm-package/README.ru.md

# lizerium-bini-converter

Freelancer BINI converter for JavaScript, browsers, and Node.js.

The package converts binary Freelancer BINI `.ini` files into editable INI text and packs text INI back into BINI bytes. It is based on the JavaScript converter used by the Lizerium BINI GitHub Pages portal.

## Install

```bash
npm install lizerium-bini-converter
```

## Node.js Usage

```js
import fs from 'fs'
import { isBini, convertBiniToText } from 'lizerium-bini-converter'

const bytes = fs.readFileSync('market_commodities.ini')

if (isBini(bytes)) {
	console.log(convertBiniToText(bytes))
}
```

## Convert Text To BINI

```js
import fs from 'fs'
import { convertTextToBini } from 'lizerium-bini-converter'

const text = fs.readFileSync('market_commodities.text.ini', 'utf8')
const bini = convertTextToBini(text, { encoding: 'cp1251' })

fs.writeFileSync('market_commodities.ini', bini)
```

## Browser Usage

Use the UMD bundle from `dist/index.umd.js`:

```html
<script src="./dist/index.umd.js"></script>
<script>
	const text = LizeriumBini.convertBiniToText(bytes)
</script>
```

For a complete browser sample, see `examples/browser-example.html`.

## API

```js
isBini(data)
convertBiniToText(data, options)
convertTextToBini(text, options)
```

`data` can be an `ArrayBuffer`, `Uint8Array`, or Node.js `Buffer`.

`convertTextToBini` accepts a string.

## Supported Encodings

- `cp1251`
- `latin1`
- `utf8`

Aliases are also accepted:

- `windows-1251`, `win1251`, `1251`
- `latin-1`, `iso-8859-1`
- `utf-8`

## Build

```bash
npm install
npm run build
```

Build output:

- `dist/index.js` - ESM
- `dist/index.cjs` - CommonJS
- `dist/index.umd.js` - browser UMD global `LizeriumBini`

## Test

```bash
npm test
```
