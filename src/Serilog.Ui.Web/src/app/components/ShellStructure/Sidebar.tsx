import { Box, Button } from '@mantine/core';
import { IconHomeDot } from '@tabler/icons-react';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { Suspense, lazy } from 'react';
import styles from 'style/header.module.css';
import { isStringGuard } from '../../util/guards';
import BrandBadge from './BrandBadge';

const FilterButton = lazy(() => import('./FilterButton'));
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
            <Suspense>
              <AuthorizeButton />
            </Suspense>
            <Suspense>
              <FilterButton />
            </Suspense>
          </Box>
        </Button.Group>
      </Box>
      <Box>
        <Suspense>
          <Paging />
        </Suspense>

        <Box display="flex">
          <BrandBadge size="lg" margin={'0 auto'} />
        </Box>
      </Box>
    </Box>
  );
};

export default Sidebar;
