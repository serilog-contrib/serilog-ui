/* eslint-disable react/jsx-props-no-spreading */
import { ActionIcon, Box, Select } from '@mantine/core';
import {
  IconColumns,
  IconLayoutList,
  IconSortAscending,
  IconSortDescending,
} from '@tabler/icons-react';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { useEffect } from 'react';
import { useController } from 'react-hook-form';
import { SortDirectionOptions, SortPropertyOptions } from 'types/types';

const entriesOptions = ['10', '25', '50', '100'].map((entry) => ({
  value: entry,
  label: entry,
}));

const sortOnOptions = Object.values(SortPropertyOptions).map((entry) => ({
  value: entry,
  label: entry,
}));

export const PagingLeftColumn = ({
  changePage,
}: {
  changePage: (page: number) => void;
}) => {
  const { refetch } = useQueryLogs();
  const { control } = useSearchForm();

  const {
    field: { onChange, ...fieldEntries },
  } = useController({ ...control, name: 'entriesPerPage' });
  const { field: fieldSortOn } = useController({ ...control, name: 'sortOn' });
  const { field: fieldSortBy } = useController({ ...control, name: 'sortBy' });
  const isSortByDesc = fieldSortBy.value === SortDirectionOptions.Desc;

  const setEntries = (event: string | null) => {
    changePage(1);
    onChange(event);
  };

  useEffect(() => {
    void refetch();
  }, [refetch, fieldEntries.value]);

  return (
    <Box
      display="grid"
      style={{ alignItems: 'center', justifyContent: 'center', gap: '0.4em' }}
    >
      <Select
        {...fieldEntries}
        onChange={setEntries}
        label=""
        leftSection={<IconLayoutList />}
        data={entriesOptions}
        allowDeselect={false}
      ></Select>
      <Box
        display="grid"
        style={{
          gridTemplateColumns: '4fr 1fr',
          alignItems: 'center',
          justifyItems: 'right',
        }}
      >
        <Select
          {...fieldSortOn}
          label=""
          leftSection={<IconColumns />}
          data={sortOnOptions}
          allowDeselect={false}
        ></Select>
        <ActionIcon
          {...fieldSortBy}
          onClick={() => {
            fieldSortBy.onChange(
              isSortByDesc ? SortDirectionOptions.Asc : SortDirectionOptions.Desc,
            );
          }}
        >
          {isSortByDesc ? <IconSortDescending /> : <IconSortAscending />}
        </ActionIcon>
      </Box>
    </Box>
  );
};
