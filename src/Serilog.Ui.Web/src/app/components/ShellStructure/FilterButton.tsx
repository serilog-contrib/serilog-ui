import loadable from '@loadable/component';
import { ActionIcon, Box, Button, Modal, Text } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconEraser, IconFilterSearch } from '@tabler/icons-react';
import { useCloseOnResize } from 'app/hooks/useCloseOnResize';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { useSearchForm } from 'app/hooks/useSearchForm';
import classes from 'style/header.module.css';

const Search = loadable(() => import('../Search/Search'));

const FilterButton = () => {
  const [filterModalOpened, { open, close }] = useDisclosure(false);
  const { reset } = useSearchForm();
  const { refetch } = useQueryLogs();

  useCloseOnResize(close);

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
              onClick={async () => {
                reset();
                await refetch();
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

export default FilterButton;
