import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { lazy, memo, Suspense } from 'react';
import { theme } from 'style/theme';
import { AuthType } from 'types/types';
import { useAuthProperties } from '../../hooks/useAuthProperties';

const BasicModal = lazy(() => import('./BasicModal'));
const JwtModal = lazy(() => import('./JwtModal'));

const AuthorizeButton = () => {
  const [opened, { open, close }] = useDisclosure(false);
  const { authType } = useSerilogUiProps();
  const { isHeaderReady } = useAuthProperties();

  if (![AuthType.Basic, AuthType.Jwt].includes(authType ?? AuthType.Custom)) return null;

  return (
    <>
      <Button color={theme.colors?.green?.[7]} size="compact-md" onClick={open}>
        {isHeaderReady ? <IconClose /> : <IconOpen />}
        Authorize
      </Button>
      <Modal opened={opened} onClose={close} centered size="lg">
        <Suspense>
          {authType === AuthType.Basic && <BasicModal onClose={close} />}
          {authType === AuthType.Jwt && <JwtModal onClose={close} />}
        </Suspense>
      </Modal>
    </>
  );
};

const IconClose = memo(() => <IconLockCheck size="24px" />);
const IconOpen = memo(() => <IconLockOpen size="24px" />);

export default AuthorizeButton;
