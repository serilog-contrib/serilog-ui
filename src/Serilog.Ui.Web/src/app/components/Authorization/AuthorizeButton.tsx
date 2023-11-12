import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { AuthType } from '../../../types/types';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';
import AuthorizeModal from './AuthorizeModal';

const AuthorizeButton = () => {
  const { authProps } = useAuthProperties();
  const [opened, { open, close }] = useDisclosure(false);

  if (authProps.authType !== AuthType.Jwt) return null;

  return (
    <>
      <Button color="green" onClick={open}>
        {isStringGuard(authProps.bearerToken) ? <IconLockCheck /> : <IconLockOpen />}
        Authorize
      </Button>
      <Modal opened={opened} onClose={close} title="JWT Authorization" centered>
        <AuthorizeModal close={close} />
      </Modal>
    </>
  );
};

export default AuthorizeButton;
