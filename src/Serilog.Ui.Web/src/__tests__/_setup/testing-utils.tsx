import { MantineProvider } from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render as testingLibraryRender } from '@testing-library/react';
import { UserEvent, userEvent } from '@testing-library/user-event';
// Import your theme object
import { theme } from 'style/theme';

export * from '@testing-library/react';
export { userEvent, type UserEvent };

export function render(ui: React.ReactNode) {
  const queryClient = new QueryClient();

  return testingLibraryRender(<>{ui}</>, {
    wrapper: ({ children }: { children: React.ReactNode }) => (
      <QueryClientProvider client={queryClient}>
        <MantineProvider theme={theme}>{children}</MantineProvider>
      </QueryClientProvider>
    ),
  });
}
