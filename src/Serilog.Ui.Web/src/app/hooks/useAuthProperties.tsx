import { useQueryClient } from '@tanstack/react-query';
import {
  checkErrors,
  clearAuth,
  getAuthorizationHeader,
  IAuthPropertiesData,
  IAuthPropertiesStorageKeys,
  initialAuthProps,
  saveAuthKey,
} from 'app/util/auth';
import { createRequestInit } from 'app/util/queries';
import {
  createContext,
  type ReactNode,
  useCallback,
  useContext,
  useMemo,
  useState,
} from 'react';
import { AuthType, DispatchedCustomEvents } from '../../types/types.ts';
import { isStringGuard } from '../util/guards.ts';
import { useSerilogUiProps } from './useSerilogUiProps';

interface AuthProps {
  authProps: IAuthPropertiesData;
  authHeader: string;
  isHeaderReady?: boolean;
  fetchInfo: {
    headers: RequestInit;
    routePrefix?: string;
  };
  clearAuthState: () => void;
  saveAuthState: (authKeysToSave: { [key: string]: string }) => {
    success: boolean;
    errors?: string[];
  };
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: {},
  authHeader: '',
  isHeaderReady: false,
  fetchInfo: {
    headers: {},
  },
  clearAuthState: () => {},
  saveAuthState: () => ({
    success: true,
  }),
});

export const AuthPropertiesProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const queryClient = useQueryClient();
  const { authType, routePrefix } = useSerilogUiProps();

  const [authInfo, setAuthInfo] = useState<IAuthPropertiesData>({
    ...initialAuthProps(),
  });

  const authHeader = useMemo(
    () => getAuthorizationHeader(authInfo, authType),
    [authInfo, authType],
  );
  const isHeaderReady = authType === AuthType.Custom || isStringGuard(authHeader);

  const fetchInfo = useMemo(
    () => ({
      headers: createRequestInit(authType, authHeader),
      routePrefix,
    }),
    [authHeader, authType, routePrefix],
  );

  const clearAuthState = useCallback(() => {
    const cleanState = clearAuth();

    queryClient.removeQueries({ queryKey: ['get-keys'], exact: false });
    document.dispatchEvent(new CustomEvent(DispatchedCustomEvents.RemoveTableKey));

    setAuthInfo(cleanState);
  }, [queryClient]);

  const saveAuthState = useCallback((input: { [key: string]: string }) => {
    const validationInfo: string[] = [];

    const updatedData = Object.keys(input).reduce((acc, value) => {
      if (!Object.keys(IAuthPropertiesStorageKeys).includes(value)) return acc;
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
    [authInfo, authHeader, isHeaderReady, clearAuthState, fetchInfo, saveAuthState],
  );

  return (
    <AuthPropertiesContext.Provider value={providerValue}>
      {children}
    </AuthPropertiesContext.Provider>
  );
};

export const useAuthProperties = () => {
  const {
    authProps,
    authHeader,
    fetchInfo,
    isHeaderReady,
    clearAuthState,
    saveAuthState,
  } = useContext(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    isHeaderReady,
    clearAuthState,
    fetchInfo,
    saveAuthState,
  };
};
