import { Button, Group, PasswordInput } from '@mantine/core';
import useQueryLogsHook from 'app/hooks/useQueryLogs';
import { type ChangeEvent } from 'react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const JwtModal = ({ close }: { close: () => void }) => {
  const { authHeader, clearAuthState, jwt_bearerToken, saveAuthState, updateAuthKey } =
    useAuthProperties();
  const { refetch } = useQueryLogsHook();

  const isHeaderReady = isStringGuard(authHeader);

  return (
    <>
      <Group mb="md">
        <PasswordInput
          placeholder="eyJhbGciOiJSUz [...]"
          label="JWT Token"
          radius="sm"
          size="sm"
          style={{ flexGrow: 1 }}
          value={jwt_bearerToken ?? ''}
          disabled={isHeaderReady}
          withAsterisk
          autoComplete="off"
          onChange={(event: ChangeEvent<HTMLInputElement>) => {
            updateAuthKey('jwt_bearerToken', event.currentTarget.value);
          }}
        />
      </Group>
      <Group display="flex" justify="right">
        <Button
          display={isHeaderReady ? 'none' : 'inherit'}
          onClick={async () => {
            saveAuthState();
            await refetch();
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
export default JwtModal;
