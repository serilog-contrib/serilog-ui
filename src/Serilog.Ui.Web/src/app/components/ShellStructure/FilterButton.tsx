import { ActionIcon, Box, Button, Modal, Text } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconEraser, IconFilterSearch } from '@tabler/icons-react';
import { useCloseOnResize } from 'app/hooks/useCloseOnResize';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { Suspense, lazy } from 'react';
import classes from 'style/header.module.css';

const Search = lazy(() => import('../Search/Search'));

const FilterButton = () => {
  const [filterModalOpened, { open, close }] = useDisclosure(false);
  const { reset } = useSearchForm();
  const { refetch } = useQueryLogs();
  const onClear = async () => {
    const shouldRefetch = reset();

    if (shouldRefetch) {
      await refetch();
    }
  };

  useCloseOnResize(close);

  return (
    <>
      <Button color="gray" size="compact-md" onClick={open}>
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
              onClick={onClear}
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
        <Suspense>
          <Search onRefetch={close} />
        </Suspense>
      </Modal>
    </>
  );
};

export default FilterButton;
