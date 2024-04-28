import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {
  render as testingLibraryRender,
  renderHook as testingLibraryRenderHook,
} from '@testing-library/react';
import { UserEvent, userEvent } from '@testing-library/user-event';
import { AuthPropertiesProvider } from 'app/hooks/useAuthProperties';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { SerilogUiPropsProvider } from 'app/hooks/useSerilogUiProps';
import { ReactNode } from 'react';
import { FormProvider } from 'react-hook-form';
import { theme } from 'style/theme';
import { AuthType, ColumnsInfo } from 'types/types';

export * from '@testing-library/react';
export { userEvent, type UserEvent };

const Wrapper = ({
  children,
  authType,
  columnsInfo,
}: {
  children: ReactNode;
  authType: AuthType;
  columnsInfo: ColumnsInfo;
}) => {
  const queryClient = new QueryClient();

  return (
    <MantineProvider theme={theme}>
      <div hidden id="serilog-ui-props">
        {JSON.stringify({
          routePrefix: 'test-serilog-ui',
          authType: authType,
          homeUrl: 'https://test-google.com',
          columnsInfo,
          disabledSortOnKeys: ['disabled-sort-db'],
        })}
      </div>

      <QueryClientProvider client={queryClient}>
        <FormWrapper>{children}</FormWrapper>
      </QueryClientProvider>
    </MantineProvider>
  );
};

export function render(
  ui: React.ReactNode,
  authType = AuthType.Jwt,
  columnsInfo?: ColumnsInfo,
) {
  return testingLibraryRender(<>{ui}</>, {
    wrapper: ({ children }: { children: ReactNode }) => (
      <Wrapper authType={authType} columnsInfo={columnsInfo ?? {}}>
        {children}
      </Wrapper>
    ),
  });
}

const FormWrapper = ({ children }: { children: ReactNode }) => {
  const { methods } = useSearchForm();
  return (
    // eslint-disable-next-line react/jsx-props-no-spreading
    <FormProvider {...methods}>
      <SerilogUiPropsProvider>
        <AuthPropertiesProvider>{children}</AuthPropertiesProvider>
      </SerilogUiPropsProvider>
    </FormProvider>
  );
};

interface RenderHookConfig<T> {
  initialProps?: T;
  authType?: AuthType;
  columnsInfo?: ColumnsInfo;
}

export const renderHook = <T, T1>(
  hook: (initialProps: T1) => T,
  config?: RenderHookConfig<T1>,
) => {
  return testingLibraryRenderHook(hook, {
    wrapper: ({ children }: { children: ReactNode }) => (
      <Wrapper
        authType={config?.authType ?? AuthType.Basic}
        columnsInfo={config?.columnsInfo ?? {}}
      >
        {children}
      </Wrapper>
    ),
    initialProps: config?.initialProps,
  });
};
