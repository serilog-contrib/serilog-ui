import { dbKeysMock } from '__tests__/_setup/mocks/samples';
import { renderHook, waitFor } from '__tests__/_setup/testing-utils';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { AuthType } from 'types/types';
import { describe, expect, it } from 'vitest';

describe('useSearchForm', () => {
  it('renders and sets default table key on reset', async () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

    const { result } = renderHook(() => useSearchForm(), { authType: AuthType.Jwt });

    await waitFor(() => {
      result.current.reset();
      expect(result.current.getValues('table')).toBe(dbKeysMock[0]);
    });
  });

  it('renders and leaves default table key undefined, if internal query was not successful', async () => {
    sessionStorage.removeItem(IAuthPropertiesStorageKeys.jwt_bearerToken);

    const { result } = renderHook(() => useSearchForm(), { authType: AuthType.Jwt });

    await waitFor(() => {
      result.current.reset();
      expect(result.current.getValues('table')).toBe('');
    });
  });
});
