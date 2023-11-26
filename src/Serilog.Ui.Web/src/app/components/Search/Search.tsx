/* eslint-disable react/jsx-props-no-spreading */
import { Button, Grid, Select, TextInput } from '@mantine/core';
import { DateTimePicker } from '@mantine/dates';
import { useQuery } from '@tanstack/react-query';
import { useEffect } from 'react';
import { LogLevel } from '../../../types/types';
import { useSearchFormContext } from '../../hooks/SearchFormContext';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';
import { fetchKeys } from '../../queries/table-keys';
import { isArrayGuard, isStringGuard } from '../../util/guards';

const levelsArray = Object.keys(LogLevel).map((p) => ({
  value: p,
  label: p,
}));

const Search = () => {
  const { authProps } = useAuthProperties();
  const form = useSearchFormContext();
  const queryTableKeys = useQuery<string[]>({
    queryKey: ['get-keys'],
    queryFn: async () => {
      return await fetchKeys(authProps);
    },
    staleTime: Infinity,
  });

  const { refetch } = useQueryLogsHook();

  useEffect(() => {
    const refetchLogs = async () => await refetch();

    refetchLogs();
  }, [refetch]);

  useEffect(() => {
    const tableKeysDefaultValue =
      isArrayGuard(queryTableKeys.data) && queryTableKeys.data.at(0);
    form.setFieldValue(
      'table',
      isStringGuard(tableKeysDefaultValue) ? tableKeysDefaultValue : '',
    );
  }, [form, queryTableKeys.data]);

  return (
    <form
      onSubmit={form.onSubmit((values) => {
        form.setFieldValue('page', 1);
        console.log(values);
        void refetch(); // TODO temporary...
      })}
    >
      <Grid w="100%" justify="space-between" align="flex-end">
        <Grid.Col span={{ xs: 6, sm: 8, md: 4, lg: 3 }} order={{ sm: 1, md: 1 }}>
          <Select
            label="Table"
            data={queryTableKeys.data?.map((d) => ({ value: d, label: d })) ?? []}
            {...form.getInputProps('table')}
          ></Select>
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 4, md: 3, lg: 3 }} order={{ sm: 2, md: 4 }}>
          <Select
            label="Level"
            data={levelsArray}
            {...form.getInputProps('level')}
          ></Select>
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 4, lg: 3 }} order={{ sm: 3, md: 2 }}>
          <DateTimePicker
            label="Start date:"
            withSeconds={true}
            mx="auto"
            {...form.getInputProps('startDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 4, lg: 3 }} order={{ sm: 4, md: 3 }}>
          <DateTimePicker
            label="End date:"
            withSeconds={true}
            mx="auto"
            {...form.getInputProps('endDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 5, lg: 3 }} order={{ sm: 6, md: 6 }}>
          <TextInput
            withAsterisk
            label="Search"
            placeholder="Your input..."
            {...form.getInputProps('search')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 3, sm: 3, md: 2, lg: 3 }} order={{ sm: 7 }}>
          <Button type="submit">Submit</Button>
        </Grid.Col>
      </Grid>
    </form>
  );
};

export default Search;
