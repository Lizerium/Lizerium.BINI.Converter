export type BiniEncoding = 'cp1251' | 'windows-1251' | 'win1251' | '1251' | 'latin1' | 'latin-1' | 'iso-8859-1' | 'utf8' | 'utf-8';

export interface BiniConvertOptions {
  encoding?: BiniEncoding | string;
}

export type BinaryInput = ArrayBuffer | Uint8Array;

export function isBini(data: BinaryInput): boolean;

export function convertBiniToText(
  data: BinaryInput,
  options?: BiniConvertOptions
): string;

export function convertTextToBini(
  text: string,
  options?: BiniConvertOptions
): Uint8Array;
