import { AppShell, Burger, useMantineTheme } from '@mantine/core';
import { type Dispatch, type SetStateAction } from 'react';
import AuthorizeButton from '../Authorization/AuthorizeButton';

const Head = ({
  isOpen,
  toggleOpen,
}: {
  isOpen: boolean;
  toggleOpen: Dispatch<SetStateAction<boolean>>;
}) => {
  const theme = useMantineTheme();
  return (
    <AppShell.Header visibleFrom="sm" p="md">
      <div style={{ display: 'flex', alignItems: 'center', height: '100%' }}>
        <Burger
          opened={isOpen}
          onClick={() => {
            toggleOpen((o) => !o);
          }}
          size="sm"
          color={theme.colors.gray[6]}
          mr="xl"
        />
        <AuthorizeButton />
      </div>
    </AppShell.Header>
  );
};

export default Head;
