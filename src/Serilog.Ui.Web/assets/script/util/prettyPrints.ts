import { parseJSON, format } from 'date-fns';
import { LogLevel } from '../../types/types';
import { type MantineTheme } from '@mantine/core';

export const cleanHtmlTags = (onReplace: string) => {
  return onReplace
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
};

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

export const printDate = (date: string) =>
  format(parseJSON(date), 'PP H:mm:ss.SSS', { weekStartsOn: 1 });

export const printXmlCode = (xml: string, tab = '\t') => {
  let formatted = '';
  let indent = '';
  xml.split(/>\s*</).forEach((node) => {
    // decrease indent by one "tab"
    if (node.match(/^\/\w/) != null) indent = indent.substring(tab.length);
    formatted += indent + '<' + node + '>\r\n';
    // increase indent
    if (node.match(/^<?\w[^>]*[^/]$/) != null) indent += tab;
  });
  return formatted.substring(1, formatted.length - 3);
};
