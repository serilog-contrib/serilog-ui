import { Alert, Button, Fieldset, Group, PasswordInput, TextInput } from '@mantine/core';
import { IconAlertTriangleFilled } from '@tabler/icons-react';
import { useAuthProperties } from 'app/hooks/useAuthProperties';
import { ChangeEvent, useState } from 'react';

const BasicModal = ({ onClose }: { onClose: () => void }) => {
  const { isHeaderReady, basic_pwd, basic_user, clearAuthState, saveAuthState } =
    useAuthProperties();
  const [user, setUser] = useState(basic_user ?? '');
  const [pwd, setPwd] = useState(basic_pwd ?? '');

  const onSave = async () => {
    saveAuthState({ basic_pwd: pwd, basic_user: user });
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
          value={user}
          disabled={isHeaderReady}
          onChange={(event) => {
            setUser(event.currentTarget.value);
          }}
        />
        <PasswordInput
          placeholder="admin"
          label="Password"
          disabled={isHeaderReady}
          radius="sm"
          size="sm"
          style={{ flexGrow: 1 }}
          value={pwd}
          withAsterisk
          autoComplete="off"
          onChange={(event: ChangeEvent<HTMLInputElement>) => {
            setPwd(event.currentTarget.value);
          }}
        />
      </Fieldset>
      <Group display="flex" justify="right">
        <Button
          display={isHeaderReady ? 'none' : 'inherit'}
          disabled={!(user && pwd)}
          onClick={onSave}
        >
          Save
        </Button>
        <Button display={!isHeaderReady ? 'none' : 'inherit'} onClick={clearAuthState}>
          Change
        </Button>
        <Button onClick={onClose}>Close</Button>
      </Group>
    </>
  );
};

export default BasicModal;
