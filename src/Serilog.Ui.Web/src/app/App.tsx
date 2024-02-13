import loadable from '@loadable/component';
import { AppShell, ColorSchemeScript, MantineProvider } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { Notifications } from '@mantine/notifications';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Suspense } from 'react';
import { FormProvider } from 'react-hook-form';
import { theme } from 'style/theme';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';
import { useCloseOnResize } from './hooks/useCloseOnResize';
import { useSearchForm } from './hooks/useSearchForm';
import { SerilogUiPropsProvider } from './hooks/useSerilogUiProps';

const AppBody = loadable(() => import('./components/AppBody'));
const Head = loadable(() => import('./components/ShellStructure/Header'));
const Sidebar = loadable(() => import('./components/ShellStructure/Sidebar'));

const App = () => {
  const queryClient = new QueryClient();

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider defaultColorScheme="auto" theme={theme}>
        <Notifications />
        <QueryClientProvider client={queryClient}>
          <Shell />
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

const Shell = () => {
  const { methods } = useSearchForm();
  const [mobileOpen, { toggle: toggleMobile, close }] = useDisclosure();

  const headerProps = { height: '4.3em' };
  const navbarProps = {
    breakpoint: 'sm',
    collapsed: { mobile: !mobileOpen, desktop: true },
    width: 70,
  };

  useCloseOnResize(close);

  return (
    // eslint-disable-next-line react/jsx-props-no-spreading
    <FormProvider {...methods}>
      <SerilogUiPropsProvider>
        <AuthPropertiesProvider>
          <AppShell header={headerProps} navbar={navbarProps}>
            <AppShell.Header>
              <Head isMobileOpen={mobileOpen} toggleMobile={toggleMobile} />
            </AppShell.Header>

            <AppShell.Navbar p="sm">
              <Sidebar />
            </AppShell.Navbar>

            <AppShell.Main>
              <Suspense fallback={<div />}>
                <AppBody hideMobileResults={mobileOpen} />
              </Suspense>
            </AppShell.Main>
          </AppShell>
        </AuthPropertiesProvider>
      </SerilogUiPropsProvider>
    </FormProvider>
  );
};

export default App;
