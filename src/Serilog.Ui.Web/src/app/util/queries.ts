import { DefaultMantineColor } from '@mantine/core';
import { notifications } from '@mantine/notifications';
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

export const send403Notification = () => {
  notifications.show({
    title: 'Unauthorized',
    message:
      "You are not logged in or you don't have enough permissions to perform the requested operation",
    color: 'red',
    radius: 'md',
    withBorder: true,
  });
};

export const sendUnexpectedNotification = (
  message: string,
  title: string = 'Query error',
  color: DefaultMantineColor = 'red',
) => {
  notifications.show({
    title,
    message,
    color: color,
    radius: 'md',
    withBorder: true,
    autoClose: 5000,
  });
};
