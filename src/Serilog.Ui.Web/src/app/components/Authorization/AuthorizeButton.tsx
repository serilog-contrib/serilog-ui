import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';
import AuthorizeModal from './AuthorizeModal';

const AuthorizeButton = ({ customStyle }: { customStyle?: string }) => {
  const { jwt_bearerToken } = useAuthProperties();
  const [opened, { open, close }] = useDisclosure(false);

  // TODO: expose to different Modals if (authProps.authType !== AuthType.Jwt) return null;

  return (
    <>
      <Button color="green" size="compact-md" onClick={open} className={customStyle}>
        {isStringGuard(jwt_bearerToken) ? (
          <IconLockCheck size="24px" />
        ) : (
          <IconLockOpen size="24px" />
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
