import type { ReactNode } from 'react';
import type { SerilogUiConfig } from 'types/types';
import { SerilogUiPropsContext } from 'app/hooks/useSerilogUiProps';
import { useMemo, useState } from 'react';
import { defaultAuthType } from 'types/types';

const defaults: SerilogUiConfig = {
  authType: defaultAuthType,
  columnsInfo: {},
  disabledSortOnKeys: [],
  renderExceptionAsStringKeys: [],
  homeUrl: 'https://google.com',
  routePrefix: 'serilog-ui',
  showBrand: true,
};

const readServerDataOnRender = () => {
  const config = document.getElementById('serilog-ui-props')?.textContent;

  if (config) {
    try {
      const decodedConfig = decodeURIComponent(config);
      const configObject = JSON.parse(decodedConfig);

      return configObject;
    } catch {
      console.warn('SerilogUI Config not received correctly! Using defaults');
    }
  }

  return defaults;
};

export const SerilogUiPropsProvider = ({
  children,
}: {
  children: ReactNode | undefined;
}) => {
  const serverProps = useMemo(() => readServerDataOnRender(), []);
  const [isUtc, setIsUtc] = useState<boolean>(false);
  const [authenticatedFromAccessDenied, setAuthenticatedFromAccessDenied] =
    useState<boolean>(false);

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
    <SerilogUiPropsContext value={providerValue}>
      {children}
    </SerilogUiPropsContext>
  );
};
