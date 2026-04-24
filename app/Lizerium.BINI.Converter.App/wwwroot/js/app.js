const languageSelect = document.getElementById('language');
const zone = document.getElementById('dropzone');
const fileInput = document.getElementById('file');
const convertButton = document.getElementById('convert');
const downloadButton = document.getElementById('download');
const mode = document.getElementById('mode');
const encoding = document.getElementById('encoding');
const status = document.getElementById('status');
const preview = document.getElementById('preview');
const resultName = document.getElementById('resultName');

const storageKey = 'lizerium.bini.converter.language';
const supportedLanguages = ['en', 'ru'];

let currentFile = null;
let currentBlob = null;
let currentName = null;
let dictionary = {};
let selectedLanguage = detectLanguage();

init();

async function init() {
  await setLanguage(selectedLanguage);
  bindEvents();
}

function bindEvents() {
  languageSelect.addEventListener('change', () => setLanguage(languageSelect.value, true));
  zone.addEventListener('click', () => fileInput.click());
  zone.addEventListener('dragover', event => {
    event.preventDefault();
    zone.classList.add('dragover');
  });
  zone.addEventListener('dragleave', () => zone.classList.remove('dragover'));
  zone.addEventListener('drop', event => {
    event.preventDefault();
    zone.classList.remove('dragover');
    setFile(event.dataTransfer.files[0]);
  });
  fileInput.addEventListener('change', () => setFile(fileInput.files[0]));
  convertButton.addEventListener('click', convert);
  downloadButton.addEventListener('click', downloadResult);
}

function detectLanguage() {
  const saved = localStorage.getItem(storageKey);
  if (supportedLanguages.includes(saved)) {
    return saved;
  }

  const browserLanguage = navigator.language?.slice(0, 2).toLowerCase();
  return supportedLanguages.includes(browserLanguage) ? browserLanguage : 'en';
}

async function setLanguage(language, persist = false) {
  selectedLanguage = supportedLanguages.includes(language) ? language : 'en';
  const response = await fetch(`/locales/${selectedLanguage}.json`);
  dictionary = await response.json();

  document.documentElement.lang = selectedLanguage;
  languageSelect.value = selectedLanguage;
  document.querySelectorAll('[data-i18n]').forEach(element => {
    element.textContent = t(element.dataset.i18n);
  });

  if (persist) {
    localStorage.setItem(storageKey, selectedLanguage);
  }

  refreshDynamicText();
}

function t(key, values = {}) {
  let text = dictionary[key] || key;
  for (const [name, value] of Object.entries(values)) {
    text = text.replaceAll(`{${name}}`, value);
  }

  return text;
}

function refreshDynamicText() {
  if (currentFile) {
    status.textContent = t('status.selected', { fileName: currentFile.name });
    return;
  }

  status.textContent = t('status.noFile');
  preview.textContent = t('preview.empty');
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

  const form = new FormData();
  form.append('file', currentFile);
  status.textContent = t('status.converting');
  convertButton.disabled = true;

  try {
    const query = new URLSearchParams({
      mode: mode.value,
      encoding: encoding.value
    });
    const response = await fetch(`/api/convert?${query}`, {
      method: 'POST',
      body: form
    });
    const data = await response.json();
    if (!response.ok) {
      throw new Error(data.error || t('errors.conversionFailed'));
    }

    const bytes = Uint8Array.from(atob(data.base64), char => char.charCodeAt(0));
    currentBlob = new Blob([bytes], {
      type: data.isText ? 'text/plain' : 'application/octet-stream'
    });
    currentName = data.fileName;
    resultName.textContent = data.fileName;
    preview.textContent = data.isText ? data.preview : t('preview.binary');
    downloadButton.disabled = false;
    status.textContent = t('status.done', { direction: data.direction });
  } catch (error) {
    status.textContent = error.message;
  } finally {
    convertButton.disabled = false;
  }
}

function downloadResult() {
  if (!currentBlob || !currentName) {
    return;
  }

  const link = document.createElement('a');
  link.href = URL.createObjectURL(currentBlob);
  link.download = currentName;
  link.click();
  URL.revokeObjectURL(link.href);
}
