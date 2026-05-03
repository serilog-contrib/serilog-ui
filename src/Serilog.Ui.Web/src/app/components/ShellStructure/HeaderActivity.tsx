import type { ComponentProps } from 'react';
import type Head from './Header';
import { Box, Burger, useMantineTheme } from '@mantine/core';
import { lazy, Suspense } from 'react';

const AuthorizeButton = lazy(() => import('../Authorization/AuthorizeButton'));
const FilterButton = lazy(() => import('./FilterButton'));

const HeaderActivity = (props: ComponentProps<typeof Head>) => {
  const theme = useMantineTheme();

  return (
    <>
      <Burger
        opened={props.isMobileOpen}
        onClick={props.toggleMobile}
        hiddenFrom="sm"
        size="sm"
        color={theme.colors.gray[6]}
      />

      <Box visibleFrom="sm">
        <Suspense>
          <AuthorizeButton />
        </Suspense>
      </Box>

      <Box visibleFrom="sm" hiddenFrom="lg">
        <Suspense>
          <FilterButton />
        </Suspense>
      </Box>
    </>
  );
};

export default HeaderActivity;
