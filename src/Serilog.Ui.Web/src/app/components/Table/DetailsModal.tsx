import {
  Box,
  Button,
  Modal,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { useEffect, useState } from 'react';
import classes from 'style/table.module.css';
import { renderCodeContent } from '../../util/prettyPrints';

const DetailsModal = ({
  modalContent,
  modalTitle,
  contentType,
  disabled,
  buttonTitle,
}: {
  modalContent: string;
  modalTitle?: string;
  contentType?: string;
  disabled?: boolean;
  buttonTitle?: string;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();
  const [renderContent, setRenderContent] = useState<TrustedHTML>('');

  useEffect(() => {
    const fetchContent = async () => {
      const content = await renderCodeContent(modalContent, contentType);
      setRenderContent(content ?? '');
    };

    void fetchContent();
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
        <div
          dangerouslySetInnerHTML={{ __html: renderContent }}
          className={classes.detailModalCode}
        ></div>
      </Modal>

      <Box display="grid" style={{ justifyContent: 'center', alignContent: 'center' }}>
        <Button size="compact-sm" disabled={disabled} onClick={open}>
          {buttonTitle ? buttonTitle : 'View'}
        </Button>
      </Box>
    </>
  );
};

export default DetailsModal;
