import { MantineTheme } from '@mantine/core';
import { describe, expect, it, vi } from 'vitest';
import { getBgLogLevel } from '../../app/util/prettyPrints';
import { LogLevel } from '../../types/types';

describe('log level backgrounds', () => {
  it.each([
    [LogLevel.Verbose, 'bg-success'],
    [LogLevel.Debug, 'bg-success'],
    [LogLevel.Error, 'bg-danger'],
    [LogLevel.Fatal, 'bg-danger'],
    [LogLevel.Information, 'bg-primary'],
    [LogLevel.Warning, 'bg-warning'],
  ])('TODO returns %s color for log level: %s', (logLevelEntry, expectedBg) => {
    const result = getBgLogLevel(
      vi.fn() as unknown as MantineTheme,
      'dark',
      logLevelEntry,
    );
    expect(result).toBe(expectedBg);
  });

  it('TODO: returns default color for unrecognized enum case', () => {
    const result = getBgLogLevel(
      vi.fn() as unknown as MantineTheme,
      'dark',
      'fakeEnum' as LogLevel,
    );
    expect(result).toBe('bg-secondary');
  });
});
