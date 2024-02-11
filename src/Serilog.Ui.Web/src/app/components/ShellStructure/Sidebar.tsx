import { Anchor, Badge, Box, Button } from '@mantine/core';
import { IconHomeDot } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { currentYear } from 'app/util/dates';
import { serilogUiUrl } from 'app/util/prettyPrints';
import { lazy } from 'react';
import styles from 'style/header.module.css';
import { isStringGuard } from '../../util/guards';
import { FilterButton } from './FilterButton';

const AuthorizeButton = lazy(() => import('../Authorization/AuthorizeButton'));
const Paging = lazy(() => import('../Search/Paging'));

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
            variant="transparent"
            autoContrast
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

        <Paging />
      </Box>
      <Box m="0 auto">
        <Anchor href={serilogUiUrl} target="_blank">
          <Badge size="md">Serilog Ui | {currentYear}</Badge>
        </Anchor>
      </Box>
    </Box>
  );
};

export default Sidebar;
