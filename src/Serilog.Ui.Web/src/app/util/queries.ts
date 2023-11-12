import { AuthProperties } from 'app/Authorization/AuthProperties';
import { AuthType } from '../../types/types';

export const determineHost = ['development', 'test'].includes(import.meta.env.MODE ?? '')
  ? ''
  : location.pathname.replace('/index.html', '');

// TODO: review the whole token auth
export const createAuthHeaders = (authProps: AuthProperties): RequestInit => {
  const headers: Headers = new Headers();

  const notWindowsAuth = authProps.authType !== AuthType.Windows;
  if (notWindowsAuth) headers.set('Authorization', authProps.bearerToken);

  const credentials = notWindowsAuth ? 'include' : 'same-origin';

  return { headers, credentials };
};
