  const cp1251High = [
    '\u0402', '\u0403', '\u201a', '\u0453', '\u201e', '\u2026', '\u2020', '\u2021',
    '\u20ac', '\u2030', '\u0409', '\u2039', '\u040a', '\u040c', '\u040b', '\u040f',
    '\u0452', '\u2018', '\u2019', '\u201c', '\u201d', '\u2022', '\u2013', '\u2014',
    '\u0000', '\u2122', '\u0459', '\u203a', '\u045a', '\u045c', '\u045b', '\u045f',
    '\u00a0', '\u040e', '\u045e', '\u0408', '\u00a4', '\u0490', '\u00a6', '\u00a7',
    '\u0401', '\u00a9', '\u0404', '\u00ab', '\u00ac', '\u00ad', '\u00ae', '\u0407',
    '\u00b0', '\u00b1', '\u0406', '\u0456', '\u0491', '\u00b5', '\u00b6', '\u00b7',
    '\u0451', '\u2116', '\u0454', '\u00bb', '\u0458', '\u0405', '\u0455', '\u0457',
    '\u0410', '\u0411', '\u0412', '\u0413', '\u0414', '\u0415', '\u0416', '\u0417',
    '\u0418', '\u0419', '\u041a', '\u041b', '\u041c', '\u041d', '\u041e', '\u041f',
    '\u0420', '\u0421', '\u0422', '\u0423', '\u0424', '\u0425', '\u0426', '\u0427',
    '\u0428', '\u0429', '\u042a', '\u042b', '\u042c', '\u042d', '\u042e', '\u042f',
    '\u0430', '\u0431', '\u0432', '\u0433', '\u0434', '\u0435', '\u0436', '\u0437',
    '\u0438', '\u0439', '\u043a', '\u043b', '\u043c', '\u043d', '\u043e', '\u043f',
    '\u0440', '\u0441', '\u0442', '\u0443', '\u0444', '\u0445', '\u0446', '\u0447',
    '\u0448', '\u0449', '\u044a', '\u044b', '\u044c', '\u044d', '\u044e', '\u044f'
  ];

  const cp1251EncodeMap = new Map();
  cp1251High.forEach((char, index) => {
    if (char !== '\u0000') {
      cp1251EncodeMap.set(char, 0x80 + index);
    }
  });

  function normalizeBytes(data, encoding = 'cp1251') {
    if (data instanceof Uint8Array) {
      return data;
    }

    if (typeof data === 'string') {
      return encode(data, encoding);
    }

    if (data instanceof ArrayBuffer) {
      return new Uint8Array(data);
    }

    if (ArrayBuffer.isView(data)) {
      return new Uint8Array(data.buffer, data.byteOffset, data.byteLength);
    }

    throw new TypeError('Expected ArrayBuffer, Uint8Array, Buffer, or string.');
  }

  function decode(bytes, encoding) {
    const normalized = normalizeBytes(bytes, encoding);
    switch (normalizeEncoding(encoding)) {
      case 'cp1251':
        return new TextDecoder('windows-1251').decode(normalized);
      case 'latin1':
        return Array.from(normalized, byte => String.fromCharCode(byte)).join('');
      case 'utf8':
        return new TextDecoder('utf-8').decode(normalized);
      default:
        throw new Error(`Unknown encoding: ${encoding}`);
    }
  }

  function encode(text, encoding) {
    switch (normalizeEncoding(encoding)) {
      case 'cp1251':
        return encodeCp1251(text);
      case 'latin1':
        return Uint8Array.from(Array.from(text, char => char.charCodeAt(0) & 0xff));
      case 'utf8':
        return new TextEncoder().encode(text);
      default:
        throw new Error(`Unknown encoding: ${encoding}`);
    }
  }

  function normalizeEncoding(value) {
    const encoding = (value || 'cp1251').toLowerCase();
    if (encoding === 'windows-1251' || encoding === 'win1251' || encoding === '1251') {
      return 'cp1251';
    }

    if (encoding === 'latin-1' || encoding === 'iso-8859-1') {
      return 'latin1';
    }

    if (encoding === 'utf-8') {
      return 'utf8';
    }

    return encoding;
  }

  function encodeCp1251(text) {
    const bytes = [];
    for (const char of text) {
      const code = char.charCodeAt(0);
      if (code <= 0x7f) {
        bytes.push(code);
      } else if (cp1251EncodeMap.has(char)) {
        bytes.push(cp1251EncodeMap.get(char));
      } else {
        bytes.push(0x3f);
      }
    }

    return Uint8Array.from(bytes);
  }

  function isBini(data) {
    const bytes = normalizeBytes(data);
    return bytes.length >= 5 &&
      bytes[0] === 0x42 &&
      bytes[1] === 0x49 &&
      bytes[2] === 0x4e &&
      bytes[3] === 0x49 &&
      bytes[4] === 1;
  }

  function convertTextToBini(text, options = {}) {
    if (typeof text !== 'string') {
      throw new TypeError('Expected text to be a string.');
    }

    const document = new IniParser(text).parse();
    return writeBini(document, options.encoding || 'cp1251');
  }

  function convertTextBytesToBini(bytes, options = {}) {
    return convertTextToBini(decode(bytes, options.encoding || 'cp1251'), options);
  }

  function convertBiniToText(data, options = {}) {
    return readBini(normalizeBytes(data, options.encoding || 'cp1251'), options.encoding || 'cp1251');
  }

  function convertBiniToTextBytes(data, options = {}) {
    return encode(convertBiniToText(data, options), options.encoding || 'cp1251');
  }

  class IniParser {
    constructor(text) {
      this.text = text;
      this.position = 0;
      this.line = 1;
    }

    parse() {
      const sections = [];
      for (;;) {
        const section = this.parseSection();
        if (!section) {
          return { sections };
        }

        sections.push(section);
      }
    }

    parseSection() {
      if (!this.skipSpace()) {
        return null;
      }

      let c = this.get();
      if (c !== '[') {
        this.error(`unexpected '${this.printable(c)}', expected '['`);
      }

      if (!this.skipWhiteSpaceOnly()) {
        this.error('unexpected end of file');
      }

      const name = this.readNameUntil(']');
      if (!this.skipSpace()) {
        this.error('unexpected end of file');
      }

      c = this.get();
      if (c !== ']') {
        this.error(`unexpected '${this.printable(c)}', expected ']'`);
      }

      const entries = [];
      for (;;) {
        const entry = this.parseEntry();
        if (!entry) {
          return { name, entries };
        }

        if (entries.length === 0xffff) {
          this.error('too many entries in one section');
        }

        entries.push(entry);
      }
    }

    parseEntry() {
      if (!this.skipSpace()) {
        return null;
      }

      const c = this.get();
      if (c === '[') {
        this.unget();
        return null;
      }

      const name = c === '"' ? this.readQuotedBody() : this.readSimpleBody('=', true);
      const values = [];

      if (!this.skipBlank()) {
        return { name, values };
      }

      let next = this.get();
      if (next === '\r' || next === '\n' || next === ';' || next === -1) {
        if (next === '\r') {
          this.consumeOptionalLineFeed();
        }
        return { name, values };
      }

      if (next !== '=') {
        this.consumeLine(next);
        return { name, values };
      }

      if (!this.skipBlank()) {
        return { name, values };
      }

      next = this.get();
      this.unget();
      if (next === '\r') {
        this.consumeOptionalLineFeed();
        return { name, values };
      }

      if (next === '\n' || next === ';') {
        return { name, values };
      }

      for (;;) {
        const parsed = this.parseValue();
        values.push(parsed.value);
        if (values.length > 0xff) {
          this.error('too many values in one entry');
        }

        next = parsed.next;
        if (next === '\r') {
          this.consumeOptionalLineFeed();
          return { name, values };
        }

        if (next === '\n' || next === -1) {
          return { name, values };
        }

        if (next === ';') {
          while ((next = this.get()) !== -1 && next !== '\n') {
          }
          return { name, values };
        }

        if (next !== ',') {
          this.error(`unexpected '${this.printable(next)}', expected ','`);
        }

        if (!this.skipBlank()) {
          this.error('unexpected EOF, expected a value');
        }
      }
    }

    parseValue() {
      let c = this.get();
      if (c === '"') {
        const value = this.readQuotedBody();
        return { value: stringValue(value), next: this.get() };
      }

      if (c === ',' || c === '\r' || c === '\n') {
        if (c === '\r') {
          this.consumeOptionalLineFeed();
          c = '\n';
        }
        return { value: stringValue(''), next: c };
      }

      this.unget();
      const token = this.readSimpleBody(',');
      const next = this.get();

      if (token === '-0') {
        return { value: floatValue(-0), next };
      }

      if (/^[+-]?\d+$/.test(token)) {
        const big = BigInt(token);
        if (big >= BigInt(-2147483648) && big <= BigInt(4294967295)) {
          return { value: integerValue(Number(BigInt.asUintN(32, big))), next };
        }
      }

      if (/^[+-]?(?:\d+\.\d*|\.\d+|\d+[eE][+-]?\d+|\d+\.\d*[eE][+-]?\d+|\.\d+[eE][+-]?\d+)$/.test(token)) {
        const floating = Number(token);
        if (Number.isFinite(floating)) {
          return { value: floatValue(Math.fround(floating)), next };
        }
      }

      return { value: stringValue(token), next };
    }

    readNameUntil(terminator) {
      const c = this.get();
      if (c === '"') {
        return this.readQuotedBody();
      }

      this.unget();
      return this.readSimpleBody(terminator);
    }

    readQuotedBody() {
      let value = '';
      for (;;) {
        const c = this.get();
        if (c === -1) {
          this.error('EOF in middle of string');
        }

        if (c === '"') {
          const next = this.get();
          if (next !== '"') {
            if (next !== -1) {
              this.unget();
            }
            return value;
          }
        }

        value += c;
      }
    }

    readSimpleBody(terminator, alreadyRead = false) {
      const start = alreadyRead ? this.position - 1 : this.position;
      for (;;) {
        const c = this.get();
        if (c === -1) {
          break;
        }

        if (c === terminator || c === '\n' || c === '\r' || c === ';') {
          this.unget();
          break;
        }
      }

      return trimBiniWhiteSpace(this.text.slice(start, this.position));
    }

    skipSpace() {
      for (;;) {
        let c = this.get();
        while (c !== -1 && isBiniWhiteSpace(c)) {
          c = this.get();
        }

        if (c === -1) {
          return false;
        }

        if (c === ';' || c === '@') {
          do {
            c = this.get();
          } while (c !== -1 && c !== '\n');

          if (c === -1) {
            return false;
          }
        } else {
          this.unget();
          return true;
        }
      }
    }

    skipBlank() {
      let c = this.get();
      while (c !== -1 && isPlainWhiteSpace(c)) {
        c = this.get();
      }

      if (c !== -1) {
        this.unget();
      }

      return c !== -1;
    }

    skipWhiteSpaceOnly() {
      let c = this.get();
      while (c !== -1 && isBiniWhiteSpace(c)) {
        c = this.get();
      }

      if (c !== -1) {
        this.unget();
      }

      return c !== -1;
    }

    get() {
      if (this.position >= this.text.length) {
        return -1;
      }

      const c = this.text[this.position++];
      if (c === '\0') {
        this.error('invalid NUL byte');
      }

      if (c === '\n') {
        this.line++;
      }

      return c;
    }

    unget() {
      this.position--;
      if (this.position >= 0 && this.text[this.position] === '\n') {
        this.line--;
      }
    }

    consumeOptionalLineFeed() {
      const c = this.get();
      if (c !== '\n' && c !== -1) {
        this.unget();
      }
    }

    consumeLine(current) {
      let c = current;
      while (c !== -1 && c !== '\n') {
        c = this.get();
      }
    }

    error(message) {
      throw new Error(`stdin:${this.line}: ${message}`);
    }

    printable(c) {
      return c === -1 ? 'EOF' : c;
    }
  }

  function integerValue(value) {
    return { type: 1, integer: value >>> 0 };
  }

  function floatValue(value) {
    return { type: 2, floating: Math.fround(value) };
  }

  function stringValue(value) {
    return { type: 3, text: value };
  }

  function trimBiniWhiteSpace(value) {
    let start = 0;
    let end = value.length;
    while (start < end && isBiniWhiteSpace(value[start])) start++;
    while (end > start && isBiniWhiteSpace(value[end - 1])) end--;
    return value.slice(start, end);
  }

  function isBiniWhiteSpace(c) {
    return c === ' ' || c === '\f' || c === '\n' || c === '\r' || c === '\t' || c === '\v';
  }

  function isPlainWhiteSpace(c) {
    return c === ' ' || c === '\f' || c === '\r' || c === '\t' || c === '\v';
  }

  function writeBini(document, encoding) {
    const strings = new StringTable(encoding);
    let structsLength = 12;

    for (const section of document.sections) {
      strings.add(section.name);
      structsLength += 4;

      for (const entry of section.entries) {
        strings.add(entry.name);
        structsLength += 3 + entry.values.length * 5;

        for (const value of entry.values) {
          if (value.type === 3) {
            strings.add(value.text);
          }
        }
      }
    }

    strings.finalize();
    const output = [];
    writeU32(output, 0x494e4942);
    writeU32(output, 1);
    writeU32(output, structsLength >>> 0);

    for (const section of document.sections) {
      writeU16(output, strings.offsetOf(section.name));
      writeU16(output, section.entries.length);

      for (const entry of section.entries) {
        writeU16(output, strings.offsetOf(entry.name));
        output.push(entry.values.length & 0xff);

        for (const value of entry.values) {
          output.push(value.type);
          if (value.type === 1) {
            writeU32(output, value.integer >>> 0);
          } else if (value.type === 2) {
            writeU32(output, floatToBits(value.floating));
          } else {
            writeU32(output, strings.offsetOf(value.text));
          }
        }
      }
    }

    strings.writeTo(output);
    return Uint8Array.from(output);
  }

  class StringTable {
    constructor(encoding) {
      this.encoding = encoding;
      this.strings = [];
      this.offsets = new Map();
    }

    add(value) {
      if (!this.offsets.has(value)) {
        this.offsets.set(value, 0);
        this.strings.push(value);
      }
    }

    finalize() {
      let offset = 0;
      for (const value of this.strings) {
        if (offset > 0xffff) {
          throw new Error('too many strings');
        }

        this.offsets.set(value, offset);
        offset += encode(value, this.encoding).length + 1;
      }
    }

    offsetOf(value) {
      return this.offsets.get(value);
    }

    writeTo(output) {
      for (const value of this.strings) {
        output.push(...encode(value, this.encoding), 0);
      }
    }
  }

  function readBini(data, encoding) {
    if (data.length < 12) {
      throw new Error(`input is too short: ${data.length} bytes`);
    }

    const magic = readU32(data, 0);
    const version = readU32(data, 4);
    const textOffset = readU32(data, 8);

    if (magic !== 0x494e4942) {
      throw new Error(`unknown input format (bad magic): 0x${magic.toString(16)}`);
    }

    if (version !== 1) {
      throw new Error(`unknown input format (bad version): ${version}`);
    }

    if (textOffset > data.length) {
      throw new Error(`unknown input format (bad text offset): ${textOffset}`);
    }

    if (textOffset < data.length && data[data.length - 1] !== 0) {
      throw new Error('invalid input (unterminated text segment)');
    }

    let output = '';
    let position = 12;
    const textStart = textOffset;
    const textLength = data.length - textStart;
    let firstSection = true;

    while (position < textStart - 3) {
      const sectionNameOffset = readU16(data, position);
      const entryCount = readU16(data, position + 2);
      if (sectionNameOffset >= textLength) {
        throw new Error('invalid section text offset, aborting');
      }

      if (!firstSection) {
        output += '\n';
      }

      firstSection = false;
      output += `[${formatSpecial(readText(data, textStart, textLength, sectionNameOffset, encoding), '"[] \f\n\r\t\v')}]\n`;
      position += 4;

      for (let i = 0; i < entryCount; i++) {
        if (position > textStart - 3) {
          throw new Error('truncated entry, aborting');
        }

        const nameOffset = readU16(data, position);
        const valueCount = data[position + 2];
        position += 3;
        if (nameOffset >= textLength) {
          throw new Error('invalid entry text offset, aborting');
        }

        output += `${formatSpecial(readText(data, textStart, textLength, nameOffset, encoding), '"=[] \f\n\r\t\v')} =`;

        for (let j = 0; j < valueCount; j++) {
          const type = data[position + j * 5];
          const raw = readU32(data, position + j * 5 + 1);
          output += j === 0 ? ' ' : ', ';

          if (type === 1) {
            output += raw.toString(10);
          } else if (type === 2) {
            output += formatFloat(bitsToFloat(raw));
          } else if (type === 3) {
            if (raw >= textLength) {
              throw new Error('invalid value text offset, aborting');
            }
            const value = readText(data, textStart, textLength, raw, encoding);
            output += formatSpecial(value, looksNumeric(value) ? null : '", \f\n\r\t\v');
          } else {
            throw new Error(`bad value type, ${type}`);
          }
        }

        output += '\n';
        position += valueCount * 5;
      }
    }

    return output;
  }

  function readText(data, textStart, textLength, offset, encoding) {
    const absolute = textStart + offset;
    let end = absolute;
    const limit = textStart + textLength;
    while (end < limit && data[end] !== 0) {
      end++;
    }

    return decode(data.slice(absolute, end), encoding);
  }

  function formatSpecial(value, special) {
    const simple = value.length > 0 && special !== null && Array.from(value).every(char => !special.includes(char));
    if (simple) {
      return value;
    }

    return `"${value.replaceAll('"', '""')}"`;
  }

  function looksNumeric(value) {
    return /^[+-]?\d+$/.test(value) || !Number.isNaN(Number(value));
  }

  function formatFloat(value) {
    if (Object.is(value, -0)) {
      return '-0.0';
    }

    let text = Number(value).toPrecision(9).replace(/\.?0+(?=e|$)/i, '');
    const parsed = Math.fround(Number(text));
    if (!Object.is(parsed, Math.fround(value))) {
      text = Number(value).toString();
    }

    return /[.e]/i.test(text) || !Number.isFinite(value) ? text : `${text}.0`;
  }

  function readU16(data, offset) {
    return data[offset] | (data[offset + 1] << 8);
  }

  function readU32(data, offset) {
    return (data[offset] |
      (data[offset + 1] << 8) |
      (data[offset + 2] << 16) |
      (data[offset + 3] << 24)) >>> 0;
  }

  function writeU16(output, value) {
    output.push(value & 0xff, (value >>> 8) & 0xff);
  }

  function writeU32(output, value) {
    output.push(value & 0xff, (value >>> 8) & 0xff, (value >>> 16) & 0xff, (value >>> 24) & 0xff);
  }

  function floatToBits(value) {
    const buffer = new ArrayBuffer(4);
    const view = new DataView(buffer);
    view.setFloat32(0, value, true);
    return view.getUint32(0, true);
  }

  function bitsToFloat(value) {
    const buffer = new ArrayBuffer(4);
    const view = new DataView(buffer);
    view.setUint32(0, value, true);
    return view.getFloat32(0, true);
  }

  export {
    isBini,
    convertTextToBini,
    convertTextBytesToBini,
    convertBiniToText,
    convertBiniToTextBytes,
    encode,
    decode
  };
