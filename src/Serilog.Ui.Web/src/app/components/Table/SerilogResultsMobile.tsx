import {
  Blockquote,
  Box,
  Card,
  Group,
  Indicator,
  LoadingOverlay,
  SimpleGrid,
  Skeleton,
  Text,
  useMantineTheme,
} from '@mantine/core';
import { IconSearchOff } from '@tabler/icons-react';
import useQueryLogsHook from 'app/hooks/useQueryLogs';
import { useSearchForm } from 'app/hooks/useSearchForm';
import { getBgLogLevel, printDate } from 'app/util/prettyPrints';
import { memo } from 'react';
import classes from 'style/table.module.css';
import { EncodedSeriLogObject, LogLevel } from 'types/types';
import DetailsModal from './DetailsModal';

const SerilogResultsMobile = () => {
  const { data, isFetching } = useQueryLogsHook();

  const { getValues } = useSearchForm();
  const isUtc = getValues('isUtc');
  console.log(isUtc);

  return (
    <SimpleGrid
      w="100%"
      cols={{ base: 1, sm: 2 }}
      spacing="xs"
      verticalSpacing="sm"
      p="0.8em"
    >
      <LoadingOverlay
        visible={isFetching}
        zIndex={1000}
        overlayProps={{ radius: 'sm', blur: 2 }}
      />
      {isFetching && mobileSkeleton}
      {!data?.logs && (
        <Blockquote mt="lg" ml="lg" icon={<IconSearchOff />}>
          No results.
        </Blockquote>
      )}
      {data?.logs &&
        data.logs.map((log) => <LogCard key={log.rowNo} isUtc={isUtc} log={log} />)}
    </SimpleGrid>
  );
};

const mobileSkeleton = [...Array(4).keys()].map((_, i) => (
  <Skeleton height={'14em'} key={i} radius="none" mb="xl" />
));

const LogCard = memo(({ log, isUtc }: { log: EncodedSeriLogObject; isUtc?: boolean }) => {
  const theme = useMantineTheme();

  return (
    <Card key={log.rowNo} shadow="xs" padding="0" radius="sm" withBorder mih="14em">
      <Card.Section
        withBorder
        className={classes.mobileCardHeaderWrapper}
        style={{ borderColor: getBgLogLevel(theme, LogLevel[log.level]) }}
      >
        <Indicator
          size={14}
          position="middle-start"
          color={getBgLogLevel(theme, LogLevel[log.level])}
          zIndex={1}
        >
          <Box className={classes.mobileCardHeaderText}>
            <Text size="md" fw={600} truncate="end" ta="center">
              {log.rowNo}
            </Text>
            <Text size="xs" c={theme.colors.gray[5]} ta="center">
              {printDate(log.timestamp, isUtc)}
            </Text>
          </Box>
        </Indicator>
      </Card.Section>

      <Group p="0.8em">
        <Text ta="justify" fz="sm" lh="sm" style={{ letterSpacing: '0.002em' }}>
          {log.message}
        </Text>
      </Group>
      <Card.Section
        p="0.8em"
        display="grid"
        w="80%"
        fz="xs"
        m="auto auto 0.5em"
        style={{
          gridTemplateColumns: '1fr 1fr',
          justifyContent: 'space-evenly',
          alignContent: 'center',
        }}
      >
        <DetailsModal
          modalContent={log.exception || ''}
          modalTitle="Exception details"
          buttonTitle="exception"
          contentType={log.propertyType}
          disabled={!log.exception}
        />
        <DetailsModal
          modalContent={log.properties || ''}
          modalTitle="Properties details"
          contentType={log.propertyType}
          disabled={!log.properties}
          buttonTitle="properties"
        />
      </Card.Section>
    </Card>
  );
});

export default SerilogResultsMobile;
