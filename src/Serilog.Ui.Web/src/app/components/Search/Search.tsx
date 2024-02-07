/* eslint-disable react/jsx-props-no-spreading */
import {
  ActionIcon,
  Button,
  Grid,
  Group,
  Select,
  Switch,
  TextInput,
} from '@mantine/core';
import { DateTimePicker } from '@mantine/dates';
import { IconEraser } from '@tabler/icons-react';
import { useQuery } from '@tanstack/react-query';
import useQueryLogsHook from 'app/hooks/useQueryLogsHook';
import { useEffect } from 'react';
import classes from 'style/search.module.css';
import { LogLevel } from '../../../types/types';
import {
  searchFormInitialValues,
  useSearchFormContext,
} from '../../hooks/SearchFormContext';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { fetchKeys } from '../../queries/table-keys';
import { isArrayGuard } from '../../util/guards';

const levelsArray = Object.keys(LogLevel).map((p) => ({
  value: p,
  label: p,
}));

const Search = () => {
  const { getAuthHeader } = useAuthProperties();
  const { getInputProps, setInitialValues, onSubmit, reset, setFieldValue } =
    useSearchFormContext();
  const queryTableKeys = useQuery<string[]>({
    queryKey: ['get-keys'],
    queryFn: async () => {
      return await fetchKeys(getAuthHeader);
    },
    staleTime: Infinity,
  });

  const { refetch } = useQueryLogsHook();

  useEffect(() => {
    const refetchLogs = async () => await refetch();

    refetchLogs();
  }, [refetch]);

  useEffect(() => {
    const tableKeysDefaultValue = isArrayGuard(queryTableKeys.data)
      ? queryTableKeys.data.at(0)!
      : '';

    setFieldValue('table', tableKeysDefaultValue);
    setInitialValues({ ...searchFormInitialValues, table: tableKeysDefaultValue });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [queryTableKeys.data]);

  return (
    <form
      onSubmit={onSubmit((values) => {
        setFieldValue('page', 1);
        console.log(values);
        // void refetch(); // TODO temporary...
      })}
    >
      <Grid className={classes.searchFiltersGrid}>
        <Grid.Col span={{ xs: 6, sm: 7, md: 4, lg: 2 }} order={{ sm: 1, md: 1 }}>
          <Select
            label="Table"
            data={queryTableKeys.data?.map((d) => ({ value: d, label: d })) ?? []}
            {...getInputProps('table')}
          ></Select>
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 5, md: 2, lg: 2 }} order={{ sm: 2, md: 4, lg: 3 }}>
          <Select label="Level" data={levelsArray} {...getInputProps('level')}></Select>
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 3, md: 2, lg: 4 }}>
          <DateTimePicker
            label="Start date"
            withSeconds={true}
            mx="auto"
            {...getInputProps('startDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 4, md: 3, lg: 5 }}>
          <DateTimePicker
            label="End date"
            withSeconds={true}
            mx="auto"
            {...getInputProps('endDate')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 6, lg: 8 }} order={{ sm: 5, md: 6, lg: 2 }}>
          <TextInput
            label="Search"
            placeholder="Your input..."
            {...getInputProps('search')}
          />
        </Grid.Col>{' '}
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 6 }}>
          <Group justify="end" align="center" h="100%">
            <Switch
              size="md"
              offLabel="Local"
              onLabel="UTC"
              checked={getInputProps('isUtc').value}
              {...getInputProps('isUtc')}
            />
            <Button type="submit">Submit</Button>
            <ActionIcon
              visibleFrom="lg"
              size={28}
              onClick={reset}
              variant="light"
              aria-label="reset filters"
            >
              <IconEraser />
            </ActionIcon>
          </Group>
        </Grid.Col>
      </Grid>
    </form>
  );
};

export default Search;
