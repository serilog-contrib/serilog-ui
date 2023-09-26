import { Header, MediaQuery, Burger, useMantineTheme } from '@mantine/core';
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
    <MediaQuery largerThan="sm" styles={{ display: 'none' }}>
      <Header height={{ base: 50, sm: 0 }} p="md">
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
      </Header>
    </MediaQuery>
  );
};

export default Head;
