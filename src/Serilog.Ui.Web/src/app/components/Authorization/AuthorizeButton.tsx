import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { Suspense, lazy, memo } from 'react';
import { AuthType } from 'types/types';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const BasicModal = lazy(() => import('./BasicModal'));
const JwtModal = lazy(() => import('./JwtModal'));

const AuthorizeButton = ({ customStyle }: { customStyle?: string }) => {
  const [opened, { open, close }] = useDisclosure(false);
  const { authType } = useSerilogUiProps();
  const { authHeader } = useAuthProperties();

  if (![AuthType.Basic, AuthType.Jwt].includes(authType ?? AuthType.Custom)) return null;

  const isHeaderReady = isStringGuard(authHeader);

  return (
    <>
      <Button color="green" size="compact-md" onClick={open} className={customStyle}>
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
