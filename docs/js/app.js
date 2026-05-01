(function () {
  'use strict';

  const strings = {
    en: {
      'meta.title': 'BINI to INI Converter for Freelancer | Lizerium',
      'meta.description': 'Convert Freelancer BINI files to editable INI text and pack INI back to BINI in your browser. No upload, no backend, Windows-1251 support.',
      'nav.subtitle': 'Freelancer modding tool',
      'nav.converter': 'Converter',
      'nav.guide': 'Guide',
      'hero.eyebrow': 'Free browser tool',
      'hero.title': 'Convert Freelancer BINI files to editable INI text',
      'hero.lead': 'Open binary Freelancer configuration files, decode them to readable INI, edit the text, and pack it back to BINI without uploading files anywhere.',
      'hero.cta': 'Convert a file',
      'hero.pages': 'Guides',
      'hero.card1.k': 'Formats',
      'hero.card2.k': 'Privacy',
      'hero.card2.v': '100% local',
      'hero.card3.k': 'Encoding',
      'drop.title': 'Drop a Freelancer .ini file here',
      'drop.subtitle': 'or click to choose BINI or text INI',
      'controls.mode': 'Mode',
      'controls.encoding': 'Encoding',
      'mode.auto': 'Auto detect',
      'mode.toText': 'BINI to INI text',
      'mode.toBini': 'INI text to BINI',
      'actions.convert': 'Convert',
      'actions.download': 'Download',
      'status.noFile': 'No file selected.',
      'status.selected': '{fileName} selected.',
      'status.converting': 'Converting locally...',
      'status.done': '{direction}: done.',
      'preview.title': 'Preview',
      'preview.empty': 'Text output will appear here.',
      'preview.binary': 'BINI output is binary. Use Download to save it.',
      'guide.eyebrow': 'Freelancer BINI guide',
      'guide.title': 'How to open and convert BINI files',
      'guide.open.title': 'How to open BINI files in Freelancer',
      'guide.open.text': 'BINI files are binary configuration files used by Freelancer. Convert them to INI text before editing ship, market, equipment, or universe data.',
      'guide.convert.title': 'BINI to INI converter',
      'guide.convert.text': 'Drag a file into the converter, keep Auto mode, and download readable text output. The conversion runs inside your browser.',
      'guide.reverse.title': 'INI to BINI packing',
      'guide.reverse.text': 'After editing INI text, choose INI text to BINI and save a binary file that can be used by existing Freelancer tooling.',
      'errors.eyebrow': 'Troubleshooting',
      'errors.title': 'Common errors when opening BINI files',
      'errors.read.title': 'File is not readable',
      'errors.read.text': 'Check that the file is a Freelancer BINI or plain INI file and that it was not truncated during download or extraction.',
      'errors.encoding.title': 'Encoding issues',
      'errors.encoding.text': 'Most Russian Freelancer files need Windows-1251. Try Latin-1 or UTF-8 only when the text looks broken.',
      'errors.corrupt.title': 'Corrupted data',
      'errors.corrupt.text': 'Invalid BINI headers, missing string-table data, or unsupported values can prevent conversion.',
      'pages.eyebrow': 'More guides',
      'pages.title': 'Freelancer BINI topics',
      'pages.biniToIni': 'Convert binary Freelancer BINI files to readable INI.',
      'pages.freelancer.title': 'Freelancer BINI files',
      'pages.freelancer.text': 'What BINI files are and how modders work with them.',
      'footer.local': 'Files are processed locally in your browser.'
    },
    ru: {
      'meta.title': 'Конвертер BINI в INI для Freelancer | Lizerium',
      'meta.description': 'Конвертируйте Freelancer BINI в редактируемый INI и собирайте INI обратно в BINI прямо в браузере. Без загрузки файлов на сервер, с поддержкой Windows-1251.',
      'nav.subtitle': 'Инструмент для моддинга Freelancer',
      'nav.converter': 'Конвертер',
      'nav.guide': 'Гайд',
      'hero.eyebrow': 'Бесплатный инструмент в браузере',
      'hero.title': 'Конвертер Freelancer BINI в редактируемый INI',
      'hero.lead': 'Открывайте бинарные конфиги Freelancer, переводите их в читаемый INI, редактируйте текст и собирайте обратно в BINI без отправки файлов на сервер.',
      'hero.cta': 'Конвертировать файл',
      'hero.pages': 'Гайды',
      'hero.card1.k': 'Форматы',
      'hero.card2.k': 'Приватность',
      'hero.card2.v': '100% локально',
      'hero.card3.k': 'Кодировка',
      'drop.title': 'Перетащите .ini файл Freelancer сюда',
      'drop.subtitle': 'или нажмите, чтобы выбрать BINI или текстовый INI',
      'controls.mode': 'Режим',
      'controls.encoding': 'Кодировка',
      'mode.auto': 'Автоопределение',
      'mode.toText': 'BINI в текстовый INI',
      'mode.toBini': 'Текстовый INI в BINI',
      'actions.convert': 'Конвертировать',
      'actions.download': 'Скачать',
      'status.noFile': 'Файл пока не выбран.',
      'status.selected': '{fileName} выбран.',
      'status.converting': 'Локальная конвертация...',
      'status.done': '{direction}: готово.',
      'preview.title': 'Предпросмотр',
      'preview.empty': 'Текстовый результат появится здесь.',
      'preview.binary': 'Результат BINI является бинарным. Нажмите «Скачать», чтобы сохранить его.',
      'guide.eyebrow': 'Гайд по Freelancer BINI',
      'guide.title': 'Как открыть и конвертировать BINI файлы',
      'guide.open.title': 'Как открыть BINI файлы в Freelancer',
      'guide.open.text': 'BINI - бинарные конфигурационные файлы Freelancer. Перед редактированием данных кораблей, рынков, оборудования или вселенной их удобно перевести в текстовый INI.',
      'guide.convert.title': 'Конвертер BINI в INI',
      'guide.convert.text': 'Перетащите файл в конвертер, оставьте автоопределение и скачайте читаемый текстовый результат. Конвертация выполняется в браузере.',
      'guide.reverse.title': 'Сборка INI в BINI',
      'guide.reverse.text': 'После редактирования INI выберите режим «Текстовый INI в BINI» и сохраните бинарный файл для существующих инструментов Freelancer.',
      'errors.eyebrow': 'Проблемы и решения',
      'errors.title': 'Частые ошибки при открытии BINI файлов',
      'errors.read.title': 'Файл не читается',
      'errors.read.text': 'Проверьте, что это Freelancer BINI или обычный INI, а файл не был обрезан при скачивании или распаковке.',
      'errors.encoding.title': 'Проблемы с кодировкой',
      'errors.encoding.text': 'Для русских файлов Freelancer чаще всего нужна Windows-1251. Latin-1 или UTF-8 стоит пробовать, если текст выглядит битым.',
      'errors.corrupt.title': 'Поврежденные данные',
      'errors.corrupt.text': 'Некорректный заголовок BINI, отсутствующая таблица строк или неподдерживаемые значения могут остановить конвертацию.',
      'pages.eyebrow': 'Еще гайды',
      'pages.title': 'Темы по Freelancer BINI',
      'pages.biniToIni': 'Конвертация бинарных Freelancer BINI в читаемый INI.',
      'pages.freelancer.title': 'BINI файлы Freelancer',
      'pages.freelancer.text': 'Что такое BINI файлы и как с ними работают моддеры.',
      'footer.local': 'Файлы обрабатываются локально в вашем браузере.'
    }
  };

  const storageKey = 'lizerium.bini.portal.language';
  const fileInput = document.getElementById('file');
  const dropzone = document.getElementById('dropzone');
  const convertButton = document.getElementById('convert');
  const downloadButton = document.getElementById('download');
  const languageSelect = document.getElementById('language');
  const modeSelect = document.getElementById('mode');
  const encodingSelect = document.getElementById('encoding');
  const status = document.getElementById('status');
  const preview = document.getElementById('preview');
  const resultName = document.getElementById('resultName');

  let currentLanguage = detectLanguage();
  let currentFile = null;
  let currentBlob = null;
  let currentName = null;

  applyLanguage();
  bindEvents();

  function bindEvents() {
    languageSelect.addEventListener('change', () => {
      currentLanguage = languageSelect.value;
      localStorage.setItem(storageKey, currentLanguage);
      if (navigateToLanguagePage(currentLanguage)) {
        return;
      }

      applyLanguage();
    });

    dropzone.addEventListener('click', () => fileInput.click());
    dropzone.addEventListener('dragover', event => {
      event.preventDefault();
      dropzone.classList.add('dragover');
    });
    dropzone.addEventListener('dragleave', () => dropzone.classList.remove('dragover'));
    dropzone.addEventListener('drop', event => {
      event.preventDefault();
      dropzone.classList.remove('dragover');
      setFile(event.dataTransfer.files[0]);
    });

    fileInput.addEventListener('change', () => setFile(fileInput.files[0]));
    convertButton.addEventListener('click', convert);
    downloadButton.addEventListener('click', download);
  }

  function detectLanguage() {
    const queryLanguage = new URLSearchParams(location.search).get('lang');
    if (queryLanguage === 'en' || queryLanguage === 'ru') {
      return queryLanguage;
    }

    const pageLanguage = document.documentElement.dataset.defaultLanguage;
    if (pageLanguage === 'en' || pageLanguage === 'ru') {
      return pageLanguage;
    }

    const saved = localStorage.getItem(storageKey);
    if (saved === 'en' || saved === 'ru') {
      return saved;
    }

    return navigator.language?.toLowerCase().startsWith('ru') ? 'ru' : 'en';
  }

  function navigateToLanguagePage(language) {
    const currentPageLanguage = document.documentElement.dataset.defaultLanguage || 'en';
    if (language === currentPageLanguage) {
      return false;
    }

    const alternate = document.querySelector(`link[rel="alternate"][hreflang="${language}"]`);
    if (!alternate?.href) {
      return false;
    }

    location.href = alternate.href;
    return true;
  }

  function applyLanguage() {
    document.documentElement.lang = currentLanguage;
    document.title = t('meta.title');
    document
      .querySelector('meta[name="description"]')
      ?.setAttribute('content', t('meta.description'));
    languageSelect.value = currentLanguage;
    document.querySelectorAll('[data-i18n]').forEach(element => {
      element.textContent = t(element.dataset.i18n);
    });

    status.textContent = currentFile ? t('status.selected', { fileName: currentFile.name }) : t('status.noFile');
    if (!currentBlob) {
      preview.textContent = t('preview.empty');
      resultName.textContent = t('preview.title');
    }
  }

  function t(key, values = {}) {
    let value = strings[currentLanguage][key] || key;
    for (const [name, replacement] of Object.entries(values)) {
      value = value.replaceAll(`{${name}}`, replacement);
    }

    return value;
  }

  function setFile(file) {
    currentFile = file;
    currentBlob = null;
    currentName = null;
    convertButton.disabled = !file;
    downloadButton.disabled = true;
    resultName.textContent = t('preview.title');
    preview.textContent = t('preview.empty');
    status.textContent = file ? t('status.selected', { fileName: file.name }) : t('status.noFile');
  }

  async function convert() {
    if (!currentFile) {
      return;
    }

    status.textContent = t('status.converting');
    convertButton.disabled = true;

    try {
      const bytes = new Uint8Array(await currentFile.arrayBuffer());
      const direction = resolveDirection(bytes);
      const encoding = encodingSelect.value;

      if (direction === 'to-text') {
        const text = LizeriumBini.convertBiniToText(bytes, { encoding });
        const outputBytes = LizeriumBini.encode(text, encoding);
        currentBlob = new Blob([outputBytes], { type: 'text/plain' });
        currentName = outputName(currentFile.name, '.text');
        preview.textContent = text;
      } else {
        const bini = LizeriumBini.convertTextBytesToBini(bytes, { encoding });
        currentBlob = new Blob([bini], { type: 'application/octet-stream' });
        currentName = outputName(currentFile.name, '.bini');
        preview.textContent = t('preview.binary');
      }

      resultName.textContent = currentName;
      downloadButton.disabled = false;
      status.textContent = t('status.done', { direction });
    } catch (error) {
      status.textContent = error.message;
    } finally {
      convertButton.disabled = false;
    }
  }

  function resolveDirection(bytes) {
    const requested = modeSelect.value;
    if (requested !== 'auto') {
      return requested;
    }

    return LizeriumBini.isBini(bytes) ? 'to-text' : 'to-bini';
  }

  function outputName(fileName, suffix) {
    const dot = fileName.lastIndexOf('.');
    if (dot <= 0) {
      return `${fileName}${suffix}`;
    }

    return `${fileName.slice(0, dot)}${suffix}${fileName.slice(dot)}`;
  }

  function download() {
    if (!currentBlob || !currentName) {
      return;
    }

    const link = document.createElement('a');
    link.href = URL.createObjectURL(currentBlob);
    link.download = currentName;
    link.click();
    URL.revokeObjectURL(link.href);
  }
})();
