import { Button, Modal, em } from '@mantine/core';
import { useDisclosure, useMediaQuery } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';
import AuthorizeModal from './AuthorizeModal';

const AuthorizeButton = () => {
  const isSmallish = useMediaQuery(`(max-width: ${em(360)})`);

  const { jwt_bearerToken } = useAuthProperties();
  const [opened, { open, close }] = useDisclosure(false);

  // TODO: expose to different Modals if (authProps.authType !== AuthType.Jwt) return null;

  return (
    <>
      <Button
        color="green"
        size={isSmallish ? 'compact-xs' : 'compact-md'}
        onClick={open}
      >
        {isStringGuard(jwt_bearerToken) ? (
          <IconLockCheck size={isSmallish ? '18px' : '24px'} />
        ) : (
          <IconLockOpen size={isSmallish ? '18px' : '24px'} />
        )}
        Authorize
      </Button>
      <Modal opened={opened} onClose={close} title="Authorization" centered>
        <AuthorizeModal close={close} />
      </Modal>
    </>
  );
};

export default AuthorizeButton;
