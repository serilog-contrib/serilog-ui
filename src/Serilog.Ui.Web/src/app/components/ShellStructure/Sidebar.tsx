import {
  ActionIcon,
  Anchor,
  AppShell,
  Badge,
  Group,
  NavLink,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { IconMoonStars, IconSun } from '@tabler/icons-react';
import classes from 'style/header.module.css';
import { isStringGuard } from '../../util/guards';
import AuthorizeButton from '../Authorization/AuthorizeButton';

const Sidebar = () => {
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();
  const theme = useMantineTheme();
  const homeUrl = window.config.homeUrl;

  return (
    <AppShell.Navbar
      // TODO: center all the buttons, correct bg to use dynamic mantine themes, improve all items
      p="md"
      bg={colorScheme === 'dark' ? theme.colors.blue[7] : theme.colors.blue[4]}
    >
      <AppShell.Section mt="xs">
        <Group justify="center" gap="xs">
          <ActionIcon
            variant="default"
            onClick={() => {
              toggleColorScheme();
            }}
            size={30}
          >
            {colorScheme === 'dark' ? (
              <IconSun size="1rem" stroke="3" />
            ) : (
              <IconMoonStars size="1rem" stroke="3" />
            )}
          </ActionIcon>
        </Group>
      </AppShell.Section>
      <AppShell.Section grow mt="md">
        <Group>
          <NavLink
            label="Home"
            description="Home"
            component={isStringGuard(homeUrl) ? 'a' : 'button'}
            href={isStringGuard(homeUrl) ? homeUrl : ''}
            target="_blank"
            className={classes.sidebarGroupNavlink}
            leftSection={
              <Badge size="xs" variant="filled" color="red" w={16} h={16} p={0}>
                TODO?
              </Badge>
            }
          />
          <AuthorizeButton />
        </Group>
      </AppShell.Section>
      <AppShell.Section>
        <Group>
          <Anchor
            // TODO: hide on sm size or write alt, define sizes by page size
            href="https://github.com/serilog-contrib/serilog-ui"
            target="_blank"
          >
            <Badge size="sm">Serilog Ui | {new Date().getFullYear()}</Badge>
          </Anchor>
        </Group>
      </AppShell.Section>
    </AppShell.Navbar>
  );
};

export default Sidebar;
