import loadable from '@loadable/component';
import {
  Accordion,
  ActionIcon,
  Box,
  Button,
  CopyButton,
  Modal,
  Switch,
  TextInput,
  Textarea,
  Tooltip,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconCheck, IconCodeDots, IconCopy } from '@tabler/icons-react';
import { useColumnsInfo } from 'app/hooks/useColumnsInfo';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { capitalize, convertLogType, printDate } from 'app/util/prettyPrints';
import { memo } from 'react';
import { boxButton, boxGridProperties, overlayProps } from 'style/modal';
import { ColumnType, EncodedSeriLogObject } from 'types/types';

const CodeContent = loadable(() => import('./CodeContent'));

const PropertiesModal = ({
  modalContent,
  title = 'Properties',
}: {
  modalContent: EncodedSeriLogObject;
  title?: string;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const { level, message, propertyType, rowNo, timestamp, exception, ...rest } =
    modalContent;

  return (
    <>
      <Modal
        opened={opened}
        onClose={close}
        centered
        radius="sm"
        size="xl"
        title="Additional Columns"
        overlayProps={overlayProps(colorScheme, theme.colors)}
      >
        <Box display="grid" style={boxGridProperties}>
          {Object.keys(rest).map((k) => (
            <RenderProps key={k} name={k} prop={rest[k]} logPropertyType={propertyType} />
          ))}
        </Box>
      </Modal>

      <Box display="grid" style={boxButton}>
        <Button size="compact-sm" disabled={!Object.keys(rest).length} onClick={open}>
          {title}
        </Button>
      </Box>
    </>
  );
};

const RenderProps = memo(
  ({
    logPropertyType,
    name,
    prop,
  }: {
    name: string;
    prop: unknown;
    logPropertyType: string;
  }) => {
    const { isUtc } = useSerilogUiProps();
    const { additionalColumn, removeProperties } = useColumnsInfo(name, logPropertyType);

    if (!additionalColumn) return null;

    const propertyName = capitalize(name);

    if (additionalColumn.codeType && !removeProperties) {
      return (
        <Accordion style={{ order: 1 }}>
          <Accordion.Item value={propertyName}>
            <Accordion.Control icon={<IconCodeDots />}>{propertyName}</Accordion.Control>
            <Accordion.Panel>
              <CodeContent
                prop={prop as string}
                codeType={convertLogType(additionalColumn.codeType)}
              />
            </Accordion.Panel>
          </Accordion.Item>
        </Accordion>
      );
    }

    switch (additionalColumn?.typeName) {
      case ColumnType.boolean:
        return (
          <Switch
            labelPosition="left"
            checked={prop as boolean}
            label={propertyName}
            size="md"
            onLabel="true"
            offLabel="false"
          />
        );
      case ColumnType.datetime:
      case ColumnType.datetimeoffset:
        const date = printDate(prop as string, isUtc);
        return (
          <TextInput
            label={propertyName}
            rightSectionPointerEvents="all"
            rightSection={<CopySection value={date} />}
            disabled
            value={date}
          />
        );
    }

    return (
      <Textarea
        autosize
        label={propertyName}
        value={prop as string}
        disabled
        rightSectionPointerEvents="all"
        rightSection={<CopySection value={prop as string} />}
        minRows={1}
        maxRows={4}
      />
    );
  },
);

const CopySection = memo(({ value }: { value: string }) => (
  <CopyButton value={value}>
    {({ copied, copy }) => (
      <Tooltip label={copied ? 'Copied' : 'Copy'} withArrow position="right">
        <IconCopySection copied={copied} copy={copy} />
      </Tooltip>
    )}
  </CopyButton>
));

const IconCopySection = memo(
  ({ copied, copy }: { copied: boolean; copy: () => void }) => (
    <ActionIcon color={copied ? 'teal' : 'gray'} variant="subtle" onClick={copy}>
      {copied ? <IconCheck /> : <IconCopy />}
    </ActionIcon>
  ),
);

export default PropertiesModal;
