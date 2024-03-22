import { MantineColorScheme, MantineTheme } from '@mantine/core';
import dayjs from 'dayjs';
import { theme } from 'style/theme';
import { describe, expect, it, vi } from 'vitest';
import {
  getBgLogLevel,
  printDate,
  renderCodeContent,
  splitPrintDate,
} from '../../app/util/prettyPrints';
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
    it('returns xml prettified', async () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = await renderCodeContent(
        LogType.Xml,
        '<root><my-xml>sample</my-xml></root>',
      );

      expect(act).toBe(
        '<root>\r\n    <my-xml>\r\n        sample\r\n    </my-xml>\r\n</root>',
      );
    });

    it('returns json prettified', async () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = await renderCodeContent(LogType.Json, '{ "json": "ok"}');
      const result = {
        json: 'ok',
      };
      expect(act).toBe(JSON.stringify(result, null, 2));
    });

    it('returns error message if content cannot be parsed', async () => {
      const consoleMock = vi.fn();
      console.warn = consoleMock;
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const actXml = await renderCodeContent(LogType.Xml, 'not an XML!');

      expect(actXml).toBe('Content could not be parsed, as per expected type: xml');

      // eslint-disable-next-line testing-library/render-result-naming-convention
      const actJson = await renderCodeContent(LogType.Json, 'not a JSON!');

      expect(actJson).toBe('Content could not be parsed, as per expected type: json');

      expect(consoleMock).toHaveBeenCalledTimes(2);
    });

    it.each(['', '  '])('returns content if input: %s [invalid]', async (input) => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = await renderCodeContent(LogType.Json, input);

      expect(act).toBe(input || '');
    });

    it('returns content if type is not expected', async () => {
      // eslint-disable-next-line testing-library/render-result-naming-convention
      const act = await renderCodeContent('fake-content', '{ "json": "ok"}');

      expect(act).toBe('{ "json": "ok"}');
    });
  });

  describe('date prints', () => {
    it('print local date from iso string', () => {
      const time = '2022-09-27T14:15:10.000Z';
      const parsedDate = dayjs(time);

      const sut = printDate(time);

      expect(sut).toBe(`Sep 27, 2022 ${parsedDate.hour()}:15:10`);
    });

    it('print utc date from iso string', () => {
      const time = '2022-09-27T14:15:10.000Z';
      const sut = printDate(time, true);

      expect(sut).toBe(`Sep 27, 2022 14:15:10 [UTC]`);
    });

    it('return split local date from iso string', () => {
      const time = '2022-09-27T14:15:10.000Z';
      const parsedDate = dayjs(time);

      const sut = splitPrintDate(time);

      expect(sut[0]).toBe(`Sep 27, 2022`);
      expect(sut[1]).toBe(`${parsedDate.hour()}:15:10`);
    });

    it('return split utc date from iso string', () => {
      const time = '2022-09-27T14:15:10.000Z';
      const sut = splitPrintDate(time, true);

      expect(sut[0]).toBe(`Sep 27, 2022`);
      expect(sut[1]).toBe(`14:15:10 [UTC]`);
    });
  });
});
