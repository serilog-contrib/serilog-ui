import { Loader, Table, useMantineTheme } from '@mantine/core';
import { useMemo } from 'react';
import { LogLevel } from '../../../types/types';
import useQueryLogsHook from '../../hooks/useQueryLogsHook';
import { isArrayGuard, isObjectGuard, isStringGuard } from '../../util/guards';
import { getBgLogLevel, printDate } from '../../util/prettyPrints';
import DetailsModal from './DetailsModal';

const SerilogResults = () => {
  const { data, isFetching } = useQueryLogsHook();
  const theme = useMantineTheme();

  const getCellColor = useMemo(
    () => (logLevel: string) => getBgLogLevel(theme, LogLevel[logLevel]),
    [theme],
  );

  return (
    <div style={{ overflowX: 'auto' }}>
      <Table highlightOnHover withTableBorder withColumnBorders>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>#</Table.Th>
            <Table.Th>Level</Table.Th>
            <Table.Th>Date</Table.Th>
            <Table.Th>Message</Table.Th>
            <Table.Th>Exception</Table.Th>
            <Table.Th>Properties</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {!isFetching &&
            isObjectGuard(data) &&
            isArrayGuard(data.logs) &&
            data.logs.map((log) => (
              // TODO: all styles and modals
              <Table.Tr key={log.rowNo} className={log.level}>
                <Table.Td>{log.rowNo}</Table.Td>
                <Table.Td style={{ backgroundColor: getCellColor(log.level) }}>
                  {log.level} TODO Color
                </Table.Td>
                <Table.Td>{printDate(log.timestamp)}</Table.Td>
                <Table.Td>{log.message}</Table.Td>
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
                      contentType="json"
                    />
                  ) : null}
                </Table.Td>
              </Table.Tr>
            ))}
        </Table.Tbody>
      </Table>
      {isFetching && (
        <Loader
          // TODO replace with logs skeleton
          variant="dots"
        />
      )}
    </div>
  );
};

export default SerilogResults;
