import { AppShell, ColorSchemeScript, MantineProvider, createTheme } from '@mantine/core';
import '@mantine/core/styles.css';
import { useDisclosure } from '@mantine/hooks';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import AppBody from './components/AppBody';
import Head from './components/ShellStructure/Header';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';

const App = () => {
  const [mobileOpen, { toggle: toggleMobile }] = useDisclosure();
  const queryClient = new QueryClient();
  const theme = createTheme({
    breakpoints: {
      xs: '28em', // 448px
      sm: '36em', // 576px
    },
  });

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider defaultColorScheme="auto" theme={theme}>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <AppShell
            header={{ height: '4.2em' }}
            navbar={{
              breakpoint: 'sm',
              collapsed: { mobile: !mobileOpen, desktop: true },
              width: { sm: 70, md: 150, lg: 220 },
            }}
          >
            <AppShell.Header>
              <AuthPropertiesProvider>
                <Head isMobileOpen={mobileOpen} toggleMobile={toggleMobile} />
              </AuthPropertiesProvider>
            </AppShell.Header>

            <AppShell.Navbar
              // TODO: center all the buttons, correct bg to use dynamic mantine themes, improve all items
              p="md"
              // bg={colorScheme === 'dark' ? theme.colors.blue[7] : theme.colors.blue[4]}
            >
              {/* show sidebar with search options */}
              {/* <Sidebar /> */}
            </AppShell.Navbar>
            <AppShell.Main>
              <AppBody />
            </AppShell.Main>
          </AppShell>
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

export default App;
