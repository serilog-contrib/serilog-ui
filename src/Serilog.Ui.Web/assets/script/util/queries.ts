import { AuthType } from '../../types/types';
import { type AuthProperties } from '../Authorization/AuthProperties';

export const determineHost = ['development', 'test'].includes(process.env.NODE_ENV ?? '')
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
