import { ActionIcon, Box, Select } from '@mantine/core';
import {
  IconColumns,
  IconLayoutList,
  IconSortAscending,
  IconSortDescending,
} from '@tabler/icons-react';
import { useQueryParamSync } from 'app/hooks/useQueryParamSync';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { memo, useEffect, useMemo } from 'react';
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

export const PagingLeftColumn = memo(() => {
  const { disabledSortOnKeys } = useSerilogUiProps();
  const { control, watch } = useSearchForm();
  const currentDbKey = watch('table');

  const {
    field: { ...fieldEntries },
  } = useController({ ...control, name: 'entriesPerPage' });
  const { field: fieldSortOn } = useController({ ...control, name: 'sortOn' });
  const { field: fieldSortBy } = useController({ ...control, name: 'sortBy' });

  const { updateMultipleParams, updateParam } = useQueryParamSync();

  const isSortByDesc = fieldSortBy.value === SortDirectionOptions.Desc;

  const disableSortOn = useMemo(
    () =>
      !!disabledSortOnKeys?.length &&
      !!currentDbKey &&
      disabledSortOnKeys.includes(currentDbKey),
    [currentDbKey, disabledSortOnKeys],
  );

  const setEntries = (event: string | null) => {
    updateMultipleParams({ page: 1, [fieldEntries.name]: event });
  };
  const setSortBy = () => {
    updateParam(fieldSortBy.name)(
      isSortByDesc ? SortDirectionOptions.Asc : SortDirectionOptions.Desc,
    );
  };

  // reset sort property to default, if db key can't be sorted
  useEffect(() => {
    if (disableSortOn && fieldSortOn.value !== SortPropertyOptions.Timestamp) {
      fieldSortOn.onChange(SortPropertyOptions.Timestamp);
    }
  }, [disableSortOn, fieldSortOn]);

  return (
    <Box
      aria-label='paging-left-column'
      display='grid'
      style={{ alignItems: 'center', justifyContent: 'center', gap: '0.4em' }}>
      <Select
        {...fieldEntries}
        onChange={setEntries}
        label={fieldEntries.name}
        leftSection={<IconLayoutList />}
        data={entriesOptions}
        allowDeselect={false}
      />
      <Box
        display='grid'
        style={{
          gridTemplateColumns: '4fr 1fr',
          alignItems: 'center',
          justifyItems: 'right',
        }}>
        <Select
          {...fieldSortOn}
          label={fieldSortOn.name}
          leftSection={<IconColumns />}
          data={sortOnOptions}
          disabled={disableSortOn}
          allowDeselect={false}
          onChange={updateParam(fieldSortOn.name)}
        />
        <ActionIcon
          {...fieldSortBy}
          aria-label={fieldSortBy.name}
          onClick={setSortBy}>
          {isSortByDesc ? <IconSortDescending /> : <IconSortAscending />}
        </ActionIcon>
      </Box>
    </Box>
  );
});
