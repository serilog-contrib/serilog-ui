import { Button, Fieldset, Group, PasswordInput, TextInput } from '@mantine/core';
import { useAuthProperties } from 'app/hooks/useAuthProperties';
import useQueryLogs from 'app/hooks/useQueryLogs';
import { isStringGuard } from 'app/util/guards';
import { ChangeEvent } from 'react';

const BasicModal = ({ onClose }: { onClose: () => void }) => {
  const {
    authHeader,
    basic_pwd,
    basic_user,
    clearAuthState,
    saveAuthState,
    updateAuthKey,
  } = useAuthProperties();
  const { refetch } = useQueryLogs();

  const isHeaderReady = isStringGuard(authHeader);

  return (
    <>
      <Fieldset legend="Basic Authentication" mb="md">
        <TextInput
          placeholder="admin"
          label="Username"
          value={basic_user}
          onChange={(event) => {
            updateAuthKey('basic_user', event.currentTarget.value);
          }}
        />
        <PasswordInput
          placeholder="admin"
          label="Password"
          radius="sm"
          size="sm"
          style={{ flexGrow: 1 }}
          value={basic_pwd ?? ''}
          disabled={isHeaderReady}
          withAsterisk
          autoComplete="off"
          onChange={(event: ChangeEvent<HTMLInputElement>) => {
            updateAuthKey('basic_pwd', event.currentTarget.value);
          }}
        />
      </Fieldset>
      <Group display="flex" justify="right">
        <Button
          display={isHeaderReady ? 'none' : 'inherit'}
          disabled={!basic_pwd || !basic_user}
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
          Change
        </Button>
        <Button onClick={onClose}>Close</Button>
      </Group>
    </>
  );
};

export default BasicModal;
