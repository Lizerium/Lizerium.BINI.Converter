import fs from 'fs';
import {
  isBini,
  convertBiniToText
} from 'lizerium-bini-converter';

const bytes = fs.readFileSync('market_commodities.ini');

if (isBini(bytes)) {
  console.log(convertBiniToText(bytes));
}
