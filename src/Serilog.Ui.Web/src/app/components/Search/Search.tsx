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
import { memo } from 'react';
import { useController } from 'react-hook-form';
import classes from 'style/search.module.css';
import { LogLevel } from '../../../types/types';

const levelsArray = Object.keys(LogLevel).map((level) => ({
  value: level,
  label: level,
}));

const Search = () => {
  const { data: queryTableKeys, isError } = useQueryTableKeys();
  const { isUtc, setIsUtc } = useSerilogUiProps();
  const { control, handleSubmit, reset, setValue } = useSearchForm();

  const { field } = useController({ ...control, name: 'table' });
  const isTableDisabled = !(queryTableKeys && queryTableKeys.length > 1);
  const { field: levelField } = useController({ ...control, name: 'level' });
  const { field: startRangeField } = useController({ ...control, name: 'startDate' });
  const { field: endRangeField } = useController({ ...control, name: 'endDate' });
  const { field: textField } = useController({ ...control, name: 'search' });

  const { refetch } = useQueryLogs();

  const submit = async () => {
    setValue('page', 1);
    await refetch();
  };

  return (
    <form onSubmit={() => {}}>
      <Grid className={classes.searchFiltersGrid}>
        <Grid.Col span={{ xs: 6, sm: 7, md: 4, lg: 2 }} order={{ sm: 1, md: 1 }}>
          <Select
            label="Table"
            data={queryTableKeys?.map((d) => ({ value: d, label: d })) ?? []}
            allowDeselect={false}
            {...field}
            disabled={isTableDisabled}
          ></Select>
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 5, md: 2, lg: 2 }} order={{ sm: 2, md: 4, lg: 3 }}>
          <Select label="Level" data={levelsArray} {...levelField}></Select>
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 3, md: 2, lg: 4 }}>
          <DateTimePicker
            label="Start date"
            withSeconds={true}
            mx="auto"
            {...startRangeField}
          />
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 6, md: 4 }} order={{ sm: 4, md: 3, lg: 5 }}>
          <DateTimePicker
            label="End date"
            withSeconds={true}
            mx="auto"
            {...endRangeField}
          />
        </Grid.Col>
        <Grid.Col span={{ xs: 6, sm: 6, md: 6, lg: 8 }} order={{ sm: 5, md: 6, lg: 2 }}>
          <TextInput label="Search" placeholder="Your input..." {...textField} />
        </Grid.Col>
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
            <Button type="submit" onClick={handleSubmit(submit)} disabled={isError}>
              Submit
            </Button>
            <ActionIcon
              visibleFrom="lg"
              size={28}
              onClick={async () => {
                reset();
                await refetch();
              }}
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

export default memo(Search);
