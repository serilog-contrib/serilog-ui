import { Box } from '@mantine/core';
import classes from 'style/table.module.css';
import Paging from './Search/Paging';
import Search from './Search/Search';
import SerilogResults from './Table/SerilogResults';
import { SerilogResultsMobile } from './Table/SerilogResultsMobile';

const AppBody = () => {
  return (
    <>
      <Box visibleFrom="lg">
        <Search />
      </Box>
      <Box hiddenFrom="md" className={classes.mobileTableWrapper}>
        <SerilogResultsMobile />
      </Box>
      <Box visibleFrom="md">
        <SerilogResults />
      </Box>
      <Paging />
    </>
  );
};

export default AppBody;
