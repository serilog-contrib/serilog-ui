import {
  AppShell,
  ColorScheme,
  ColorSchemeProvider,
  MantineProvider,
} from '@mantine/core';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { useState } from 'react';
import AppBody from './components/AppBody';
import Head from './components/ShellStructure/Header';
import Sidebar from './components/ShellStructure/Sidebar';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';

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
