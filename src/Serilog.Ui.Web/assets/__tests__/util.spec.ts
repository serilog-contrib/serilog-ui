import { expect, describe, it, vi } from 'vitest';
import { getBgLogLevel } from '../script/util/prettyPrints';
import { LogLevel } from '../types/types';
import { MantineTheme } from '@mantine/core';

describe('log level backgrounds', () => {
  it.skip.each([
    [LogLevel.Verbose, 'bg-success'],
    [LogLevel.Debug, 'bg-success'],
    [LogLevel.Error, 'bg-danger'],
    [LogLevel.Fatal, 'bg-danger'],
    [LogLevel.Information, 'bg-primary'],
    [LogLevel.Warning, 'bg-warning'],
  ])('TODO returns %s color for log level: %s', (logLevelEntry, expectedBg) => {
    const result = getBgLogLevel(vi.fn() as unknown as MantineTheme, logLevelEntry);
    expect(result).toBe(expectedBg);
  });

  it.skip('TODO: returns default color for unrecognized enum case', () => {
    const result = getBgLogLevel(
      vi.fn() as unknown as MantineTheme,
      'fakeEnum' as LogLevel,
    );
    expect(result).toBe('bg-secondary');
  });
});
