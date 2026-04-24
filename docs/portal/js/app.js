(function () {
  'use strict';

  const strings = {
    en: {
      'drop.title': 'Drop an .ini file here',
      'drop.subtitle': 'or click to choose one',
      'mode.auto': 'Auto',
      'mode.toText': 'BINI -> INI text',
      'mode.toBini': 'INI text -> BINI',
      'actions.convert': 'Convert',
      'actions.download': 'Download',
      'status.noFile': 'No file selected.',
      'status.selected': '{fileName} selected.',
      'status.converting': 'Converting locally...',
      'status.done': '{direction}: done.',
      'preview.title': 'Preview',
      'preview.empty': 'Text output will appear here.',
      'preview.binary': 'BINI output is binary. Use Download to save it.'
    },
    ru: {
      'drop.title': 'Перетащи .ini файл сюда',
      'drop.subtitle': 'или нажми, чтобы выбрать его',
      'mode.auto': 'Авто',
      'mode.toText': 'BINI -> текстовый INI',
      'mode.toBini': 'Текстовый INI -> BINI',
      'actions.convert': 'Конвертировать',
      'actions.download': 'Скачать',
      'status.noFile': 'Файл пока не выбран.',
      'status.selected': '{fileName} выбран.',
      'status.converting': 'Локальная конвертация...',
      'status.done': '{direction}: готово.',
      'preview.title': 'Предпросмотр',
      'preview.empty': 'Текстовый результат появится здесь.',
      'preview.binary': 'Результат BINI является бинарным. Используй Download, чтобы сохранить его.'
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
    const saved = localStorage.getItem(storageKey);
    if (saved === 'en' || saved === 'ru') {
      return saved;
    }

    return navigator.language?.toLowerCase().startsWith('ru') ? 'ru' : 'en';
  }

  function applyLanguage() {
    document.documentElement.lang = currentLanguage;
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
