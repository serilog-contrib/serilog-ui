import {
  IAuthPropertiesData,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/authorization/AuthProperties';
import { createContext, useCallback, useContext, useMemo, type ReactNode } from 'react';
import { useImmer } from 'use-immer';
import { useSerilogUiProps } from './useSerilogUiProps';

interface AuthProps {
  authProps: IAuthPropertiesData;
  authHeader: string;
  clearAuthState: () => void;
  saveAuthState: () => { success: boolean; errors?: string[] };
  updateAuthKey: (key: keyof IAuthPropertiesData, value: string) => void;
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: {},
  authHeader: '',
  clearAuthState: () => {},
  saveAuthState: () => ({
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

  const [authInfo, setAuthInfo] = useImmer<IAuthPropertiesData>({ ...initialAuthProps });
  const [activeAuthProps, setAuthProps] = useImmer<IAuthPropertiesData>(authInfo);
  const authHeader = useMemo(
    () => getAuthorizationHeader(authType, authInfo),
    [authInfo, authType],
  );

  const clearAuthState = useCallback(() => {
    setAuthInfo(initialAuthProps);
  }, [setAuthInfo]);

  const saveAuthState = useCallback(() => {
    const validationInfo: string[] = [];
    setAuthInfo((draft) => {
      Object.keys(initialAuthProps).forEach((e) => {
        const saveResult = saveAuthKey(
          draft,
          e as keyof IAuthPropertiesData,
          activeAuthProps[e],
        );
        if (saveResult.error) {
          validationInfo.push(saveResult.error);
        }
      });

      return draft;
    });

    return { success: !validationInfo.length, errors: validationInfo };
  }, [activeAuthProps, setAuthInfo]);

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
  const {
    authProps,
    clearAuthState,
    authHeader,
    saveAuthState,
    updateAuthKey: updateBearerToken,
  } = useContext(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    clearAuthState,
    saveAuthState,
    updateBearerToken,
  };
};
