/* eslint-disable react/jsx-props-no-spreading */
import { IconListNumbers } from '@tabler/icons-react';
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
import useQueryLogsHook from '../../Hooks/useQueryLogsHook';
import { useSearchFormContext } from '../../Hooks/SearchFormContext';
import { useDisclosure } from '@mantine/hooks';
import { type MouseEvent, useState } from 'react';
import { isStringGuard } from '../../util/guards';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const Paging = () => {
  const { data, isFetching } = useQueryLogsHook();
  const form = useSearchFormContext();
  const [opened, { open, close }] = useDisclosure(false);
  const [pageToChange, changePage] = useState<number | ''>(1);
  
  // TODO Object guard
  if (isFetching || data == null) return null;
  const totalPages = Math.ceil(data.total / data.count);

  const setPage = (_: MouseEvent<HTMLButtonElement>) => {
    if (!isStringGuard(pageToChange)) {
      form.setFieldValue('page', pageToChange);
      close();
    }
  };

  return (
    <>
      <Group position="left">
        <Select
          label="entries"
          data={entriesOptions}
          {...form.getInputProps('entriesPerPage')}
        ></Select>
      </Group>
      <Group position="right">
        <ActionIcon onClick={open}>
          <IconListNumbers size={48} strokeWidth={2} />
        </ActionIcon>
        <Dialog opened={opened} withCloseButton onClose={close} size="lg" radius="md">
          <Text size="sm" mb="xs" weight={500}>
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
              sx={{ flex: 1 }}
            />
            /{`${totalPages}`}
            <Button onClick={setPage}>set</Button>
          </Group>
        </Dialog>
        <Pagination
          position="right"
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
