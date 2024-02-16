import loadable from '@loadable/component';
import { Button, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconLockCheck, IconLockOpen } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { AuthType } from 'types/types';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const BasicModal = loadable(() => import('./BasicModal'));
const JwtModal = loadable(() => import('./JwtModal'));

const AuthorizeButton = ({ customStyle }: { customStyle?: string }) => {
  const { authType } = useSerilogUiProps();
  const { authHeader } = useAuthProperties();
  const [opened, { open, close }] = useDisclosure(false);

  if (![AuthType.Basic, AuthType.Jwt].includes(authType ?? AuthType.Windows)) return null;

  const isHeaderReady = isStringGuard(authHeader);

  return (
    <>
      <Button color="green" size="compact-md" onClick={open} className={customStyle}>
        {isHeaderReady ? <IconLockCheck size="24px" /> : <IconLockOpen size="24px" />}
        Authorize
      </Button>
      <Modal opened={opened} onClose={close} centered>
        {authType === AuthType.Basic && <BasicModal onClose={close} />}
        {authType === AuthType.Jwt && <JwtModal onClose={close} />}
      </Modal>
    </>
  );
};

export default AuthorizeButton;
