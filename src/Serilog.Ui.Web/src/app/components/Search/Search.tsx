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
import useQueryLogs from 'app/hooks/useQueryLogs';
import { useQueryTableKeys } from 'app/hooks/useQueryTableKeys';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { memo, useEffect } from 'react';
import { useController, useWatch } from 'react-hook-form';
import classes from 'style/search.module.css';
import { DispatchedCustomEvents, LogLevel } from '../../../types/types';

const levelsArray = Object.keys(LogLevel).map((level) => ({
  value: level,
  label: level,
}));

const Search = ({ onRefetch }: { onRefetch?: () => void }) => {
  const { isError } = useQueryTableKeys(true);
  const { isUtc, setIsUtc } = useSerilogUiProps();
  const { handleSubmit, reset, setValue } = useSearchForm();
  const { refetch } = useQueryLogs();
  const currentPage = useWatch({ name: 'page' });

  const onSubmit = async () => {
    if (currentPage === 1) {
      await refetch();
    } else {
      setValue('page', 1);
    }

    onRefetch?.();
  };

  const onClear = async () => {
    const shouldRefetch = reset();

    if (shouldRefetch) {
      await refetch();
    }
  };

  useEffect(() => {
    const resetTableKey = () => {
      reset(true);
    };

    document.addEventListener(DispatchedCustomEvents.RemoveTableKey, resetTableKey);

    return () =>
      document.removeEventListener(DispatchedCustomEvents.RemoveTableKey, resetTableKey);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <form aria-label="search-logs-form" onSubmit={() => {}}>
      <Grid className={classes.searchFiltersGrid}>
        <SelectDbKeyInput />
        <SelectLevelInput />
        <DateStartInput />
        <DateEndInput />
        <TextSearchInput />
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 6 }}>
          <Group justify="end" align="center" h="100%">
            <Switch
              size="md"
              offLabel="Local"
              onLabel="UTC"
              checked={isUtc}
              onChange={(event) => {
                setIsUtc(event.currentTarget.checked);
              }}
            />
            <Button type="submit" onClick={handleSubmit(onSubmit)} disabled={isError}>
              Submit
            </Button>
            <ActionIcon
              visibleFrom="lg"
              size={28}
              onClick={onClear}
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

const dbKeySpan = { xs: 6, sm: 7, md: 4, lg: 2 };
const dbKeyOrder = { sm: 1, md: 1 };
const SelectDbKeyInput = memo(() => {
  const { control } = useSearchForm();
  const { data: queryTableKeys } = useQueryTableKeys(true);
  const { field } = useController({ ...control, name: 'table' });
  const queryKeys = queryTableKeys?.map((d) => ({ value: d, label: d })) ?? [];
  const isTableDisabled = !queryKeys.length;

  return (
    <Grid.Col span={dbKeySpan} order={dbKeyOrder}>
      <Select
        allowDeselect={false}
        data={queryKeys}
        disabled={isTableDisabled}
        label="Table"
        {...field}
      ></Select>
    </Grid.Col>
  );
});

const levelSpan = { xs: 6, sm: 5, md: 2, lg: 2 };
const levelOrder = { sm: 2, md: 4, lg: 3 };
const SelectLevelInput = memo(() => {
  const { control } = useSearchForm();
  const { field: levelField } = useController({ ...control, name: 'level' });

  return (
    <Grid.Col span={levelSpan} order={levelOrder}>
      <Select label="Level" data={levelsArray} {...levelField}></Select>
    </Grid.Col>
  );
});

const searchSpan = { xs: 6, sm: 6, md: 6, lg: 8 };
const searchOrder = { sm: 5, md: 6, lg: 2 };
const TextSearchInput = memo(() => {
  const { control } = useSearchForm();
  const { field: searchField } = useController({ ...control, name: 'search' });

  return (
    <Grid.Col span={searchSpan} order={searchOrder}>
      <TextInput label="Search" placeholder="Your input..." {...searchField} />
    </Grid.Col>
  );
});

const dateStartSpan = { xs: 6, sm: 6, md: 4 };
const dateStartOrder = { sm: 3, md: 2, lg: 4 };
const DateStartInput = () => {
  const { control } = useSearchForm();
  const { field: startRangeField } = useController({ ...control, name: 'startDate' });

  return (
    <Grid.Col span={dateStartSpan} order={dateStartOrder}>
      <DateTimePicker
        label="Start date"
        clearable
        timeInputProps={{
          'aria-label': 'start-time-input',
        }}
        withSeconds={true}
        mx="auto"
        {...startRangeField}
      />
    </Grid.Col>
  );
};

const dateEndSpan = { xs: 6, sm: 6, md: 4 };
const dateEndOrder = { sm: 4, md: 3, lg: 5 };
const DateEndInput = () => {
  const { control } = useSearchForm();
  const { field: endRangeField } = useController({ ...control, name: 'endDate' });

  return (
    <Grid.Col span={dateEndSpan} order={dateEndOrder}>
      <DateTimePicker
        label="End date"
        clearable
        timeInputProps={{
          'aria-label': 'end-time-input',
        }}
        withSeconds={true}
        mx="auto"
        {...endRangeField}
      />
    </Grid.Col>
  );
};

export default memo(Search);
