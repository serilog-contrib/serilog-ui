import { MantineColorScheme, type MantineTheme } from '@mantine/core';
import { BundledTheme, CodeOptionsMultipleThemes } from 'shiki';
import { highlighter } from 'style/shikijiBundle';
import formatXml from 'xml-formatter';
import { AdditionalColumnLogType, LogLevel, LogType } from '../../types/types';
import {
  formatLocalDate,
  formatLocalSplitDate,
  formatUtcDate,
  formatUtcSplitDate,
} from './dates';

export const serilogUiUrl = 'https://github.com/serilog-contrib/serilog-ui';

export const capitalize = (str: string) =>
  str ? str.charAt(0).toUpperCase() + str.slice(1) : str;

export const getBgLogLevel = (
  theme: MantineTheme,
  colorScheme: MantineColorScheme,
  logLevel: LogLevel,
): string => {
  switch (logLevel) {
    case LogLevel.Verbose:
      return colorScheme === 'dark' ? theme.colors.green[9] : theme.colors.green[7];
    case LogLevel.Debug:
      return colorScheme === 'dark' ? theme.colors.blue[9] : theme.colors.blue[4];
    case LogLevel.Information:
      return colorScheme === 'dark' ? theme.colors.blue[6] : theme.colors.blue[3];
    case LogLevel.Warning:
      return colorScheme === 'dark' ? theme.colors.yellow[9] : theme.colors.yellow[5];
    case LogLevel.Error:
      return colorScheme === 'dark' ? theme.colors.red[6] : theme.colors.red[4];
    case LogLevel.Fatal:
      return colorScheme === 'dark' ? theme.colors.red[9] : theme.colors.red[8];
    default:
      return theme.colors.cyan[0];
  }
};

export const printDate = (date: string, utc?: boolean) =>
  utc ? formatUtcDate(date) : formatLocalDate(date);

export const splitPrintDate = (date: string, utc?: boolean) =>
  utc ? formatUtcSplitDate(date) : formatLocalSplitDate(date);

export const convertLogType = (logType: AdditionalColumnLogType) => {
  if (logType === AdditionalColumnLogType.Json) return LogType.Json;
  if (logType === AdditionalColumnLogType.Xml) return LogType.Xml;

  return '' as LogType;
};

const shikijiThemes: CodeOptionsMultipleThemes<BundledTheme> = {
  themes: {
    light: 'tokyo-night',
    dark: 'night-owl',
  },
};
export const renderCodeContent = async (
  modalContent: string,
  contentType: string = '',
) => {
  if (!modalContent?.trim() || !Object.values(LogType).includes(contentType as LogType))
    return modalContent;

  try {
    const highlighterInstance = await highlighter();
    if (contentType === LogType.Xml) {
      const xmlResult = formatXml(modalContent, { forceSelfClosingEmptyTag: true });
      return highlighterInstance.codeToHtml(xmlResult, {
        lang: 'xml',
        ...shikijiThemes,
        mergeWhitespaces: true,
      });
    }

    if (contentType === LogType.Json) {
      const jsonResult = JSON.stringify(JSON.parse(modalContent), null, 4) ?? '{}';
      return highlighterInstance.codeToHtml(jsonResult, {
        lang: 'json',
        ...shikijiThemes,
      });
    }
  } catch {
    console.warn(`${modalContent} is not a valid json!`);
    return `Content could not be parsed, as per expected type: ${contentType}`;
  }
};
