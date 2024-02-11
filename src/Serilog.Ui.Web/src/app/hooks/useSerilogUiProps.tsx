import { ReactNode, createContext, useContext, useEffect, useState } from 'react';

export interface SerilogUiConfig {
  authType?: string;
  routePrefix?: string;
  homeUrl?: string;
}

const SerilogUiPropsContext = createContext<SerilogUiConfig>({});

export const SerilogUiPropsProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const [serverProps, setServerProps] = useState<SerilogUiConfig>({});

  useEffect(() => {
    const config = '%(Configs)';
    try {
      const decodedConfig = decodeURIComponent(config);
      const configObject = JSON.parse(decodedConfig);
      setServerProps(configObject);
    } catch (e) {
      console.warn('SerilogUI Config not received correctly! Using defaults');
      setServerProps({
        routePrefix: 'serilog-ui',
        authType: 'Jwt',
        homeUrl: 'https://google.com',
      });
    }
  }, []);

  return (
    <SerilogUiPropsContext.Provider value={serverProps}>
      {children}
    </SerilogUiPropsContext.Provider>
  );
};

export const useSerilogUiProps = () => {
  const authProps = useContext(SerilogUiPropsContext);

  return authProps;
};
