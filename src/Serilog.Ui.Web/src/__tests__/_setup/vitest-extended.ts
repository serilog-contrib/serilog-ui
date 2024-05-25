import dayjs, { Dayjs, OpUnitType } from 'dayjs';
import { expect } from 'vitest';

export type ToBeSameDate = (expected: Dayjs, options?: { unit?: OpUnitType }) => void;

// ref for how to write an extender: https://kettanaito.com/blog/practical-guide-to-custom-jest-matchers
expect.extend({
  toBeSameDate(received: Date, expected: Dayjs, options?: { unit?: OpUnitType }) {
    const { isNot } = this;

    const dayjsReceived = dayjs(received);
    const pass = dayjsReceived.isSame(expected, options?.unit);

    return {
      pass,
      message: () =>
        `${received.toISOString()} is ${isNot ? '' : 'not '}equal to ${expected.toISOString()}${options?.unit ? `, with unit ${options?.unit}` : ''}`,
      actual: dayjsReceived,
      expected: expected,
    };
  },
});
