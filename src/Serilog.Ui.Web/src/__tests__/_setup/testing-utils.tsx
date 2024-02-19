import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render as testingLibraryRender } from '@testing-library/react';
import { UserEvent, userEvent } from '@testing-library/user-event';
import { AuthPropertiesProvider } from 'app/hooks/useAuthProperties';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { SerilogUiPropsProvider } from 'app/hooks/useSerilogUiProps';
import { ReactNode } from 'react';
import { FormProvider } from 'react-hook-form';
import { theme } from 'style/theme';

export * from '@testing-library/react';
export { userEvent, type UserEvent };

export function render(ui: React.ReactNode) {
  const queryClient = new QueryClient();
  return testingLibraryRender(<>{ui}</>, {
    wrapper: ({ children }: { children: ReactNode }) => {
      return (
        <MantineProvider theme={theme}>
          <QueryClientProvider client={queryClient}>
            <FormWrapper>{children}</FormWrapper>
          </QueryClientProvider>
        </MantineProvider>
      );
    },
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