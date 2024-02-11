import { CodeHighlight } from '@mantine/code-highlight';
import {
  Box,
  Button,
  Modal,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { useMemo } from 'react';
import { renderCodeContent } from '../../util/prettyPrints';

const DetailsModal = ({
  modalContent,
  modalTitle,
  contentType,
  disabled,
  buttonTitle,
}: {
  modalContent: string;
  modalTitle: string;
  contentType: string;
  disabled?: boolean;
  buttonTitle?: string;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();

  const codeLanguage =
    contentType === 'xml' ? 'markup' : contentType === 'json' ? 'json' : 'bash';
  const renderContent = useMemo(
    () => renderCodeContent(contentType, modalContent),
    [contentType, modalContent],
  );

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
        <CodeHighlight code={renderContent} language={codeLanguage} />
      </Modal>

      <Box display="grid" style={{ justifyContent: 'center', alignContent: 'center' }}>
        <Button size="compact-sm" disabled={disabled} onClick={open}>
          {buttonTitle || 'View'}
        </Button>
      </Box>
    </>
  );
};

export default DetailsModal;
