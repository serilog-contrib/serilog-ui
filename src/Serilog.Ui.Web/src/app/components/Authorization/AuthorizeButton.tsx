import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { AuthType } from 'types/types';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';
import AuthorizeModal from './JwtModal';

const AuthorizeButton = ({ customStyle }: { customStyle?: string }) => {
  const { authType } = useSerilogUiProps();
  const { authHeader } = useAuthProperties();
  const [opened, { open, close }] = useDisclosure(false);

  const isHeaderReady = isStringGuard(authHeader);

  return (
    <>
      <Button color="green" size="compact-md" onClick={open} className={customStyle}>
        {isHeaderReady ? <IconLockCheck size="24px" /> : <IconLockOpen size="24px" />}
        Authorize
      </Button>
      <Modal opened={opened} onClose={close} title="Authorization" centered>
        {/* // TODO: expose to different Modals */}
        {authType === AuthType.Jwt && <AuthorizeModal close={close} />}
      </Modal>
    </>
  );
};

export default AuthorizeButton;
