import {
  IAuthPropertiesData,
  checkErrors,
  clearAuth,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/util/auth';
import { createContext, useCallback, useContext, useMemo, type ReactNode } from 'react';
import { useImmer } from 'use-immer';
import { useSerilogUiProps } from './useSerilogUiProps';

interface AuthProps {
  authProps: IAuthPropertiesData;
  authHeader: string;
  clearAuthState: () => void;
  saveAuthState: (authKeysToSave: (keyof IAuthPropertiesData)[]) => {
    success: boolean;
    errors?: string[];
  };
  updateAuthKey: (key: keyof IAuthPropertiesData, value: string) => void;
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: {},
  authHeader: '',
  clearAuthState: () => {},
  saveAuthState: ([]) => ({
    success: true,
  }),
  updateAuthKey: (_, __) => {
    console.log(_, __);
  },
});

export const AuthPropertiesProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const { authType } = useSerilogUiProps();

  const [authInfo, setAuthInfo] = useImmer<IAuthPropertiesData>({
    ...initialAuthProps(),
  });
  const [activeAuthProps, setAuthProps] = useImmer<IAuthPropertiesData>(authInfo);

  const authHeader = useMemo(
    () => getAuthorizationHeader(authType, authInfo),
    [authInfo, authType],
  );

  const clearAuthState = useCallback(() => {
    const cleanState = clearAuth();
    setAuthInfo(cleanState);
    setAuthProps(cleanState);
  }, [setAuthInfo, setAuthProps]);

  const saveAuthState = useCallback(
    (authKeysToSave: (keyof IAuthPropertiesData)[]) => {
      const validationInfo: string[] = [];
      setAuthInfo((draft) => {
        authKeysToSave.forEach((e) => {
          const saveResult = saveAuthKey(
            draft,
            e as keyof IAuthPropertiesData,
            activeAuthProps[e] || '',
          );
          if (saveResult.error) {
            validationInfo.push(saveResult.error);
          }
        });
        return draft;
      });

      const result = { success: !validationInfo.length, errors: validationInfo };
      checkErrors(result);
      return result;
    },
    [activeAuthProps, setAuthInfo],
  );

  const updateAuthKey = (key: keyof IAuthPropertiesData, value: string) => {
    setAuthProps((draft) => {
      draft[key] = value;
    });
  };

  return (
    <AuthPropertiesContext.Provider
      value={{
        authProps: activeAuthProps,
        authHeader,
        updateAuthKey,
        saveAuthState,
        clearAuthState,
      }}
    >
      {children}
    </AuthPropertiesContext.Provider>
  );
};

export const useAuthProperties = () => {
  const { authProps, clearAuthState, authHeader, saveAuthState, updateAuthKey } =
    useContext(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    clearAuthState,
    saveAuthState,
    updateAuthKey,
  };
};
