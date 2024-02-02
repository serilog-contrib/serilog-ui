import { Anchor, AppShell, Badge, Box, Button, Group } from '@mantine/core';
import { IconHomeDot } from '@tabler/icons-react';
import styles from 'style/header.module.css';
import { isStringGuard } from '../../util/guards';
import AuthorizeButton from '../Authorization/AuthorizeButton';
import { FilterButton } from './FilterButton';

const Sidebar = () => {
  const homeUrl = window.config.homeUrl;

  return (
    <Box
      hiddenFrom="sm"
      display="flex"
      style={{ flexDirection: 'column', alignContent: 'space-between', height: '100%' }}
    >
      <AppShell.Section grow>
        <Box className={styles.sidebarBox}>
          <Button
            size="compact-lg"
            className={styles.sidebarBoxNavlink}
            component={isStringGuard(homeUrl) ? 'a' : 'button'}
            href={isStringGuard(homeUrl) ? homeUrl : ''}
            justify="center"
            variant="transparent"
            autoContrast
            leftSection={<IconHomeDot size="1.3rem" stroke={1.4} />}
            target="_blank"
          >
            Home
          </Button>
          <AuthorizeButton customStyle={styles.sidebarBoxNavlink} />
          <FilterButton />
        </Box>
      </AppShell.Section>
      <AppShell.Section>
        <Group>
          <Anchor
            // TODO: hide on sm size or write alt, define sizes by page size
            href="https://github.com/serilog-contrib/serilog-ui"
            target="_blank"
          >
            <Badge size="md">Serilog Ui | {new Date().getFullYear()}</Badge>
          </Anchor>
        </Group>
      </AppShell.Section>
    </Box>
  );
};

export default Sidebar;
