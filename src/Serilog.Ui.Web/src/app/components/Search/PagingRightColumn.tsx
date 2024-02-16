import {
  ActionIcon,
  Box,
  Button,
  Dialog,
  Group,
  NumberInput,
  Pagination,
  Text,
  em,
} from '@mantine/core';
import { useDisclosure, useMediaQuery } from '@mantine/hooks';
import { IconBook, IconListNumbers } from '@tabler/icons-react';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { toNumber } from 'app/util/guards';
import { useEffect, useMemo, useState } from 'react';
import { ControllerRenderProps, FieldValues } from 'react-hook-form';
import classes from 'style/search.module.css';

export const PagingRightColumn = ({
  field,
}: {
  field: ControllerRenderProps<FieldValues, 'page'>;
}) => {
  const [opened, { close, toggle }] = useDisclosure(false);

  const [dialogPage, setDialogPage] = useState(field.value);

  const { data, refetch } = useQueryLogs();

  const lessPages = useMediaQuery(`(max-width: ${em(800)})`);
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

  const setPage = async () => {
    field.onChange(dialogPage);
    close();
    await refetch();
  };

  useEffect(() => {
    void refetch();
  }, [refetch, field.value]);

  return (
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
          // eslint-disable-next-line react/jsx-props-no-spreading
          {...field}
        />
      </Box>
    </Box>
  );
};
