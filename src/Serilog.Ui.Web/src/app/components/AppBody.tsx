import { Box } from '@mantine/core';
import { useJwtTimeout } from 'app/hooks/useJwtTimeout';
import { Suspense, lazy } from 'react';
import classes from 'style/table.module.css';

const Search = lazy(() => import('./Search/Search'));
const Paging = lazy(() => import('./Search/Paging'));
const SerilogResults = lazy(() => import('./Table/SerilogResults'));
const SerilogResultsMobile = lazy(() => import('./Table/SerilogResultsMobile'));

const AppBody = ({ hideMobileResults }: { hideMobileResults: boolean }) => {
  useJwtTimeout();

  return (
    <>
      <Box visibleFrom="lg">
        <Suspense>
          <Search />
        </Suspense>
      </Box>
      <Box
        display={hideMobileResults ? 'none' : 'block'}
        hiddenFrom="md"
        className={classes.mobileTableWrapper}
      >
        <Suspense>
          <SerilogResultsMobile />
        </Suspense>
      </Box>
      <Box>
        <Box visibleFrom="md" m="xl">
          <Suspense>
            <SerilogResults />
          </Suspense>
        </Box>
        <Suspense>
          <Paging />
        </Suspense>
      </Box>
    </>
  );
};

export default AppBody;
