import {
  Box,
  Button,
  Modal,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { Suspense, lazy, memo } from 'react';
import { boxButton, boxGridProperties, overlayProps } from 'style/modal';
import { LogType } from 'types/types';
import { capitalize } from '../../util/prettyPrints';

const CodeContent = lazy(() => import('./CodeContent'));

const DetailsModal = ({
  modalContent,
  buttonTitle,
  modalTitle,
  contentType,
  disabled,
  fullScreen,
}: {
  modalContent: string;
  buttonTitle: string;
  modalTitle?: string;
  contentType?: string;
  disabled?: boolean;
  fullScreen?: boolean;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const theme = useMantineTheme();
  const { colorScheme } = useMantineColorScheme();

  return (
    <>
      <Modal
        opened={opened}
        onClose={close}
        closeButtonProps={{ 'aria-label': 'close-details-modal' }}
        centered
        radius="sm"
        size="80%"
        title={modalTitle}
        overlayProps={overlayProps(colorScheme, theme.colors)}
        fullScreen={fullScreen}
      >
        <Box display="grid" style={boxGridProperties}>
          <Suspense>
            <CodeContent content={modalContent} codeType={contentType as LogType} />
          </Suspense>
        </Box>
      </Modal>

      <Box display="grid" style={boxButton}>
        <Button size="compact-sm" disabled={disabled} onClick={open}>
          {capitalize(buttonTitle)}
        </Button>
      </Box>
    </>
  );
};

export default memo(DetailsModal);
