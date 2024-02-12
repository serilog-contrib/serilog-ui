import loadable from '@loadable/component';
import { Anchor, Badge, Box, Button } from '@mantine/core';
import { IconHomeDot } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { currentYear } from 'app/util/dates';
import { serilogUiUrl } from 'app/util/prettyPrints';
import styles from 'style/header.module.css';
import { isStringGuard } from '../../util/guards';

const FilterButton = loadable(() => import('./FilterButton'));
const AuthorizeButton = loadable(() => import('../Authorization/AuthorizeButton'));
const Paging = loadable(() => import('../Search/Paging'));

const Sidebar = () => {
  const { homeUrl } = useSerilogUiProps();

  return (
    <Box hiddenFrom="sm" h="100%" display="flex" style={{ flexDirection: 'column' }}>
      <Box h="100%">
        <Button.Group
          orientation="vertical"
          display="flex"
          style={{ alignItems: 'center' }}
        >
          <Button
            size="compact-md"
            className={styles.sidebarBoxNavlink}
            component={isStringGuard(homeUrl) ? 'a' : 'button'}
            href={isStringGuard(homeUrl) ? homeUrl : ''}
            justify="center"
            variant="light"
            leftSection={<IconHomeDot size="1.3rem" stroke={1.4} />}
            target="_blank"
          >
            Home
          </Button>
          <Box
            display="grid"
            fz="xs"
            style={{ gridTemplateColumns: 'repeat(2, 1fr)', gap: '1.2em' }}
          >
            <AuthorizeButton />
            <FilterButton />
          </Box>
        </Button.Group>
      </Box>
      <Box>
        <Paging />
        <Box display="flex">
          <Anchor m="0 auto" href={serilogUiUrl} target="_blank">
            <Badge size="md">Serilog Ui | {currentYear}</Badge>
          </Anchor>
        </Box>
      </Box>
    </Box>
  );
};

export default Sidebar;
