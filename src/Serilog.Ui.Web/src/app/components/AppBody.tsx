import { Box } from '@mantine/core';
import { lazy } from 'react';
import classes from 'style/table.module.css';

const Search = lazy(() => import('./Search/Search'));
const Paging = lazy(() => import('./Search/Paging'));
const SerilogResults = lazy(() => import('./Table/SerilogResults'));
const SerilogResultsMobile = lazy(() => import('./Table/SerilogResultsMobile'));

const AppBody = () => {
  return (
    <>
      <Box visibleFrom="lg">
        <Search />
      </Box>
      <Box hiddenFrom="md" className={classes.mobileTableWrapper}>
        <SerilogResultsMobile />
      </Box>
      <Box>
        <Box visibleFrom="md" m="xl">
          <SerilogResults />
        </Box>
        <Paging />
      </Box>
    </>
  );
};

export default AppBody;
