import { Box, Burger, useMantineTheme } from '@mantine/core';
import { ComponentProps } from 'react';
import AuthorizeButton from '../Authorization/AuthorizeButton';
import { FilterButton } from './FilterButton';
import Head from './Header';

export const HeaderActivity = (props: ComponentProps<typeof Head>) => {
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
