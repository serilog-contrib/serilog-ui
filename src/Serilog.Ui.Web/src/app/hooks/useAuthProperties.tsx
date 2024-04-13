import {
  IAuthPropertiesData,
  IAuthPropertiesStorageKeys,
  checkErrors,
  clearAuth,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/util/auth';
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
  clearAuthState: () => void;
  saveAuthState: (authKeysToSave: { [key: string]: string }) => {
    success: boolean;
    errors?: string[];
  };
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: {},
  authHeader: '',
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
  const { authType } = useSerilogUiProps();

  const [authInfo, setAuthInfo] = useState<IAuthPropertiesData>({
    ...initialAuthProps(),
  });

  const authHeader = useMemo(
    () => getAuthorizationHeader(authInfo, authType),
    [authInfo, authType],
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
      saveAuthState,
      clearAuthState,
    }),
    [authHeader, authInfo, clearAuthState, saveAuthState],
  );

  return (
    <AuthPropertiesContext.Provider value={providerValue}>
      {children}
    </AuthPropertiesContext.Provider>
  );
};

export const useAuthProperties = () => {
  const { authProps, clearAuthState, authHeader, saveAuthState } =
    useContext(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    clearAuthState,
    saveAuthState,
  };
};
