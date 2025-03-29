import { dbKeysMock } from '__tests__/_setup/mocks/samples';
import { act, renderHook, waitFor } from '__tests__/_setup/testing-utils';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { IAuthPropertiesStorageKeys } from 'app/util/auth';
import { AuthType } from 'types/types';
import { describe, expect, it } from 'vitest';

describe('useSearchForm', () => {
  it('sets default table key on reset', async () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

    const { result } = renderHook(() => useSearchForm(), { authType: AuthType.Jwt });

    await waitFor(() => {
      result.current.reset();
      expect(result.current.getValues('table')).toBe(dbKeysMock[0]);
    });
  });

  it('sets NULL table key on reset', async () => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

    const { result } = renderHook(() => useSearchForm(), { authType: AuthType.Jwt });

    await waitFor(() => {
      result.current.reset(true);
      expect(result.current.getValues('table')).toBe(null);
    });
  });

  type Properties = Parameters<ReturnType<typeof useSearchForm>['setValue']>['0'];
  it.each([{
    property: 'level' as Properties, resetResult: true,
  },
  {
    property: 'search' as Properties, resetResult: true,
  },
  {
    property: 'startDate' as Properties, resetResult: true,
  },
  {
    property: 'endDate' as Properties, resetResult: true,
  },
  {
    property: 'entriesPerPage' as Properties, resetResult: false
  },
  {
    property: 'page' as Properties, resetResult: false
  },
  {
    property: 'sortBy' as Properties, resetResult: false
  },
  {
    property: 'table' as Properties, resetResult: false
  },
  {
    property: 'sortOn' as Properties, resetResult: false
  }
  ])('hints for refetch, returning $resetResult on reset, for property $property', async ({ property, resetResult }: { property: Properties, resetResult: boolean }) => {
    sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

    const { result } = renderHook(() => useSearchForm(), { authType: AuthType.Jwt });

    act(() => {
      result.current.setValue(property, 'test')
    })

    act(() => {
      const shouldRefetch = result.current.reset();
      expect(shouldRefetch).toBe(resetResult);
    })
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
