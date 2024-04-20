import {
  isArrayGuard,
  isNotNullGuard,
  isObjectGuard,
  isStringGuard,
  toNumber,
} from 'app/util/guards';
import { describe, expect, it } from 'vitest';

describe('guards', () => {
  it.each([
    [null, false],
    [, true],
    ['test', true],
    [{}, true],
    [[], true],
    [10, true],
    ['', true],
  ])('guards is not null on %s', (value, expected) => {
    const sut = isNotNullGuard(value);
    expect(sut).toBe(expected);
  });

  it.each([
    [null, false],
    [, false],
    [0, false],
    [10, false],
    [true, false],
    [false, false],
    ['', false],
    ['test', true],
  ])('guards string on %s', (value, expected) => {
    const sut = isStringGuard(value);
    expect(sut).toBe(expected);
  });

  it.each([
    [null, false],
    [, false],
    [[], false],
    [[0], true],
  ])('guards array on %s', (value, expected) => {
    const sut = isArrayGuard(value);
    expect(sut).toBe(expected);
  });

  it.each([
    [null, false],
    [, false],
    [0, true],
    [10, true],
    [true, true],
    ['', true],
    ['test', true],
    [{}, true],
  ])('guards object on %s', (value, expected) => {
    const sut = isObjectGuard(value);
    expect(sut).toBe(expected);
  });

  it('converts string to number or fallbacks to null', () => {
    const sut = toNumber('10');
    expect(sut).toBe(10);

    const fail = toNumber('re');
    expect(fail).toBeNull();

    const fail2 = toNumber('10.01');
    expect(fail2).toBe(10);
  });
});
