import { notifications } from '@mantine/notifications';
import {
  createRequestInit,
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from 'app/util/queries';
import { AuthType } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

describe('queries util', () => {
  it('determines test-dev host', () => {
    import.meta.env.MODE = 'test';
    const sutTest = determineHost();
    expect(sutTest).toBe('https://localhost:3001');

    import.meta.env.MODE = 'development';
    const sutDev = determineHost('prefix');
    expect(sutDev).toBe('https://localhost:3001');
  });

  it('determines production host', () => {
    import.meta.env.MODE = 'production';
    window.location.assign('https://www.test.com/my-test/sub/page?query=ok');

    const sut = determineHost('prefix');
    expect(sut).toBe('https://www.test.com/prefix');

    const sutNoPrefix = determineHost();
    expect(sutNoPrefix).toBe('https://www.test.com/');
  });

  it('creates request init', () => {
    const sutNoHeader = createRequestInit();

    expect(new Headers(sutNoHeader.headers).get('Authorization')).toBeNull();
    expect(sutNoHeader.credentials).toBe('include');

    const sutWitHeader = createRequestInit(AuthType.Custom, 'header');

    expect(new Headers(sutWitHeader.headers).get('Authorization')).toBe('header');
    expect(sutWitHeader.credentials).toBe('same-origin');
  });

  it('sends unauthorized notification', () => {
    const spy = vi.spyOn(notifications, 'show');
    send403Notification();

    expect(spy).toHaveBeenCalledWith(
      expect.objectContaining({
        title: 'Unauthorized',
        message:
          "You are not logged in or you don't have enough permissions to perform the requested operation",
        color: 'red',
        radius: 'md',
        withBorder: true,
      }),
    );
  });

  it('send unexpected notification', () => {
    const spy = vi.spyOn(notifications, 'show');
    sendUnexpectedNotification('msg');
    sendUnexpectedNotification('msg', 'title', 'gray', 1000);

    expect(spy).toHaveBeenNthCalledWith(
      1,
      expect.objectContaining({
        title: 'Query error',
        message: 'msg',
        color: 'red',
        radius: 'md',
        withBorder: true,
        autoClose: 5000,
      }),
    );

    expect(spy).toHaveBeenNthCalledWith(
      2,
      expect.objectContaining({
        title: 'title',
        message: 'msg',
        color: 'gray',
        radius: 'md',
        withBorder: true,
        autoClose: 1000,
      }),
    );
  });
});
