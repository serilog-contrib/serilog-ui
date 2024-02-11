import { SerilogUiConfig } from 'app/hooks/useSerilogUiProps';
import { AuthType } from 'types/types';

export const determineHost = ['development', 'test'].includes(import.meta.env.MODE ?? '')
  ? ''
  : location.pathname.replace('/index.html', '');

export const createRequestInit = (
  uiProps: SerilogUiConfig,
  header?: string,
): RequestInit => {
  const notWindowsAuth = uiProps.authType !== AuthType.Windows;

  const headers: Headers = new Headers();

  if (header) {
    headers.set('Authorization', header);
  }

  const credentials = notWindowsAuth ? 'include' : 'same-origin';
  return { headers, credentials };
};
