(function () {
	'use strict';

	const storageKey = 'lizerium.bini.portal.language';
	const supported = new Set(['en', 'ru']);
	const switcher = document.querySelector('[data-language-switcher]');

	if (!switcher) {
		return;
	}

	const currentLanguage = switcher.dataset.currentLanguage || document.documentElement.lang || 'en';
	const alternates = {
		en: switcher.dataset.languageEnUrl,
		ru: switcher.dataset.languageRuUrl,
	};

	Array.from(document.querySelectorAll('link[rel="alternate"][hreflang]'))
		.filter(link => supported.has(link.hreflang))
		.forEach(link => {
			alternates[link.hreflang] ||= link.href;
		});

	switcher.value = currentLanguage;

	const savedLanguage = localStorage.getItem(storageKey);
	const targetLanguage = supported.has(savedLanguage)
		? savedLanguage
		: detectBrowserLanguage();

	if (!isLikelyBot() && targetLanguage !== currentLanguage && alternates[targetLanguage]) {
		location.replace(withHash(alternates[targetLanguage]));
		return;
	}

	switcher.addEventListener('change', () => {
		const nextLanguage = switcher.value;
		localStorage.setItem(storageKey, nextLanguage);

		if (nextLanguage !== currentLanguage && alternates[nextLanguage]) {
			location.href = withHash(alternates[nextLanguage]);
		}
	});

	function detectBrowserLanguage() {
		const languages = navigator.languages && navigator.languages.length
			? navigator.languages
			: [navigator.language || 'en'];

		return languages.some(language => language.toLowerCase().startsWith('ru')) ? 'ru' : 'en';
	}

	function withHash(url) {
		const target = new URL(url, location.href);
		target.hash = location.hash;
		return target.href;
	}

	function isLikelyBot() {
		return /bot|crawl|spider|slurp|duckduck|bingpreview|yandex/i.test(navigator.userAgent || '');
	}
})();
