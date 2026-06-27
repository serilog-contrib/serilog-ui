import type { SerilogUiConfig } from 'types/types';
import { createContext, use } from 'react';

interface SerilogUiProps extends SerilogUiConfig {
  isUtc: boolean;
  setIsUtc: (value: boolean) => void;
  authenticatedFromAccessDenied?: boolean;
  setAuthenticatedFromAccessDenied: (value: boolean) => void;
}

export const SerilogUiPropsContext = createContext<SerilogUiProps>({
  isUtc: false,
  setIsUtc: () => {},
  authenticatedFromAccessDenied: false,
  setAuthenticatedFromAccessDenied: () => {},
});

export const useSerilogUiProps = () => {
  const authProps = use(SerilogUiPropsContext);

  return authProps;
};
