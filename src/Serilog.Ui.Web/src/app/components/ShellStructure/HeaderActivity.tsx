import loadable from '@loadable/component';
import { Box, Burger, useMantineTheme } from '@mantine/core';
import { ComponentProps } from 'react';

const Head = loadable(() => import('./Header'));
const AuthorizeButton = loadable(() => import('../Authorization/AuthorizeButton'));
const FilterButton = loadable(() => import('./FilterButton'));

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
        <AuthorizeButton />
      </Box>

      <Box visibleFrom="sm" hiddenFrom="lg">
        <FilterButton />
      </Box>
    </>
  );
};

export default HeaderActivity;
