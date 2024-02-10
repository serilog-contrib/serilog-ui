import { Indicator, Table, Text, useMantineTheme } from '@mantine/core';
import { useMemo } from 'react';
import classes from 'style/table.module.css';
import { LogLevel } from '../../../types/types';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';
import { isArrayGuard, isObjectGuard, isStringGuard } from '../../util/guards';
import { getBgLogLevel, splitPrintDate } from '../../util/prettyPrints';
import DetailsModal from './DetailsModal';

const SerilogResults = () => {
  const { data, isFetching } = useQueryLogsHook();
  const theme = useMantineTheme();

  const getCellColor = useMemo(
    () => (logLevel: string) => getBgLogLevel(theme, LogLevel[logLevel]),
    [theme],
  );

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
            <Table.Th>#</Table.Th>
            <Table.Th>Date</Table.Th>
            <Table.Th>Message</Table.Th>
            <Table.Th>Exception</Table.Th>
            <Table.Th>Properties</Table.Th>
            {/* TODO: dynamic columns configuration from ui definition */}
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody className={isFetching ? classes.skeletonDesktopTableCell : ''}>
          {!isFetching &&
            isObjectGuard(data) &&
            isArrayGuard(data.logs) &&
            data.logs.map((log) => {
              const date = splitPrintDate(log.timestamp);

              return (
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
                  <Table.Td w="fit-content">
                    <Text size="sm" fw={300} ta="center">
                      {date[0]}
                    </Text>
                    <Text size="sm" fw={300} ta="center">
                      {date[1]}
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
              );
            })}
          {isFetching &&
            [...Array(10).keys()].map((k) => (
              <Table.Tr key={k} h="40px">
                {/* TODO: 6 + custom columns */}
                {[...Array(6).keys()].map((k) => (
                  <Table.Td key={k} />
                ))}
              </Table.Tr>
            ))}
        </Table.Tbody>
      </Table>
    </Table.ScrollContainer>
  );
};

export default SerilogResults;
