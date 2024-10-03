import { createContext, ReactNode, useContext, useEffect, useMemo, useState } from 'react';
import { AuthType, SerilogUiConfig } from 'types/types';

interface SerilogUiProps extends SerilogUiConfig {
  isUtc: boolean;
  setIsUtc: (value: boolean) => void;
  authenticatedFromAccessDenied?: boolean;
  setAuthenticatedFromAccessDenied: (value: boolean) => void;
}

export const defaultAuthType: AuthType = AuthType.Jwt;
const defaults: SerilogUiConfig = {
  authType: defaultAuthType,
  columnsInfo: {},
  disabledSortOnKeys: [],
  renderExceptionAsStringKeys: [],
  homeUrl: 'https://google.com',
  routePrefix: 'serilog-ui',
  showBrand: true,
};

const SerilogUiPropsContext = createContext<SerilogUiProps>({
  isUtc: false,
  setIsUtc: () => {},
  authenticatedFromAccessDenied: false,
  setAuthenticatedFromAccessDenied: () => {},
});

export const SerilogUiPropsProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const [serverProps, setServerProps] = useState<SerilogUiConfig>({});
  const [isUtc, setIsUtc] = useState<boolean>(false);
  const [authenticatedFromAccessDenied, setAuthenticatedFromAccessDenied] =
    useState<boolean>(false);

  useEffect(() => {
    const config = document.getElementById('serilog-ui-props')?.innerText;

    if (config) {
      try {
        const decodedConfig = decodeURIComponent(config);
        const configObject = JSON.parse(decodedConfig);
        setServerProps(configObject);
        return;
      } catch (e) {
        console.warn('SerilogUI Config not received correctly! Using defaults');
      }
    }

    setServerProps(defaults);
  }, []);

  const providerValue = useMemo(
    () => ({
      ...serverProps,
      authenticatedFromAccessDenied,
      isUtc,
      setIsUtc,
      setAuthenticatedFromAccessDenied,
    }),
    [authenticatedFromAccessDenied, isUtc, serverProps],
  );

  return (
    <SerilogUiPropsContext.Provider value={providerValue}>
      {children}
    </SerilogUiPropsContext.Provider>
  );
};

export const useSerilogUiProps = () => {
  const authProps = useContext(SerilogUiPropsContext);

  return authProps;
};
