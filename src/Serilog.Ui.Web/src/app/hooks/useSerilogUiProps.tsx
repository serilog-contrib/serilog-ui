import { fakeColumnsInfo } from '__tests__/_setup/mocks/samples';
import {
  ReactNode,
  createContext,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react';
import { AuthType, SerilogUiConfig } from 'types/types';

interface SerilogUiProps extends SerilogUiConfig {
  isUtc: boolean;
  setIsUtc: (value: boolean) => void;
}

const defaults: SerilogUiConfig = {
  authType: AuthType.Jwt,
  columnsInfo: ['development', 'test'].includes(import.meta.env.MODE)
    ? fakeColumnsInfo
    : {},
  disabledSortOnKeys: [],
  homeUrl: 'https://google.com',
  routePrefix: 'serilog-ui',
};

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
