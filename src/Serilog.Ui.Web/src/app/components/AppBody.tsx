import loadable from '@loadable/component';
import { Box } from '@mantine/core';
import { useJwtTimeout } from 'app/hooks/useJwtTimeout';
import classes from 'style/table.module.css';

const Search = loadable(() => import('./Search/Search'));
const Paging = loadable(() => import('./Search/Paging'));
const SerilogResults = loadable(() => import('./Table/SerilogResults'));
const SerilogResultsMobile = loadable(() => import('./Table/SerilogResultsMobile'));

const AppBody = ({ hideMobileResults }: { hideMobileResults: boolean }) => {
  useJwtTimeout();

  return (
    <>
      <Box visibleFrom="lg">
        <Search />
      </Box>
      <Box
        display={hideMobileResults ? 'none' : 'block'}
        hiddenFrom="md"
        className={classes.mobileTableWrapper}
      >
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
