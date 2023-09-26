import {
  ActionIcon,
  Anchor,
  Badge,
  Group,
  NavLink,
  Navbar,
  useMantineColorScheme,
  useMantineTheme,
} from '@mantine/core';
import { IconMoonStars, IconSun } from '@tabler/icons-react';
import AuthorizeButton from '../Authorization/AuthorizeButton';
import { isStringGuard } from '../../util/guards';

const Sidebar = ({ isOpen }: { isOpen: boolean }) => {
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();
  const theme = useMantineTheme();
  const homeUrl = window.config.homeUrl;

  return (
    <Navbar
      // TODO: center all the buttons, correct bg to use dynamic mantine themes, improve all items
      hiddenBreakpoint="sm"
      hidden={!isOpen}
      width={{ sm: 70, md: 150, lg: 220 }}
      p="md"
      bg={colorScheme === 'dark' ? theme.colors.blue[7] : theme.colors.blue[4]}
    >
      <Navbar.Section mt="xs">
        <Group position="center" spacing="xs">
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
      </Navbar.Section>
      <Navbar.Section grow mt="md">
        <Group>
          <NavLink
            label="Home"
            description="Home"
            component={isStringGuard(homeUrl) ? 'a' : 'button'}
            href={isStringGuard(homeUrl) ? homeUrl : ''}
            target="_blank"
            icon={
              <Badge size="xs" variant="filled" color="red" w={16} h={16} p={0}>
                TODO?
              </Badge>
            }
            sx={(theme) => ({
              display: 'block',
              width: '100%',
              padding: theme.spacing.xs,
              borderRadius: theme.radius.sm,
              color: theme.colorScheme === 'dark' ? theme.colors.dark[0] : theme.black,

              '&:hover': {
                backgroundColor:
                  theme.colorScheme === 'dark'
                    ? theme.colors.dark[6]
                    : theme.colors.gray[0],
              },
            })}
          />
          <AuthorizeButton />
        </Group>
      </Navbar.Section>
      <Navbar.Section>
        <Group>
          <Anchor
            // TODO: hide on sm size or write alt, define sizes by page size
            href="https://github.com/serilog-contrib/serilog-ui"
            target="_blank"
          >
            <Badge size="sm">Serilog Ui | {new Date().getFullYear()}</Badge>
          </Anchor>
        </Group>
      </Navbar.Section>
    </Navbar>
  );
};

export default Sidebar;
