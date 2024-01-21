import { Button, Group, PasswordInput } from '@mantine/core';
import { type ChangeEvent } from 'react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const AuthorizeModal = ({ close }: { close: () => void }) => {
  const {
    clearAuthState,
    getAuthHeader,
    jwt_bearerToken,
    saveAuthState,
    updateBearerToken,
  } = useAuthProperties();

  const isHeaderReady = isStringGuard(getAuthHeader());

  return (
    <>
      <Group mb="md">
        <PasswordInput
          placeholder="Bearer eyJhbGciOiJSUz[...]"
          label="JWT Token"
          radius="sm"
          size="sm"
          style={{ flexGrow: 1 }}
          value={jwt_bearerToken ?? ''}
          disabled={isHeaderReady}
          withAsterisk
          autoComplete="off"
          onChange={(event: ChangeEvent<HTMLInputElement>) => {
            console.log(event);

            updateBearerToken('jwt_bearerToken', event.currentTarget.value);
          }}
        />
      </Group>
      <Group display="flex" justify="right">
        <Button
          display={isHeaderReady ? 'none' : 'inherit'}
          onClick={() => {
            console.log('here on save');
            saveAuthState();
          }}
        >
          Save
        </Button>
        <Button
          display={!isHeaderReady ? 'none' : 'inherit'}
          onClick={() => {
            clearAuthState();
          }}
        >
          Change Token
        </Button>
        <Button onClick={close}>Close</Button>
      </Group>
    </>
  );
};
export default AuthorizeModal;
