import { parseJSON, format } from 'date-fns';
import { LogLevel } from '../types/types';

export const formatDate = (date: string) =>
  format(parseJSON(date), 'PP H:mm:ss.SSS', { weekStartsOn: 1 });

export const fixedLengthMessageWithModal = (str: string, sliceEnd: number) => {
  if (str.length <= sliceEnd) {
    return str;
  }

  const truncated = str.slice(0, sliceEnd) + '...';
  const html = `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
                    ${truncated}
                    <span style=\"display: none\">${str}</span>
                  </a>`;
  return html;
};

export const cleanHtmlTags = (str: string) => {
  return String(str)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
};

export const getBgLogLevel = (logLevel: LogLevel) => {
  switch (logLevel) {
    case LogLevel.Verbose:
    case LogLevel.Debug:
      return 'bg-success';
    case LogLevel.Information:
      return 'bg-primary';
    case LogLevel.Warning:
      return 'bg-warning';
    case LogLevel.Error:
    case LogLevel.Fatal:
      return 'bg-danger';
    default:
      return 'bg-secondary';
  }
};
