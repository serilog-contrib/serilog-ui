import { Box } from '@mantine/core';
import { memo } from 'react';
import classes from 'style/search.module.css';
import { PagingLeftColumn } from './PagingLeftColumn';
import { PagingRightColumn } from './PagingRightColumn';

const Paging = () => (
  <Box className={classes.pagingGrid} m="xl">
    <PagingLeftColumn />
    <PagingRightColumn />
  </Box>
);

export default memo(Paging);
