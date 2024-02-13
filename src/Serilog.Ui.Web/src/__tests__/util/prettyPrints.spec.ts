import { MantineColorScheme, MantineTheme } from '@mantine/core';
import { theme } from 'style/theme';
import { describe, expect, it, vi } from 'vitest';
import { getBgLogLevel, renderCodeContent } from '../../app/util/prettyPrints';
import { LogLevel, LogType } from '../../types/types';

describe('pretty prints util', () => {
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

  describe('code content render', () => {
    it('returns xml prettified', () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = renderCodeContent(LogType.Xml, '<root><my-xml>sample</my-xml></root>');

      expect(act).toBe(
        '<root>\r\n    <my-xml>\r\n        sample\r\n    </my-xml>\r\n</root>',
      );
    });

    it('returns json prettified', () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = renderCodeContent(LogType.Json, '{ "json": "ok"}');
      const result = {
        json: 'ok',
      };
      expect(act).toBe(JSON.stringify(result, null, 2));
    });

    it('returns error message if content cannot be parsed', () => {
      const consoleMock = vi.fn();
      console.warn = consoleMock;
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const actXml = renderCodeContent(LogType.Xml, 'not an XML!');

      expect(actXml).toBe('Content could not be parsed, as per expected type: xml');

      // eslint-disable-next-line testing-library/render-result-naming-convention
      const actJson = renderCodeContent(LogType.Json, 'not a JSON!');

      expect(actJson).toBe('Content could not be parsed, as per expected type: json');

      expect(consoleMock).toHaveBeenCalledTimes(2);
    });

    it.each(['', '  '])('returns content if input: %s [invalid]', (input) => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = renderCodeContent(LogType.Json, input);

      expect(act).toBe(input || '');
    });

    it('returns content if type is not expected', () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = renderCodeContent('fake-content', '{ "json": "ok"}');

      expect(act).toBe('{ "json": "ok"}');
    });
  });
});
