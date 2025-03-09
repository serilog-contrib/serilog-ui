import {
  AppShell,
  AppShellHeaderConfiguration,
  AppShellNavbarConfiguration,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { useCloseOnResize } from 'app/hooks/useCloseOnResize';
import { useQueryAuth } from 'app/hooks/useQueryAuth';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { Suspense, lazy } from 'react';
import { Navigate } from 'react-router';

const AppBody = lazy(() => import('./AppBody'));
const Head = lazy(() => import('./ShellStructure/Header'));
const Sidebar = lazy(() => import('./ShellStructure/Sidebar'));

export const Index = () => {
  const { blockHomeAccess, authenticatedFromAccessDenied } = useSerilogUiProps();

  useQueryAuth();
  const [mobileOpen, { toggle: toggleMobile, close }] = useDisclosure();

  const headerProps: AppShellHeaderConfiguration = { height: '4.3em' };
  const navbarProps: AppShellNavbarConfiguration = {
    breakpoint: 'sm',
    collapsed: { mobile: !mobileOpen, desktop: true },
    width: 70,
  };

  useCloseOnResize(close);

  if (blockHomeAccess && !authenticatedFromAccessDenied)
    return <Navigate to={`access-denied`} replace />;

  return (
    <AppShell aria-label="main-app" header={headerProps} navbar={navbarProps}>
      <AppShell.Header>
        <Suspense fallback={<div />}>
          <Head isMobileOpen={mobileOpen} toggleMobile={toggleMobile} />
        </Suspense>
      </AppShell.Header>

      <AppShell.Navbar p="sm">
        <Suspense fallback={<div />}>
          <Sidebar />
        </Suspense>
      </AppShell.Navbar>

      <AppShell.Main>
        <Suspense fallback={<div />}>
          <AppBody hideMobileResults={mobileOpen} />
        </Suspense>
      </AppShell.Main>
    </AppShell>
  );
};
