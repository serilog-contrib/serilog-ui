import { Button, Modal } from '@mantine/core';
import { useAuthProperties } from '../../Hooks/useAuthProperties';
import { useDisclosure } from '@mantine/hooks';
import AuthorizeModal from './AuthorizeModal';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { isStringGuard } from '../../util/guards';
import { AuthType } from '../../../types/types';

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
