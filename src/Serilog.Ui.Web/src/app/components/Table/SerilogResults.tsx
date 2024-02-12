import { Indicator, Table, Text, useMantineTheme } from '@mantine/core';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { useCallback, useMemo } from 'react';
import classes from 'style/table.module.css';
import { LogLevel } from '../../../types/types';
import useQueryLogsHook from '../../hooks/useQueryLogs';
import { isArrayGuard, isObjectGuard, isStringGuard } from '../../util/guards';
import { getBgLogLevel, splitPrintDate } from '../../util/prettyPrints';
import DetailsModal from './DetailsModal';

const SerilogResults = () => {
  const theme = useMantineTheme();

  const { data, isFetching } = useQueryLogsHook();

  const { isUtc } = useSerilogUiProps();

  const splitDate = useCallback(
    (timestamp: string) => splitPrintDate(timestamp, isUtc),
    [isUtc],
  );
  const getCellColor = useMemo(
    () => (logLevel: string) => getBgLogLevel(theme, LogLevel[logLevel]),
    [theme],
  );

  const TableRows = useMemo(() => {
    return !data?.logs
      ? null
      : data.logs.map((log) => (
          <Table.Tr
            key={log.rowNo}
            className={log.level}
            style={{
              border: `1px solid ${getCellColor(log.level)}`,
              boxSizing: 'border-box',
            }}
          >
            <Table.Td>
              <Indicator
                color={getCellColor(log.level)}
                size="13"
                position="middle-center"
                withBorder
              />
            </Table.Td>
            <Table.Td>
              <Text size="sm" fw={500} ta="center">
                {log.rowNo}
              </Text>
            </Table.Td>
            <Table.Td>
              <Text size="sm" fw={300} ta="center">
                {splitDate(log.timestamp)[0]}
              </Text>
            </Table.Td>
            <Table.Td>
              <Text size="sm" fw={300} ta="center">
                {splitDate(log.timestamp)[1]}
              </Text>
            </Table.Td>
            <Table.Td>
              <Text ta="justify" fz="sm" lh="sm">
                {log.message}
              </Text>
            </Table.Td>
            <Table.Td>
              {isStringGuard(log.exception) ? (
                <DetailsModal
                  modalContent={log.exception}
                  modalTitle="Exception details"
                  contentType={log.propertyType}
                />
              ) : null}
            </Table.Td>
            <Table.Td>
              {isStringGuard(log.properties) ? (
                <DetailsModal
                  modalContent={log.properties}
                  modalTitle="Properties details"
                  contentType={log.propertyType}
                />
              ) : null}
            </Table.Td>
          </Table.Tr>
        ));
  }, [data, getCellColor, splitDate]);

  return (
    <Table.ScrollContainer minWidth={1200}>
      <Table
        w="100%"
        verticalSpacing="sm"
        highlightOnHover
        withTableBorder
        withColumnBorders
        className={classes.desktopTableCell}
      >
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Level</Table.Th>
            <Table.Th ta="center">#</Table.Th>
            <Table.Th ta="center">Day</Table.Th>
            <Table.Th ta="center">Time</Table.Th>
            <Table.Th>Message</Table.Th>
            <Table.Th>Exception</Table.Th>
            <Table.Th>Properties</Table.Th>
            {/* TODO: dynamic columns configuration from ui definition */}
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody className={isFetching ? classes.skeletonDesktopTableCell : ''}>
          {!isFetching && isObjectGuard(data) && isArrayGuard(data.logs) && TableRows}
          {isFetching && desktopSkeleton}
        </Table.Tbody>
      </Table>
    </Table.ScrollContainer>
  );
};

const desktopSkeleton = [...Array(10).keys()].map((k) => (
  <Table.Tr key={k} h="40px">
    {/* TODO: 6 + custom columns */}
    {[...Array(6).keys()].map((k) => (
      <Table.Td key={k} />
    ))}
  </Table.Tr>
));

export default SerilogResults;
