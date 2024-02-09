/* eslint-disable react/jsx-props-no-spreading */
import { AppShell, ColorSchemeScript, MantineProvider, createTheme } from '@mantine/core';
import '@mantine/core/styles.css';
import { useDisclosure } from '@mantine/hooks';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { FormProvider } from 'react-hook-form';
import AppBody from './components/AppBody';
import Head from './components/ShellStructure/Header';
import Sidebar from './components/ShellStructure/Sidebar';
import { useSearchForm } from './hooks/SearchFormContext';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';

const App = () => {
  const queryClient = new QueryClient();
  const theme = createTheme({
    breakpoints: {
      xs: '28em', // 448px
      sm: '36em', // 576px
    },
    fontFamily: 'Arial, Helvetica, sans-serif', // todo nice font
  });

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider defaultColorScheme="auto" theme={theme}>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <Shell />
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

const Shell = () => {
  const [mobileOpen, { toggle: toggleMobile }] = useDisclosure();
  const { methods } = useSearchForm();

  return (
    <FormProvider {...methods}>
      <AppShell
        header={{ height: '4em' }}
        navbar={{
          breakpoint: 'sm',
          collapsed: { mobile: !mobileOpen, desktop: true },
          width: 70,
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
        >
          <Sidebar />
        </AppShell.Navbar>
        <AppShell.Main>
          <AppBody />
        </AppShell.Main>
      </AppShell>
    </FormProvider>
  );
};

export default App;
