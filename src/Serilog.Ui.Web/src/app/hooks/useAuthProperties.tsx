import type { IAuthPropertiesData } from 'app/util/auth';
import { createContext, use } from 'react';

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

export const AuthPropertiesContext = createContext<AuthProps>({
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

export const useAuthProperties = () => {
  const {
    authProps,
    authHeader,
    fetchInfo,
    isHeaderReady,
    clearAuthState,
    saveAuthState,
  } = use(AuthPropertiesContext);

  return {
    ...authProps,
    authHeader,
    isHeaderReady,
    clearAuthState,
    fetchInfo,
    saveAuthState,
  };
};
