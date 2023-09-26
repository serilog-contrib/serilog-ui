import { createContext, useContext, type ReactNode } from 'react';
import { useImmer } from 'use-immer';
import { AuthProperties } from '../Authorization/AuthProperties';

interface AuthProps {
  authProps: AuthProperties;
  updateBearerToken: (bearerToken: string) => void;
}

const AuthPropertiesContext = createContext<AuthProps>({
  authProps: new AuthProperties(),
  updateBearerToken(_) {},
});

export const AuthPropertiesProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const baseProps = new AuthProperties();
  const [authProps, setAuthProps] = useImmer(baseProps);

  const setToken = (bearerToken: string) => {
    setAuthProps((draft) => {
      draft.bearerToken = bearerToken;
    });
  };

  return (
    <AuthPropertiesContext.Provider value={{ authProps, updateBearerToken: setToken }}>
      {children}
    </AuthPropertiesContext.Provider>
  );
};

export const useAuthProperties = () => {
  const { authProps, updateBearerToken } = useContext(AuthPropertiesContext);
  return { authProps, updateBearerToken };
};
