import { Button, Group, Modal, useMantineTheme } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { Prism as PrismComponent } from '@mantine/prism';
import { printXmlCode } from '../../util/prettyPrints';
import { useMemo } from 'react';

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
          color:
            theme.colorScheme === 'dark' ? theme.colors.dark[9] : theme.colors.gray[2],
          opacity: 0.55,
          blur: 3,
        }}
      >
        <PrismComponent
          withLineNumbers
          trim={false}
          language={
            contentType === 'xml' ? 'markup' : contentType === 'json' ? 'json' : 'bash'
          }
        >
          {renderContent}
        </PrismComponent>
      </Modal>

      <Group position="center">
        <Button onClick={open}>Click to view</Button>
      </Group>
    </>
  );
};

export default DetailsModal;
