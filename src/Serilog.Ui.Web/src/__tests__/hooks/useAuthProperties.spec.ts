import { act, renderHook } from '__tests__/_setup/testing-utils';
import { useAuthProperties } from 'app/hooks/useAuthProperties';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { DispatchedCustomEvents } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

describe('useAuthProperties', () => {
  it('renders with default values', () => {
    const { result } = renderHook(() => useAuthProperties());

    expect(result.current.authHeader).toBe('');
    expect(result.current.basic_pwd).toBe('');
    expect(result.current.basic_user).toBe('');
    expect(result.current.jwt_bearerToken).toBe('');
    expect(result.current.fetchInfo).toStrictEqual(
      expect.objectContaining({
        routePrefix: 'test-serilog-ui',
      }),
    );
  });

  it('saves auth state and updates end properties', () => {
    const { result } = renderHook(() => useAuthProperties());

    act(() => {
      const validationResult = result.current.saveAuthState({
        basic_user: 'test',
        basic_pwd: 'test',
      });

      expect(validationResult.success).toBeTruthy();
      expect(validationResult.errors).toHaveLength(0);
    });

    expect(result.current.basic_user).toBe('test');
    expect(result.current.basic_pwd).toBe('test');
    expect(result.current.authHeader).toBe('Basic dGVzdDp0ZXN0');
  });

  it('saves auth state and returns info on validation errors', () => {
    const { result } = renderHook(() => useAuthProperties());

    act(() => {
      const validationResult = result.current.saveAuthState({
        jwt_bearerToken: 'jwt',
      });

      expect(validationResult.success).toBeFalsy();
      expect(validationResult.errors).toHaveLength(1); // jwt failed
    });

    expect(result.current.jwt_bearerToken).toBe('jwt');
  });

  it('clears auth state', () => {
    const mock = vi.fn()
    document.addEventListener(DispatchedCustomEvents.RemoveTableKey, mock)
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');
    sessionStorage.setItem(IAuthPropertiesStorageKeys.basic_user, 'user');

    const { result } = renderHook(() => useAuthProperties());

    expect(result.current.basic_user).toBe('user');
    expect(result.current.jwt_bearerToken).toBe('token');
    act(() => {
      result.current.clearAuthState();
    });

    expect(result.current.basic_user).toBe('');
    expect(result.current.basic_pwd).toBe('');
    expect(result.current.jwt_bearerToken).toBe('');
    expect(mock).toHaveBeenCalledOnce()
  });
});
