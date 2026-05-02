import { dbKeysMock } from '__tests__/_setup/mocks/samples';
import { renderHookSerilogUiTestWrapper, waitFor } from '__tests__/_setup/testing-utils';
import { useQueryParamReader } from 'app/hooks/useQueryParamSync';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { AuthType } from 'types/types';
import { describe, expect, it } from 'vitest';

const useSearchFormTester = () => {
  useQueryParamReader();
  return useSearchForm();
};

describe('useSearchForm', () => {
  it('sets default table key on reset', async () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

    const { result } = renderHookSerilogUiTestWrapper(() => useSearchFormTester(), {
      authType: AuthType.Jwt,
    });

    await waitFor(() => {
      result.current.reset();
      expect(result.current.getValues('table')).toBe(dbKeysMock[0]);
    });
  });

  it('renders and leaves default table key undefined, if internal query was not successful', async () => {
    sessionStorage.removeItem(IAuthPropertiesStorageKeys.jwt_bearerToken);

    const { result } = renderHookSerilogUiTestWrapper(() => useSearchFormTester(), {
      authType: AuthType.Jwt,
    });

    await waitFor(() => {
      result.current.reset();
      expect(result.current.getValues('table')).toBe('');
    });
  });
});
