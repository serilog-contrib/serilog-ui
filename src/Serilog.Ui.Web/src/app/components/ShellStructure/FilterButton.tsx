import { ActionIcon, Box, Button, Modal, Text } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconEraser, IconFilterSearch } from '@tabler/icons-react';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { lazy, useEffect } from 'react';
import classes from 'style/header.module.css';

const Search = lazy(() => import('../Search/Search'));

export const FilterButton = () => {
  const [filterModalOpened, { open, close }] = useDisclosure(false);
  const { reset } = useSearchForm();

  useEffect(() => {
    window.addEventListener('resize', close);

    return () => window.removeEventListener('resize', close);
  }, [close]);
  return (
    <>
      <Button size="compact-md" onClick={open}>
        <IconFilterSearch />
        Filter
      </Button>
      <Modal
        opened={filterModalOpened}
        onClose={close}
        title={
          <Box className={classes.searchFiltersModalTitle}>
            <Text>Search filters</Text>
            <ActionIcon
              onClick={() => {
                reset();
              }}
              size={28}
              variant="light"
              aria-label="reset filters"
            >
              <IconEraser />
            </ActionIcon>
          </Box>
        }
        centered
        size="lg"
      >
        <Search />
      </Modal>
    </>
  );
};
