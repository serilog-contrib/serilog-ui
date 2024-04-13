import { Button, Fieldset, Group, PasswordInput } from '@mantine/core';
import { useState, type ChangeEvent } from 'react';
import { useAuthProperties } from '../../hooks/useAuthProperties';
import { isStringGuard } from '../../util/guards';

const JwtModal = ({ onClose }: { onClose: () => void }) => {
  const { authHeader, clearAuthState, jwt_bearerToken, saveAuthState } =
    useAuthProperties();
  const [currentInput, setCurrentInput] = useState(jwt_bearerToken ?? '');

  const isHeaderReady = isStringGuard(authHeader);

  const onSave = async () => {
    saveAuthState({ jwt_bearerToken: currentInput });
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
          value={currentInput}
          disabled={isHeaderReady}
          withAsterisk
          autoComplete="off"
          onChange={(event: ChangeEvent<HTMLInputElement>) => {
            setCurrentInput(event.currentTarget.value);
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
