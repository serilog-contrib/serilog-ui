import { SerilogUiConfig } from 'app/hooks/useSerilogUiProps';
import { AuthType } from 'types/types';

export const determineHost = ['development', 'test'].includes(import.meta.env.MODE ?? '')
  ? ''
  : location.pathname.replace('/index.html', '');

export const createAuthHeaders = (
  uiProps: SerilogUiConfig,
  header?: string,
): RequestInit => {
  const headers: Headers = new Headers();

  // TODO move in class...
  const notWindowsAuth = uiProps.authType !== AuthType.Windows;
  if (header) {
    headers.set('Authorization', header);
  }

  const credentials = notWindowsAuth ? 'include' : 'same-origin';
  return { headers, credentials };
};
