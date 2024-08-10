import {
  IAuthPropertiesData,
  IAuthPropertiesStorageKeys,
  checkErrors,
  clearAuth,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/util/auth';
import { createRequestInit } from 'app/util/queries';
import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import { useSerilogUiProps } from './useSerilogUiProps';

interface AuthProps {
  authProps: IAuthPropertiesData;
  authHeader: string;
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
  const { authType, routePrefix } = useSerilogUiProps();

  const [authInfo, setAuthInfo] = useState<IAuthPropertiesData>({
    ...initialAuthProps(),
  });

  const authHeader = useMemo(
    () => getAuthorizationHeader(authInfo, authType),
    [authInfo, authType],
  );

  const fetchInfo = useMemo(
    () => ({
      headers: createRequestInit(authType, authHeader),
      routePrefix,
    }),
    [authHeader, authType, routePrefix],
  );

  const clearAuthState = useCallback(() => {
    const cleanState = clearAuth();
    setAuthInfo(cleanState);
  }, []);

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
      fetchInfo,
      saveAuthState,
      clearAuthState,
    }),
    [authInfo, authHeader, clearAuthState, fetchInfo, saveAuthState],
  );

  return (
    <AuthPropertiesContext.Provider value={providerValue}>
      {children}
    </AuthPropertiesContext.Provider>
  );
};

export const useAuthProperties = () => {
  const { authProps, authHeader, fetchInfo, clearAuthState, saveAuthState } =
    useContext(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    clearAuthState,
    fetchInfo,
    saveAuthState,
  };
};
