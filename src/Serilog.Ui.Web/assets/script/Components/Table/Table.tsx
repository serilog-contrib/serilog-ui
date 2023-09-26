import { Loader, Table, useMantineTheme } from '@mantine/core';
import useQueryLogsHook from '../../Hooks/useQueryLogsHook';
import { getBgLogLevel, printDate } from '../../util/prettyPrints';
import { LogLevel } from '../../../types/types';
import DetailsModal from './DetailsModal';
import { isArrayGuard, isObjectGuard, isStringGuard } from '../../util/guards';
import { useMemo } from 'react';

const SerilogResults = () => {
  const { data, isFetching } = useQueryLogsHook();
  const theme = useMantineTheme();

  const getCellColor = useMemo(
    () => (logLevel: string) => getBgLogLevel(theme, LogLevel[logLevel]),
    [theme],
  );

  return (
    <div style={{ overflowX: 'auto' }}>
      <Table highlightOnHover withBorder withColumnBorders>
        <thead>
          <tr>
            <th>#</th>
            <th>Level</th>
            <th>Date</th>
            <th>Message</th>
            <th>Exception</th>
            <th>Properties</th>
          </tr>
        </thead>
        <tbody>
          {!isFetching &&
            isObjectGuard(data) &&
            isArrayGuard(data.logs) &&
            data.logs.map((log) => (
              // TODO: all styles and modals
              <tr key={log.rowNo} className={log.level}>
                <td>{log.rowNo}</td>
                <td style={{ backgroundColor: getCellColor(log.level) }}>
                  {log.level} TODO Color
                </td>
                <td>{printDate(log.timestamp)}</td>
                <td>{log.message}</td>
                <td>
                  {isStringGuard(log.exception) ? (
                    <DetailsModal
                      modalContent={log.exception}
                      modalTitle="Exception details"
                      contentType={log.propertyType}
                    />
                  ) : null}
                </td>
                <td>
                  {isStringGuard(log.properties) ? (
                    <DetailsModal
                      modalContent={log.properties}
                      modalTitle="Properties details"
                      contentType="json"
                    />
                  ) : null}
                </td>
              </tr>
            ))}
        </tbody>
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
