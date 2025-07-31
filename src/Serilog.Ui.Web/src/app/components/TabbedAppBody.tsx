import { Box, Tabs } from '@mantine/core';
import { IconChartBar, IconTable } from '@tabler/icons-react';
import { useJwtTimeout } from 'app/hooks/useJwtTimeout';
import { useState, Suspense, lazy } from 'react';
import classes from 'style/table.module.css';

const Search = lazy(() => import('./Search/Search'));
const Paging = lazy(() => import('./Search/Paging'));
const SerilogResults = lazy(() => import('./Table/SerilogResults'));
const SerilogResultsMobile = lazy(() => import('./Table/SerilogResultsMobile'));
const Dashboard = lazy(() => import('./Dashboard'));

const TabbedAppBody = ({ hideMobileResults }: { hideMobileResults?: boolean }) => {
  useJwtTimeout();
  const [activeTab, setActiveTab] = useState<string | null>('logs');

  return (
    <Box p="md">
      <Tabs value={activeTab} onChange={setActiveTab}>
        <Tabs.List>
          <Tabs.Tab value="logs" leftSection={<IconTable size={16} />}>
            Logs
          </Tabs.Tab>
          <Tabs.Tab value="dashboard" leftSection={<IconChartBar size={16} />}>
            Dashboard
          </Tabs.Tab>
        </Tabs.List>

        <Tabs.Panel value="logs">
          <Box mt="md">
            <Box visibleFrom="lg">
              <Suspense>
                <Search />
              </Suspense>
            </Box>
            <Box
              display={hideMobileResults ? 'none' : 'block'}
              hiddenFrom="md"
              className={classes.mobileTableWrapper}
            >
              <Suspense>
                <SerilogResultsMobile />
              </Suspense>
            </Box>
            <Box>
              <Box visibleFrom="md" m="xl">
                <Suspense>
                  <SerilogResults />
                </Suspense>
              </Box>
              <Suspense>
                <Paging />
              </Suspense>
            </Box>
          </Box>
        </Tabs.Panel>

        <Tabs.Panel value="dashboard">
          <Box mt="md">
            <Suspense>
              <Dashboard />
            </Suspense>
          </Box>
        </Tabs.Panel>
      </Tabs>
    </Box>
  );
};

export default TabbedAppBody;