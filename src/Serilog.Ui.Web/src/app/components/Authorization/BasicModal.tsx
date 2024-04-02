import { Alert, Button, Fieldset, Group, PasswordInput, TextInput } from '@mantine/core';
import { IconAlertTriangleFilled } from '@tabler/icons-react';
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

  const onSave = async () => {
    saveAuthState(['basic_pwd', 'basic_user']);
    await refetch();
  };

  return (
    <>
      <Fieldset component="form" legend="Basic Authentication" mb="md">
        <Alert
          radius="md"
          color="yellow"
          variant="light"
          icon={<IconAlertTriangleFilled />}
          ta="justify"
          mt="xs"
          mb="md"
        >
          Basic authentication should be used only for testing and development purposes.
          <br />
          Using Basic authentication is highly discouraged in production environments!
        </Alert>
        <TextInput
          placeholder="admin"
          label="Username"
          value={basic_user}
          disabled={isHeaderReady}
          onChange={(event) => {
            updateAuthKey('basic_user', event.currentTarget.value);
          }}
        />
        <PasswordInput
          placeholder="admin"
          label="Password"
          disabled={isHeaderReady}
          radius="sm"
          size="sm"
          style={{ flexGrow: 1 }}
          value={basic_pwd ?? ''}
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
          disabled={!(basic_pwd && basic_user)}
          onClick={onSave}
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
