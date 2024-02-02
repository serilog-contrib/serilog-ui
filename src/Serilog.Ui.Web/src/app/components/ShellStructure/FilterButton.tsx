import { ActionIcon, Box, Button, Modal, Text } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconEraser, IconFilterSearch } from '@tabler/icons-react';
import { useSearchFormContext } from 'app/hooks/SearchFormContext';
import { useEffect } from 'react';
import classes from 'style/header.module.css';
import { SearchGroup } from '../Search/SearchGroup';

export const FilterButton = () => {
  const [filterModalOpened, { open, close }] = useDisclosure(false);
  const { reset } = useSearchFormContext();

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
              onClick={reset}
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
        <SearchGroup showSearch />
      </Modal>
    </>
  );
};
