import { Button, Fieldset, Group, PasswordInput } from '@mantine/core';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { type ChangeEvent } from 'react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const JwtModal = ({ onClose }: { onClose: () => void }) => {
  const { authHeader, clearAuthState, jwt_bearerToken, saveAuthState, updateAuthKey } =
    useAuthProperties();
  const { refetch } = useQueryLogs();

  const isHeaderReady = isStringGuard(authHeader);

  const onSave = async () => {
    saveAuthState(['jwt_bearerToken']);
    await refetch();
  };
  return (
    <>
      <Fieldset
        component="form"
        legend="JWT Authentication"
        mb="md"
        onSubmit={(event) => {
          event.preventDefault();
        }}
      >
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
      </Fieldset>
      <Group display="flex" justify="right">
        <Button display={isHeaderReady ? 'none' : 'inherit'} onClick={onSave}>
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
        <Button onClick={onClose}>Close</Button>
      </Group>
    </>
  );
};
export default JwtModal;
