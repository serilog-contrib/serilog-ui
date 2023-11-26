import { Burger, Button, Modal, useMantineTheme } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { IconFilterSearch } from '@tabler/icons-react';
import { ComponentProps } from 'react';
import AuthorizeButton from '../Authorization/AuthorizeButton';
import { SearchGroup } from '../Search/SearchGroup';
import Head from './Header';

export const HeaderActivity = (props: ComponentProps<typeof Head>) => {
  const theme = useMantineTheme();
  const [opened, { open, close }] = useDisclosure(false);

  return (
    <>
      <Burger
        opened={props.isMobileOpen}
        onClick={props.toggleMobile}
        hiddenFrom="sm"
        size="sm"
        color={theme.colors.gray[6]}
      />

      <AuthorizeButton />

      <Button size="compact-md" visibleFrom="sm" hiddenFrom="lg" onClick={open}>
        <IconFilterSearch />
        Filter
      </Button>
      <Modal opened={opened} onClose={close} title="Search filters" centered>
        <SearchGroup showSearch />
      </Modal>
    </>
  );
};
