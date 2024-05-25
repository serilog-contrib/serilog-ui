import { notifications } from '@mantine/notifications';
import { act, renderHook } from '__tests__/_setup/testing-utils';
import { useJwtTimeout } from 'app/hooks/useJwtTimeout';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { AuthType } from 'types/types';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

describe('useJwtTimeout', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('send error notification once on invalid jwt', async () => {
    const spy = vi.spyOn(notifications, 'show');
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'invalid_token');

    const {} = renderHook(() => useJwtTimeout(), { authType: AuthType.Jwt });

    await act(async () => {
      await vi.advanceTimersByTimeAsync(300005);
    });

    await act(async () => {
      await vi.advanceTimersByTimeAsync(300005);
    });

    expect(spy).toHaveBeenCalledOnce();
    expect(spy).toHaveBeenNthCalledWith(
      1,
      expect.objectContaining({
        title: 'Auth validation',
        color: 'yellow',
      }),
    );
  });

  it('not send error notification if valid jwt', async () => {
    const spy = vi.spyOn(notifications, 'show');
    sessionStorage.setItem(
      IAuthPropertiesStorageKeys.jwt_bearerToken,
      'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjI5MTYyMzkwMjJ9.mYtgSqdUIxK8_RnYBTUP4cmpKw83aKi7cMiixF3qMB4',
    );

    const {} = renderHook(() => useJwtTimeout(), { authType: AuthType.Jwt });

    await act(async () => {
      await vi.advanceTimersByTimeAsync(300005);
    });

    expect(spy).not.toHaveBeenCalledOnce();
  });

  it('not send error notification if auth type is not jwt', async () => {
    const spy = vi.spyOn(notifications, 'show');
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'invalid-token');

    const {} = renderHook(() => useJwtTimeout(), { authType: AuthType.Custom });

    await act(async () => {
      await vi.advanceTimersByTimeAsync(300005);
    });

    expect(spy).not.toHaveBeenCalledOnce();
  });
});
