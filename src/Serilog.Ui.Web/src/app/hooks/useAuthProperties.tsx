import { Text } from '@mantine/core';
import {
  IAuthPropertiesData,
  clearAuth,
  getAuthorizationHeader,
  initialAuthProps,
  saveAuthKey,
} from 'app/authorization/AuthProperties';
import { sendUnexpectedNotification } from 'app/util/queries';
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

const checkErrors = ({ success, errors }: { success: boolean; errors?: string[] }) => {
  if (!success) {
    sendUnexpectedNotification(
      <Text ta="justify">
        Your authorization data could be invalid, we noticed the following errors:
        <br />
        {errors?.join(', ')}
      </Text>,
      'Auth validation',
      'yellow',
      false,
    );
  }
};

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
    const cleanState = clearAuth();
    setAuthInfo(cleanState);
    setAuthProps(cleanState);
  }, [setAuthInfo, setAuthProps]);

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

    const result = { success: !validationInfo.length, errors: validationInfo };
    checkErrors(result);
    return result;
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
