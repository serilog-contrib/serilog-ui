import type { IAuthPropertiesData } from 'app/util/auth';
import type { ReactNode } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { AuthPropertiesContext } from 'app/hooks/useAuthProperties';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import {
  checkErrors,
  clearAuth,
  getAuthorizationHeader,
  IAuthPropertiesStorageKeys,
  initialAuthProps,
  saveAuthKey,
} from 'app/util/auth';
import { createRequestInit } from 'app/util/queries';
import { useCallback, useMemo, useState } from 'react';
import { useSearchParams } from 'react-router';
import { AuthType } from '../../types/types.ts';
import { isStringGuard } from '../util/guards.ts';

export const AuthPropertiesProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const queryClient = useQueryClient();
  const { authType, routePrefix } = useSerilogUiProps();

  const [authInfo, setAuthInfo] =
    useState<IAuthPropertiesData>(initialAuthProps);

  const authHeader = useMemo(
    () => getAuthorizationHeader(authInfo, authType),
    [authInfo, authType],
  );
  const isHeaderReady =
    authType === AuthType.Custom || isStringGuard(authHeader);

  const fetchInfo = useMemo(
    () => ({
      headers: createRequestInit(authType, authHeader),
      routePrefix,
    }),
    [authHeader, authType, routePrefix],
  );

  const [, setSearchParams] = useSearchParams();
  const clearAuthState = useCallback(() => {
    const cleanState = clearAuth();

    queryClient.removeQueries({ queryKey: ['get-keys'], exact: false });
    setSearchParams((prev) => {
      prev.delete('table');
      return prev;
    });

    setAuthInfo(cleanState);
  }, [queryClient, setSearchParams]);

  const saveAuthState = useCallback((input: { [key: string]: string }) => {
    const validationInfo: string[] = [];

    const updatedData = Object.keys(input).reduce((acc, value) => {
      if (!Object.keys(IAuthPropertiesStorageKeys).includes(value)) {
        return acc;
      }
      const saveResult = saveAuthKey(
        acc,
        value as keyof IAuthPropertiesData,
        input[value] ?? '',
      );
      if (saveResult.error) {
        validationInfo.push(saveResult.error);
      }
      return acc;
    }, {});

    setAuthInfo((draft) => ({ ...draft, ...updatedData }));

    const result = { success: !validationInfo.length, errors: validationInfo };
    checkErrors(result);
    return result;
  }, []);

  const providerValue = useMemo(
    () => ({
      authProps: authInfo,
      authHeader,
      isHeaderReady,
      fetchInfo,
      saveAuthState,
      clearAuthState,
    }),
    [
      authInfo,
      authHeader,
      isHeaderReady,
      clearAuthState,
      fetchInfo,
      saveAuthState,
    ],
  );

  return (
    <AuthPropertiesContext value={providerValue}>
      {children}
    </AuthPropertiesContext>
  );
};
