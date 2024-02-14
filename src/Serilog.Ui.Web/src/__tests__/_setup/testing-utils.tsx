import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render as testingLibraryRender } from '@testing-library/react';
import { UserEvent, userEvent } from '@testing-library/user-event';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { FormProvider } from 'react-hook-form';
// Import your theme object
import { theme } from 'style/theme';

export * from '@testing-library/react';
export { userEvent, type UserEvent };

export function render(ui: React.ReactNode) {
  return testingLibraryRender(<>{ui}</>, {
    wrapper: ({ children }: { children: React.ReactNode }) => {
      const queryClient = new QueryClient();
      const { methods } = useSearchForm();
      return (
        <QueryClientProvider client={queryClient}>
          <MantineProvider theme={theme}>
            <FormProvider {...methods}>{children}</FormProvider>
          </MantineProvider>
        </QueryClientProvider>
      );
    },
  });
}
