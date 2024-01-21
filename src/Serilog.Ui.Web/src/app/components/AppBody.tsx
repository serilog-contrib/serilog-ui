import { Box } from '@mantine/core';
import Paging from './Search/Paging';
import Search from './Search/Search';
import SerilogResults from './Table/Table';

const AppBody = () => {
  return (
    <>
      <Box visibleFrom="lg">
        <Search />
      </Box>
      <SerilogResults />
      <Paging />
    </>
  );
};

export default AppBody;
