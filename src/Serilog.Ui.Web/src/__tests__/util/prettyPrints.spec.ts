import { MantineColorScheme, MantineTheme } from '@mantine/core';
import { theme } from 'style/theme';
import { describe, expect, it } from 'vitest';
import { getBgLogLevel } from '../../app/util/prettyPrints';
import { LogLevel } from '../../types/types';

describe('log level backgrounds', () => {
  it.each([
    {
      logLevelEntry: LogLevel.Verbose,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.green[9],
    },
    {
      logLevelEntry: LogLevel.Debug,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.blue[9],
    },
    {
      logLevelEntry: LogLevel.Information,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.blue[6],
    },
    {
      logLevelEntry: LogLevel.Warning,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.yellow[9],
    },
    {
      logLevelEntry: LogLevel.Error,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.red[6],
    },
    {
      logLevelEntry: LogLevel.Fatal,
      colorScheme: 'dark',
      expectedBg: (theme: MantineTheme) => theme.colors.red[9],
    },
  ])(
    'returns color for log level $logLevelEntry and scheme $colorScheme',
    ({ logLevelEntry, colorScheme, expectedBg }) => {
      const result = getBgLogLevel(
        theme as MantineTheme,
        colorScheme as MantineColorScheme,
        logLevelEntry,
      );
      expect(result).toBe(expectedBg(theme as MantineTheme));
    },
  );

  it('returns default color for unrecognized enum case', () => {
    const themeWithDefault = {
      ...theme,
      colors: { ...theme.colors, cyan: ['#111111', '#222222'] },
    } as unknown as MantineTheme;
    const result = getBgLogLevel(themeWithDefault, 'dark', 'fakeEnum' as LogLevel);
    expect(result).toBe('#111111');
  });
});
