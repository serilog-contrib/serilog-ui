import { Button, Group, PasswordInput } from '@mantine/core';
import { useEffect, type ChangeEvent } from 'react';
import { useImmer } from 'use-immer';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const AuthorizeModal = ({ close }: { close: () => void }) => {
  const { authProps, updateBearerToken } = useAuthProperties();
  const [token, setToken] = useImmer(authProps.bearerToken);

  useEffect(() => {
    setToken(authProps.bearerToken);
  }, [authProps.bearerToken, setToken]);

  return (
    <form onSubmit={() => {}}>
      <PasswordInput
        placeholder="Bearer eyJhbGciOiJSUz..."
        label="JWT Token:"
        radius="md"
        size="md"
        value={token}
        disabled={isStringGuard(authProps.bearerToken)}
        withAsterisk
        autoComplete="off"
        onChange={(event: ChangeEvent<HTMLInputElement>) => {
          authProps.validateToken(event.currentTarget.value);
          setToken(event.currentTarget.value);
        }}
      />

      <Group display={!isStringGuard(authProps.bearerToken) ? 'inherit' : 'none'}>
        <Button
          onClick={() => {
            updateBearerToken(token);
          }}
        >
          Save
        </Button>
      </Group>
      <Group display={isStringGuard(authProps.bearerToken) ? 'inherit' : 'none'}>
        <Button
          onClick={() => {
            updateBearerToken('');
          }}
        >
          Change Token
        </Button>
      </Group>
      <Group>
        <Button onClick={close}>Close</Button>
      </Group>
    </form>
  );
};
export default AuthorizeModal;
