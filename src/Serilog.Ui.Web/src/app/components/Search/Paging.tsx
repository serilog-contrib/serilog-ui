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
  em,
} from '@mantine/core';
import { useDisclosure, useMediaQuery } from '@mantine/hooks';
import { IconBook, IconLayoutList, IconListNumbers } from '@tabler/icons-react';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { toNumber } from 'app/util/guards';
import { memo, useEffect, useMemo, useState } from 'react';
import { useController } from 'react-hook-form';
import classes from 'style/search.module.css';
import useQueryLogs from '../../hooks/useQueryLogs';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const Paging = () => {
  const [opened, { close, toggle }] = useDisclosure(false);
  const lessPages = useMediaQuery(`(max-width: ${em(800)})`);

  const { control } = useSearchForm();

  const { field } = useController({ ...control, name: 'page' });
  const {
    field: { onChange, ...fieldEntries },
  } = useController({ ...control, name: 'entriesPerPage' });
  const [dialogPage, setDialogPage] = useState(field.value);

  const { data, refetch } = useQueryLogs();

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
    <Box className={classes.pagingGrid} m="xl">
      <Box display="flex" style={{ alignItems: 'center', justifyContent: 'center' }}>
        <Select
          {...fieldEntries}
          onChange={setEntries}
          label=""
          leftSection={<IconLayoutList />}
          data={entriesOptions}
          allowDeselect={false}
        ></Select>
      </Box>
      <Box
        className={classes.paginationGrid}
        display={totalPages === 0 ? 'none' : 'inherit'}
      >
        <Box m="xs" style={{ justifySelf: 'end' }}>
          <ActionIcon disabled={totalPages < 2} onClick={toggle}>
            <IconListNumbers strokeWidth={2} />
          </ActionIcon>
          <Dialog opened={opened} withCloseButton onClose={close} size="lg" radius="md">
            <Text size="sm" mb="xs" w={500}>
              Select page
            </Text>
            <Group align="flex-end">
              <NumberInput
                onChange={changePageInput}
                hideControls
                max={totalPages}
                min={1}
                placeholder={`${field.value}`}
                style={{ flex: 1 }}
                suffix={` of ${totalPages}`}
                value={dialogPage}
              />
              <Button size="sm" onClick={setPage}>
                <IconBook />
              </Button>
            </Group>
          </Dialog>
        </Box>
        <Box m="xs">
          <Pagination
            withEdges
            total={totalPages}
            siblings={lessPages ? 1 : 2}
            {...field}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default memo(Paging);
