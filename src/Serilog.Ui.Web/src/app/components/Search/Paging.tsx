/* eslint-disable react/jsx-props-no-spreading */
import {
  ActionIcon,
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
import { useState, type MouseEvent } from 'react';
import { useSearchFormContext } from '../../hooks/SearchFormContext';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';
import { isStringGuard } from '../../util/guards';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const Paging = () => {
  const { data, isFetching } = useQueryLogsHook();
  const form = useSearchFormContext();
  const [opened, { open, close }] = useDisclosure(false);
  const [pageToChange, changePage] = useState<number | string>(1);

  // TODO Object guard
  if (isFetching || data == null) return null;
  const totalPages = Math.ceil(data.total / data.count);

  const setPage = (_: MouseEvent<HTMLButtonElement>) => {
    console.log('todo', _);
    if (!isStringGuard(pageToChange)) {
      form.setFieldValue('page', pageToChange);
      close();
    }
  };

  return (
    <>
      <Group justify="left">
        <Select
          label="entries"
          data={entriesOptions}
          {...form.getInputProps('entriesPerPage')}
        ></Select>
      </Group>
      <Group justify="right">
        <ActionIcon onClick={open}>
          <IconListNumbers size={48} strokeWidth={2} />
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
              placeholder={`${form.values.page}`}
              style={{ flex: 1 }}
            />
            /{`${totalPages}`}
            <Button onClick={setPage}>set</Button>
          </Group>
        </Dialog>
        <Pagination
          style={{ justifyContent: 'right' }}
          withEdges
          total={totalPages}
          siblings={3}
          {...form.getInputProps('page')}
        />
      </Group>
    </>
  );
};

export default Paging;
