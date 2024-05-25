import { isValidJwtUnixDate } from 'app/util/dates';
import dayjs from 'dayjs';
import { describe, expect, it } from 'vitest';

describe('util: dates', () => {
  it('returns true if unix time is after now', () => {
    const validUnix = dayjs().add(1, 'day').unix();
    const sut = isValidJwtUnixDate(validUnix);
    expect(sut).toBeTruthy();

    const invalidUnix = dayjs().subtract(1, 'day').unix();
    const invalidSut = isValidJwtUnixDate(invalidUnix);
    expect(invalidSut).toBeFalsy();
  });
});
