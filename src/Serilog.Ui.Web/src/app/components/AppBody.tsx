import { Box } from '@mantine/core';
import { lazy } from 'react';
import classes from 'style/table.module.css';
import Paging from './Search/Paging';
import Search from './Search/Search';
import SerilogResults from './Table/SerilogResults';

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
      <Box m="xl">
        <Box visibleFrom="md">
          <SerilogResults />
        </Box>
        <Paging />
      </Box>
    </>
  );
};

export default AppBody;
