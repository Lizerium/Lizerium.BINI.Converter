<h1 align="center">Lizerium BINI Portal</h1>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>

  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇷🇺 Russian (current)
  </span>
  |
  <a href="./README.md" style="color: #F5F752; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

Эта папка содержит статический GitHub Pages портал. Внутри находится полностью браузерная JavaScript-реализация основного BINI-конвертера, поэтому всё работает без ASP.NET, .NET, Node.js и без серверной части.

## Файлы

- `index.html` — страница портала.
- `css/app.css` — стили портала.
- `js/bini-converter.js` — автономная JavaScript-библиотека BINI.
- `js/app.js` — логика интерфейса страницы.

## Локальный запуск

Откройте `index.html` напрямую в браузере либо запустите папку через любой статический web-сервер.

Для GitHub Pages опубликуйте репозиторий с корнем Pages на папке `docs`. После этого портал будет доступен по адресу:

```text
/portal/
```

## JavaScript API

Библиотека доступна через `window.LizeriumBini`.

```js
const bytes = await file.arrayBuffer()

if (LizeriumBini.isBini(bytes)) {
	const text = LizeriumBini.convertBiniToText(bytes, { encoding: 'cp1251' })
}

const bini = LizeriumBini.convertTextToBini(text, { encoding: 'cp1251' })
```

## Поддерживаемые кодировки

- `cp1251`
- `latin1`
- `utf8`

## Текущее состояние

JavaScript-порт разработан так, чтобы практически повторять поведение .NET-конвертера и быть удобным для использования прямо в браузере:

- определение сигнатуры BINI;
- преобразование BINI → текстовый INI;
- преобразование текстового INI → BINI;
- работа с кодировками CP1251, Latin-1 и UTF-8;
- беззнаковые 32-битные идентификаторы;
- канонический текстовый вывод.

.NET-реализация остаётся эталонной reference-версией.
