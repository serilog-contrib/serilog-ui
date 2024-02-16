import { Box } from '@mantine/core';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { memo } from 'react';
import { useController } from 'react-hook-form';
import classes from 'style/search.module.css';
import { PagingLeftColumn } from './PagingLeftColumn';
import { PagingRightColumn } from './PagingRightColumn';

const Paging = () => {
  const { control } = useSearchForm();

  const { field } = useController({ ...control, name: 'page' });

  return (
    <Box className={classes.pagingGrid} m="xl">
      <PagingLeftColumn changePage={field.onChange} />
      <PagingRightColumn field={field} />
    </Box>
  );
};

export default memo(Paging);
