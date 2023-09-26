/* eslint-disable react/jsx-props-no-spreading */
import { TextInput, Button, Select, Grid } from '@mantine/core';
import { DateTimePicker } from '@mantine/dates';
import { useQuery } from '@tanstack/react-query';
import { useSearchFormContext } from '../../Hooks/SearchFormContext';
import useQueryLogsHook from '../../Hooks/useQueryLogsHook';
import { useEffect } from 'react';
import { fetchKeys } from '../../Queries/table-keys';
import { useAuthProperties } from '../../Hooks/useAuthProperties';
import { isArrayGuard, isStringGuard } from '../../util/guards';
import { LogLevel } from '../../../types/types';

const levelsArray = Object.keys(LogLevel).map((p) => ({
  value: p,
  label: p,
}));

const Search = () => {
  const { authProps } = useAuthProperties();
  const form = useSearchFormContext();
  const queryTableKeys = useQuery({
    queryKey: ['get-keys'],
    queryFn: async () => {
      return await fetchKeys(authProps);
    },
    staleTime: Infinity,
    onSuccess: (data) => {
      const tableKeysDefaultValue = isArrayGuard(data) && data.at(0);
      form.setFieldValue(
        'table',
        isStringGuard(tableKeysDefaultValue) ? tableKeysDefaultValue : '',
      );
    },
    onError: (err) => {
      console.error(err);
    },
  });

  const { refetch } = useQueryLogsHook();

  useEffect(() => {
    const refetchLogs = async () => await refetch();

    void refetchLogs();
  }, [refetch]);

  return (
    <form
      onSubmit={form.onSubmit((values) => {
        form.setFieldValue('page', 1);
        console.log(values);
        void refetch(); // TODO temporary...
      })}
    >
      <Grid w="100%" justify="space-between" align="flex-end">
        <Grid.Col xs={6} sm={8} md={4} lg={3} orderSm={1} orderMd={1}>
          <Select
            label="Table"
            data={queryTableKeys.data?.map((d) => ({ value: d, label: d })) ?? []}
            {...form.getInputProps('table')}
          ></Select>
        </Grid.Col>
        <Grid.Col xs={6} sm={4} md={3} lg={3} orderSm={2} orderMd={4}>
          <Select
            label="Level"
            data={levelsArray}
            {...form.getInputProps('level')}
          ></Select>
        </Grid.Col>{' '}
        <Grid.Col xs={6} sm={6} md={4} lg={3} orderSm={3} orderMd={2}>
          <DateTimePicker
            label="Start date:"
            withSeconds={true}
            mx="auto"
            {...form.getInputProps('startDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col xs={6} sm={6} md={4} lg={3} orderSm={4} orderMd={3}>
          <DateTimePicker
            label="End date:"
            withSeconds={true}
            mx="auto"
            {...form.getInputProps('endDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col xs={6} sm={6} md={5} lg={3} orderSm={6} orderMd={6}>
          <TextInput
            withAsterisk
            label="Search"
            placeholder="Your input..."
            {...form.getInputProps('search')}
          />
        </Grid.Col>{' '}
        <Grid.Col xs={3} sm={3} md={2} lg={3} orderSm={7}>
          <Button type="submit">Submit</Button>
        </Grid.Col>
      </Grid>
    </form>
  );
};

export default Search;
