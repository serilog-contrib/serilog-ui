import loadable from '@loadable/component';
import {
  AppShell,
  AppShellHeaderConfiguration,
  AppShellNavbarConfiguration,
  ColorSchemeScript,
  MantineProvider,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { Notifications } from '@mantine/notifications';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Suspense } from 'react';
import { FormProvider } from 'react-hook-form';
import { Navigate, Outlet, RouterProvider, createBrowserRouter } from 'react-router-dom';
import { theme } from 'style/theme';
import { ErrorBoundaryPage } from './components/ErrorPage';
import { HomePageNotAuthorized } from './components/HomePageNotAuthorized';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';
import { useCloseOnResize } from './hooks/useCloseOnResize';
import { useSearchForm } from './hooks/useSearchForm';
import { SerilogUiPropsProvider, useSerilogUiProps } from './hooks/useSerilogUiProps';

const AppBody = loadable(() => import('./components/AppBody'));
const Head = loadable(() => import('./components/ShellStructure/Header'));
const Sidebar = loadable(() => import('./components/ShellStructure/Sidebar'));

const App = () => {
  const queryClient = new QueryClient();

  const router = createBrowserRouter([
    {
      element: <Layout />,
      path: '/',
      ErrorBoundary: ErrorBoundaryPage,
      children: [
        {
          element: <Shell />,
          index: true,
        },
        {
          element: <HomePageNotAuthorized />,
          path: 'access-denied',
        },
      ],
    },
  ]);

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider defaultColorScheme="auto" theme={theme}>
        <Notifications />
        <QueryClientProvider client={queryClient}>
          <RouterProvider router={router} />
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

const Layout = () => {
  const { methods } = useSearchForm();

  return (
    // eslint-disable-next-line react/jsx-props-no-spreading
    <FormProvider {...methods}>
      <SerilogUiPropsProvider>
        <AuthPropertiesProvider>
          <Outlet />
        </AuthPropertiesProvider>
      </SerilogUiPropsProvider>
    </FormProvider>
  );
};

const Shell = () => {
  const { isHomeUnauthorized } = useSerilogUiProps();
  const [mobileOpen, { toggle: toggleMobile, close }] = useDisclosure();

  const headerProps: AppShellHeaderConfiguration = { height: '4.3em' };
  const navbarProps: AppShellNavbarConfiguration = {
    breakpoint: 'sm',
    collapsed: { mobile: !mobileOpen, desktop: true },
    width: 70,
  };

  useCloseOnResize(close);

  if (isHomeUnauthorized) return <Navigate to="access-denied" replace />;

  return (
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
  );
};

export default App;
