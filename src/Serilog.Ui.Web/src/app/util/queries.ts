export const determineHost = ['development', 'test'].includes(import.meta.env.MODE ?? '')
  ? ''
  : location.pathname.replace('/index.html', '');

// TODO: review the whole token auth
export const createAuthHeaders = (header?: string): RequestInit => {
  const headers: Headers = new Headers();

  // TODO move in class...
  // const notWindowsAuth = authProps.authType !== AuthType.Windows;
  if (header) {
    headers.set('Authorization', header);
  }

  // const credentials = notWindowsAuth ? 'include' : 'same-origin';
  const credentials = 'same-origin'; // tmp
  return { headers, credentials };
};
