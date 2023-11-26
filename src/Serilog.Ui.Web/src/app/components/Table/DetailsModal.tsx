import { CodeHighlight } from '@mantine/code-highlight';
import '@mantine/code-highlight/styles.css';
import {
  Button,
  Group,
  Modal,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { useMemo } from 'react';
import { printXmlCode } from '../../util/prettyPrints';

const DetailsModal = ({
  modalContent,
  modalTitle,
  contentType,
}: {
  modalContent: string;
  modalTitle: string;
  contentType: string;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();

  const renderContent = useMemo(() => {
    if (contentType === 'xml') return printXmlCode(contentType);
    if (contentType === 'json') {
      try {
        return JSON.stringify(JSON.parse(modalContent), null, 2) ?? '{}';
      } catch {
        console.warn(`${modalContent} is not a valid json!`);
      }
    }
    return '{}';
  }, [contentType, modalContent]);

  return (
    <>
      <Modal
        opened={opened}
        onClose={close}
        centered
        radius="sm"
        size="xl"
        title={modalTitle}
        overlayProps={{
          color: colorScheme === 'dark' ? theme.colors.dark[9] : theme.colors.gray[2],
          opacity: 0.55,
          blur: 3,
        }}
      >
        <CodeHighlight
          code={renderContent}
          language={
            contentType === 'xml' ? 'markup' : contentType === 'json' ? 'json' : 'bash'
          }
        />
      </Modal>

      <Group>
        <Button onClick={open}>Click to view</Button>
      </Group>
    </>
  );
};

export default DetailsModal;
