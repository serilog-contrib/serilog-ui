import loadable from '@loadable/component';
import {
  Skeleton,
  Table,
  Text,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useColumnsInfo } from 'app/hooks/useColumnsInfo';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { memo, useCallback, useMemo } from 'react';
import classes from 'style/table.module.css';
import { ColumnType, LogLevel } from '../../../types/types';
import useQueryLogs from '../../hooks/useQueryLogs';
import { isArrayGuard, isObjectGuard, isStringGuard } from '../../util/guards';
import { getBgLogLevel, splitPrintDate } from '../../util/prettyPrints';

const DetailsModal = loadable(() => import('./DetailsModal'));
const PropertiesModal = loadable(() => import('./PropertiesModal'));

const SerilogResults = () => {
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();
  const { removeException } = useColumnsInfo();

  const { data, isFetching } = useQueryLogs();

  const getCellColor = useMemo(
    () => (logLevel: string) => getBgLogLevel(theme, colorScheme, LogLevel[logLevel]),
    [colorScheme, theme],
  );

  const TableRows = useMemo(() => {
    return data?.logs.map((log) => (
      <Table.Tr
        key={log.rowNo}
        className={log.level}
        style={{
          borderBottom: `0.15em solid ${getCellColor(log.level)}`,
          boxSizing: 'border-box',
        }}
      >
        <Table.Td bg={getCellColor(log.level)} ta="center">
          <Text fz="sm">{log.level}</Text>
        </Table.Td>
        <TableCell content={log.rowNo} columnType={ColumnType.shortstring} />
        <TableCell content={log.timestamp} columnType={ColumnType.datetime} />
        <TableCell content={log.message} columnType={ColumnType.text} />
        {!removeException && (
          <TableCell
            content={log.exception}
            columnType={ColumnType.code}
            codeContentType={log.propertyType}
            codeModalTitle="Exception details"
          />
        )}
        <Table.Td>
          <PropertiesModal modalContent={log} title="View" />
        </Table.Td>
      </Table.Tr>
    ));
  }, [data?.logs, getCellColor, removeException]);

  return (
    <Table.ScrollContainer minWidth={1250}>
      <Table
        w="100%"
        verticalSpacing="md"
        highlightOnHover
        withTableBorder
        withColumnBorders
        className={classes.desktopTableCell}
      >
        <TableHead />
        <Table.Tbody className={isFetching ? classes.skeletonDesktopTableCell : ''}>
          {!isFetching && isObjectGuard(data) && isArrayGuard(data.logs) && TableRows}
          {isFetching && <DesktopSkeleton />}
        </Table.Tbody>
      </Table>
    </Table.ScrollContainer>
  );
};

const DesktopSkeleton = () =>
  [...Array(10).keys()].map((k) => (
    <Table.Tr key={k} h="4em">
      {/* TODO: 8 + custom columns */}
      {[...Array(8).keys()].map((k) => (
        <Table.Td key={k} p="0">
          <Skeleton h="4em" radius="0" />
        </Table.Td>
      ))}
    </Table.Tr>
  ));

const TableHead = memo(() => {
  const { removeException } = useColumnsInfo();

  return (
    <Table.Thead>
      <Table.Tr>
        <TableHeader text="Level" columnType={ColumnType.shortstring} />
        <TableHeader text="#" columnType={ColumnType.shortstring} />
        <TableHeader text="" columnType={ColumnType.datetime} />
        <TableHeader text="Message" columnType={ColumnType.text} />
        {!removeException && (
          <TableHeader text="Exception" columnType={ColumnType.code} />
        )}
        <TableHeader text="Properties" columnType={ColumnType.code} />
      </Table.Tr>
    </Table.Thead>
  );
});

const TableHeader = memo(
  ({ text, columnType }: { text: string; columnType: ColumnType }) => {
  switch (columnType) {
    case ColumnType.datetime:
      return (
        <>
          <Table.Th ta="center">{`${text}${text ? ' ' : ''}[Day]`}</Table.Th>
          <Table.Th ta="center">{`${text}${text ? ' ' : ''}[Time]`}</Table.Th>
        </>
      );
    case ColumnType.shortstring:
    case ColumnType.code:
      return <Table.Th ta="center">{text}</Table.Th>;
    default:
      return <Table.Th>{text}</Table.Th>;
  }
  },
);

const TableCell = memo(
  ({
  content,
  columnType,
  codeContentType,
  codeModalTitle,
}: {
  columnType: ColumnType;
  content?: string | number;
  codeModalTitle?: string;
  codeContentType?: string;
}) => {
  const { isUtc } = useSerilogUiProps();

  const splitDate = useCallback(
    (timestamp: string) => splitPrintDate(timestamp, isUtc),
    [isUtc],
  );

  switch (columnType) {
    case ColumnType.shortstring:
      return (
        <Table.Td>
          <Text size="sm" fw={500} ta="center">
            {content}
          </Text>
        </Table.Td>
      );
    case ColumnType.datetime:
      return (
        <>
          <Table.Td>
            <Text size="sm" fw={300} ta="center">
              {splitDate(`${content}`)[0]}
            </Text>
          </Table.Td>
          <Table.Td>
            <Text size="sm" fw={300} ta="center">
              {splitDate(`${content}`)[1]}
            </Text>
          </Table.Td>
        </>
      );
    case ColumnType.text:
      return (
        <Table.Td>
          <Text ta="justify" fz="sm" lh="sm">
            {content}
          </Text>
        </Table.Td>
      );
    case ColumnType.code:
      return (
        <Table.Td>
          {isStringGuard(content) ? (
            <DetailsModal
              modalContent={content}
              modalTitle={codeModalTitle}
              contentType={codeContentType}
              buttonTitle="View"
            />
          ) : null}
        </Table.Td>
      );
  }
  },
);

export default SerilogResults;
