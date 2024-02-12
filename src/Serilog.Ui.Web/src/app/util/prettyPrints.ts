import { type MantineTheme } from '@mantine/core';
import formatXml from 'xml-formatter';
import { LogLevel } from '../../types/types';
import {
  formatLocalDate,
  formatLocalSplitDate,
  formatUtcDate,
  formatUtcSplitDate,
} from './dates';

export const serilogUiUrl = 'https://github.com/serilog-contrib/serilog-ui';

export const getBgLogLevel = (theme: MantineTheme, logLevel: LogLevel): string => {
  switch (logLevel) {
    case LogLevel.Verbose:
    case LogLevel.Debug:
      return theme.colors.green[7];
    case LogLevel.Information:
      return theme.colors.blue[6];
    case LogLevel.Warning:
      return theme.colors.yellow[6];
    case LogLevel.Error:
      return theme.colors.red[8];
    case LogLevel.Fatal:
      return theme.colors.red[9];
    default:
      return theme.colors.cyan[0];
  }
};

export const printDate = (date: string, utc?: boolean) =>
  utc ? formatUtcDate(date) : formatLocalDate(date);

export const splitPrintDate = (date: string, utc?: boolean) =>
  utc ? formatUtcSplitDate(date) : formatLocalSplitDate(date);

export const renderCodeContent = (contentType: string = '', modalContent: string) => {
  if (!modalContent) return contentType;

  try {
    if (contentType === 'xml') {
      return formatXml(modalContent, { forceSelfClosingEmptyTag: true });
    }

    if (contentType === 'json') {
      return JSON.stringify(JSON.parse(modalContent), null, 2) ?? '{}';
    }
  } catch {
    console.warn(`${modalContent} is not a valid json!`);
    return `Content could not be parsed, as per expected type: ${contentType}`;
  }

  return contentType;
};
