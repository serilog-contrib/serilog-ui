import { CodeHighlight } from '@mantine/code-highlight';
import {
  Box,
  Card,
  Group,
  Indicator,
  Loader,
  SimpleGrid,
  Skeleton,
  Spoiler,
  Tabs,
  Text,
  useMantineTheme,
} from '@mantine/core';
import useQueryLogsHook from 'app/hooks/useQueryLogs';
import { getBgLogLevel, printDate, renderCodeContent } from 'app/util/prettyPrints';
import { memo } from 'react';
import classes from 'style/table.module.css';
import { EncodedSeriLogObject, LogLevel } from 'types/types';

const SerilogResultsMobile = () => {
  const { data, isFetching } = useQueryLogsHook();

  if (!isFetching) return <Loader />;

  if (!data?.logs?.length) return <Skeleton height="10" radius="xl"></Skeleton>;

  return (
    <SimpleGrid
      w="100%"
      cols={{ base: 1, sm: 2 }}
      spacing="xs"
      verticalSpacing="sm"
      p="0.3em"
    >
      {data.logs.map((log) => (
        <LogCard key={log.rowNo} log={log} />
      ))}
    </SimpleGrid>
  );
};

const LogCard = memo(({ log }: { log: EncodedSeriLogObject }) => {
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
              {printDate(log.timestamp)}
            </Text>
          </Box>
        </Indicator>
      </Card.Section>

      <Group p="0.8em">
        <Spoiler
          hideLabel="Close"
          showLabel="More..."
          ta="justify"
          fz="sm"
          lh="sm"
          style={{ letterSpacing: '0.002em' }}
        >
          {log.message}
        </Spoiler>
      </Group>
      <Card.Section p="0.8em" h="100%" display="flex" style={{ alignContent: 'end' }}>
        <Tabs
          w="100%"
          allowTabDeactivation
          display="grid"
          style={{ alignContent: 'end' }}
        >
          <Tabs.List>
            <Tabs.Tab value="exception" disabled={!log.exception}>
              Exception
            </Tabs.Tab>
            <Tabs.Tab value="properties" disabled={!log.properties}>
              Properties
            </Tabs.Tab>
          </Tabs.List>
          <Tabs.Panel value="exception">
            <CodeHighlight
              code={renderCodeContent(log.propertyType, log.exception || '')}
              language={
                log.propertyType === 'xml'
                  ? 'markup'
                  : log.propertyType === 'json'
                    ? 'json'
                    : 'bash'
              }
            />
          </Tabs.Panel>
          <Tabs.Panel value="properties">
            <CodeHighlight
              code={renderCodeContent(log.propertyType, log.properties || '')}
              language={
                log.propertyType === 'xml'
                  ? 'markup'
                  : log.propertyType === 'json'
                    ? 'json'
                    : 'bash'
              }
            />
          </Tabs.Panel>
        </Tabs>
      </Card.Section>
    </Card>
  );
});

export default SerilogResultsMobile;
