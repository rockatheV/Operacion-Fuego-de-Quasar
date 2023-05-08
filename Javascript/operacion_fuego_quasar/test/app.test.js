/* eslint-disable */
import { GetLocation, GetMessage } from '../src/app.js';

test('Prueba de GetLocation', () => {
    const result = GetLocation([412.31, 282.84, 721.11]);
    expect(Math.round(result.x)).toBe(-100);
    expect(Math.round(result.y)).toBe(-300);
});

test('Prueba de GetMessage', () => {
    const result = GetMessage([
        ["", "este", "es", "un", "mensaje"],
        ["este", "", "un", "mensaje"],
        ["", "", "es", "", "mensaje"]
    ]);
    expect(result).toBe("este es un mensaje");
});