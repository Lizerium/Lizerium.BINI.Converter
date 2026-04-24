import assert from 'node:assert/strict';
import {
  isBini,
  convertBiniToText,
  convertTextToBini
} from '../src/index.js';

const text = `[Commodities]
iron = 1.42, 300, icons\\iron.bmp, "+1", -0

[Weapons]
red_laser_beam = 255, 0, 0, "Laser Beam, ""Red"""
`;

const bini = convertTextToBini(text, { encoding: 'latin1' });
assert.equal(isBini(bini), true);

const restored = convertBiniToText(bini, { encoding: 'latin1' });
assert.match(restored, /\[Commodities\]/);
assert.match(restored, /"\+1"/);
assert.match(restored, /-0\.0/);

console.log('Smoke test passed.');
