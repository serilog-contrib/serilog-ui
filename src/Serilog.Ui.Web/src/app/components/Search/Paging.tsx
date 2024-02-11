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
import { useSearchForm } from 'app/hooks/useSearchForm';
import { toNumber } from 'app/util/guards';
import { memo, useEffect, useMemo, useState } from 'react';
import { useController } from 'react-hook-form';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const Paging = () => {
  const [opened, { close, toggle }] = useDisclosure(false);
  const { control } = useSearchForm();

  const { field } = useController({ ...control, name: 'page' });
  const {
    field: { onChange, ...fieldEntries },
  } = useController({ ...control, name: 'entriesPerPage' });
  const [dialogPage, setDialogPage] = useState(field.value);

  const { data, refetch } = useQueryLogsHook();

  const totalPages = useMemo(() => {
    if (!data) return 1;
    const pages = Math.ceil(data.total / data.count);
    return Number.isNaN(pages) ? 1 : pages;
  }, [data]);

  const changePageInput = (val: string | number) => {
    const newPage = toNumber(`${val}`);
    if (newPage) {
      setDialogPage(newPage);
    }
  };

  const setPage = () => {
    field.onChange(dialogPage);
    close();
    refetch();
  };

  const setEntries = (event) => {
    field.onChange(1);
    onChange(event);
  };

  useEffect(() => {
    refetch();
  }, [refetch, field.value, fieldEntries.value]);

  return (
    <Box
      display="grid"
      style={{ gridTemplateColumns: '1fr 4fr', justifyContent: 'space-between' }}
    >
      <Box display="flex" style={{ alignItems: 'center', justifyContent: 'start' }}>
        <Select
          {...fieldEntries}
          onChange={setEntries}
          label="entries"
          data={entriesOptions}
          allowDeselect={false}
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
              value={dialogPage}
              onChange={changePageInput}
              max={totalPages}
              min={1}
              hideControls
              placeholder={`${dialogPage}`}
              style={{ flex: 1 }}
            />
            /{`${totalPages}`}
            <Button onClick={setPage}>set</Button>
          </Group>
        </Dialog>
        <Pagination withEdges total={totalPages} siblings={2} {...field} />
      </Box>
    </Box>
  );
};

export default memo(Paging);
