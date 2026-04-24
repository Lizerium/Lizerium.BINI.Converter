## Язык

- 🇺🇸 English: [README.md](README.md)
- 🇷🇺 Russian (current)

# lizerium-bini-converter

Конвертер Freelancer BINI для JavaScript, браузеров и Node.js.

Пакет преобразует бинарные BINI `.ini` файлы :contentReference[oaicite:0]{index=0} в редактируемый текстовый INI и упаковывает текстовый INI обратно в BINI байты. Основан на JavaScript-конвертере, используемом в портале Lizerium BINI GitHub Pages.

## Установка

```bash
npm install lizerium-bini-converter
```

## Использование в Node.js

```js
import fs from 'fs'
import { isBini, convertBiniToText } from 'lizerium-bini-converter'

const bytes = fs.readFileSync('market_commodities.ini')

if (isBini(bytes)) {
	console.log(convertBiniToText(bytes))
}
```

## Преобразование текста в BINI

```js
import fs from 'fs'
import { convertTextToBini } from 'lizerium-bini-converter'

const text = fs.readFileSync('market_commodities.text.ini', 'utf8')
const bini = convertTextToBini(text, { encoding: 'cp1251' })

fs.writeFileSync('market_commodities.ini', bini)
```

## Использование в браузере

Используйте UMD-сборку из `dist/index.umd.js`:

```html
<script src="./dist/index.umd.js"></script>
<script>
	const text = LizeriumBini.convertBiniToText(bytes)
</script>
```

Полный пример для браузера находится в:

```text
examples/browser-example.html
```

## API

```js
isBini(data)
convertBiniToText(data, options)
convertTextToBini(text, options)
```

`data` может быть:

- `ArrayBuffer`
- `Uint8Array`
- Node.js `Buffer`

`convertTextToBini` принимает строку.

## Поддерживаемые кодировки

- `cp1251`
- `latin1`
- `utf8`

Также поддерживаются псевдонимы:

- `windows-1251`, `win1251`, `1251`
- `latin-1`, `iso-8859-1`
- `utf-8`

## Сборка

```bash
npm install
npm run build
```

Результат сборки:

- `dist/index.js` — ESM
- `dist/index.cjs` — CommonJS
- `dist/index.umd.js` — browser UMD глобальный объект `LizeriumBini`

## Тесты

```bash
npm test
```
