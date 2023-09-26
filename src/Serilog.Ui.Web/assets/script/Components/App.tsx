import {
  type ColorScheme,
  ColorSchemeProvider,
  MantineProvider,
  AppShell,
} from '@mantine/core';
import { useState } from 'react';
import Sidebar from './ShellStructure/Sidebar';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query/';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import AppBody from './AppBody';
import Head from './ShellStructure/Header';
import { AuthPropertiesProvider } from '../Hooks/useAuthProperties';

const App = () => {
  const [colorScheme, setColorScheme] = useState<ColorScheme>('light');
  const toggleColorScheme = (value?: ColorScheme) => {
    setColorScheme(value ?? (colorScheme === 'dark' ? 'light' : 'dark'));
  };

  const queryClient = new QueryClient();
  const [opened, setOpened] = useState(false);

  return (
    <ColorSchemeProvider colorScheme={colorScheme} toggleColorScheme={toggleColorScheme}>
      <MantineProvider
        theme={{
          colorScheme,
          breakpoints: {
            xs: '28em',
            sm: '36em',
            md: '48em',
            lg: '74em',
            xl: '90em',
          },
        }}
        withGlobalStyles
        withNormalizeCSS
      >
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <AuthPropertiesProvider>
            <AppShell
              navbar={<Sidebar isOpen={opened} />}
              header={<Head isOpen={opened} toggleOpen={setOpened} />}
            >
              <AppBody />
            </AppShell>
          </AuthPropertiesProvider>
        </QueryClientProvider>
      </MantineProvider>
    </ColorSchemeProvider>
  );
};

export default App;
