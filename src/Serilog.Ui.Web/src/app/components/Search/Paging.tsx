/* eslint-disable react/jsx-props-no-spreading */
import {
  ActionIcon,
  Box,
  Button,
  Dialog,
  Group,
  NumberInput,
  Pagination,
  Select,
  Text,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconListNumbers } from '@tabler/icons-react';
import { useMemo, useState, type MouseEvent } from 'react';
import { useSearchFormContext } from '../../hooks/SearchFormContext';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';
import { isStringGuard } from '../../util/guards';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const Paging = () => {
  const [opened, { close, toggle }] = useDisclosure(false);
  const { data, isFetching } = useQueryLogsHook();
  const { getInputProps, setFieldValue, values } = useSearchFormContext();
  const [pageToChange, changePage] = useState<number | string>(1);

  const totalPages = useMemo(() => {
    if (!data) return 1;
    const pages = Math.ceil(data.total / data.count);
    return Number.isNaN(pages) ? 1 : pages;
  }, [data]);

  // TODO Object guard
  if (isFetching) return null;

  const setPage = (_: MouseEvent<HTMLButtonElement>) => {
    console.log('todo', _);
    if (!isStringGuard(pageToChange)) {
      setFieldValue('page', pageToChange);
      close();
    }
  };

  console.log(values);
  return (
    <Box
      display="grid"
      style={{ gridTemplateColumns: '1fr 4fr', justifyContent: 'space-between' }}
    >
      <Box display="flex" style={{ alignItems: 'center', justifyContent: 'start' }}>
        <Select
          label="entries"
          data={entriesOptions}
          defaultValue={getInputProps('entriesPerPage').value.toString()}
          value={getInputProps('entriesPerPage').value.toString()}
          {...getInputProps('entriesPerPage')}
        ></Select>
      </Box>
      <Box display="flex" style={{ alignItems: 'center', justifyContent: 'end' }}>
        <ActionIcon disabled={totalPages < 2} onClick={toggle} mr="xs">
          <IconListNumbers strokeWidth={2} />
        </ActionIcon>
        <Dialog opened={opened} withCloseButton onClose={close} size="lg" radius="md">
          <Text size="sm" mb="xs" w={500}>
            Select page
          </Text>
          <Group align="flex-end">
            <NumberInput
              value={pageToChange}
              onChange={changePage}
              max={totalPages}
              min={1}
              hideControls
              placeholder={`${values.page}`}
              style={{ flex: 1 }}
            />
            /{`${totalPages}`}
            <Button onClick={setPage}>set</Button>
          </Group>
        </Dialog>
        <Pagination
          withEdges
          total={totalPages}
          siblings={2}
          {...getInputProps('page')}
        />
      </Box>
    </Box>
  );
};

export default Paging;
