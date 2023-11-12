import { AppShell, ColorSchemeScript, MantineProvider } from '@mantine/core';
import '@mantine/core/styles.css';
import { useDisclosure } from '@mantine/hooks';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import AppBody from './components/AppBody';
import Head from './components/ShellStructure/Header';
import Sidebar from './components/ShellStructure/Sidebar';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';

const App = () => {
  const [opened, { toggle }] = useDisclosure();

  const queryClient = new QueryClient();

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider
        defaultColorScheme="auto"
        theme={{
          breakpoints: {
            xs: '28em',
            sm: '36em',
            md: '48em',
            lg: '74em',
            xl: '90em',
          },
        }}
      >
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <AuthPropertiesProvider>
            <AppShell
              header={{ height: { base: 50, sm: 0 } }}
              navbar={{
                breakpoint: 'sm',
                collapsed: { desktop: true, mobile: !opened },
                width: { sm: 70, md: 150, lg: 220 },
              }}
            >
              <Head isOpen={opened} toggleOpen={toggle} />

              <Sidebar />

              <AppBody />
            </AppShell>
          </AuthPropertiesProvider>
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

export default App;
