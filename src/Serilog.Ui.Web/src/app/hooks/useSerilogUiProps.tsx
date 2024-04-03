import {
  ReactNode,
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react';
import { AuthType } from 'types/types';

export interface SerilogUiConfig {
  authType?: AuthType;
  routePrefix?: string;
  homeUrl?: string;
}
interface SerilogUiProps extends SerilogUiConfig {
  isUtc: boolean;
  setIsUtc: (value: boolean) => void;
}

const SerilogUiPropsContext = createContext<SerilogUiProps>({
  isUtc: false,
  setIsUtc: () => {},
});

export const SerilogUiPropsProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const [serverProps, setServerProps] = useState<SerilogUiConfig>({});
  const [isUtc, setIsUtc] = useState<boolean>(false);

  useEffect(() => {
    const setDefaults = () => {
      setServerProps({
        routePrefix: 'serilog-ui',
        authType: AuthType.Jwt,
        homeUrl: 'https://google.com',
      });
    };
    const config = document.getElementById('serilog-ui-props')?.innerText;
    if (!config) {
      setDefaults();
      return;
    }

    try {
      const decodedConfig = decodeURIComponent(config);
      const configObject = JSON.parse(decodedConfig);
      setServerProps(configObject);
    } catch (e) {
      console.warn('SerilogUI Config not received correctly! Using defaults');
      setDefaults();
    }
  }, []);

  const providerValue = useMemo(
    () => ({ ...serverProps, isUtc, setIsUtc }),
    [isUtc, serverProps],
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
