import {
  IAuthPropertiesData,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/authorization/AuthProperties';
import { createContext, useContext, type ReactNode } from 'react';
import { useImmer } from 'use-immer';

interface AuthProps {
  authProps: IAuthPropertiesData;
  clearAuthState: () => void;
  getAuthHeader: () => string | undefined;
  saveAuthState: () => { success: boolean; errors?: string[] };
  updateAuthKey: (key: keyof IAuthPropertiesData, value: string) => void;
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: {},
  clearAuthState: () => {},
  getAuthHeader: () => '',
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
  const [authInfo, setAuthInfo] = useImmer<IAuthPropertiesData>({ ...initialAuthProps });
  const [activeAuthProps, setAuthProps] = useImmer<IAuthPropertiesData>(authInfo);

  const updateAuthKey = (key: keyof IAuthPropertiesData, value: string) => {
    setAuthProps((draft) => {
      draft[key] = value;
    });
  };

  const getAuthHeader = () => getAuthorizationHeader(authInfo);

  const saveAuthState = () => {
    const result = setAuthInfo((draft) => {
      Object.keys(initialAuthProps).forEach((e) =>
        saveAuthKey(draft, e as keyof IAuthPropertiesData, activeAuthProps[e]),
      );
      return draft;
    });

    return result as unknown as { success: true };
  };

  const clearAuthState = () => {
    setAuthInfo({});
  };

  return (
    <AuthPropertiesContext.Provider
      value={{
        authProps: activeAuthProps,
        getAuthHeader,
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
    getAuthHeader,
    saveAuthState,
    updateAuthKey: updateBearerToken,
  } = useContext(AuthPropertiesContext);
  return {
    ...authProps,
    getAuthHeader,
    clearAuthState,
    saveAuthState,
    updateBearerToken,
  };
};
