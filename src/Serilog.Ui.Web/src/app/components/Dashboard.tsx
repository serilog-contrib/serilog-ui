import { Box, Card, Grid, Text, Title, Group, Stack } from '@mantine/core';
import { IconActivity, IconAlertTriangle, IconCalendar, IconChartBar } from '@tabler/icons-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import useQueryDashboard from '../hooks/useQueryDashboard';

const LEVEL_COLORS = {
  Verbose: '#868e96',
  Debug: '#4c6ef5',
  Information: '#12b886',
  Warning: '#fd7e14',
  Error: '#fa5252',
  Fatal: '#c92a2a',
  Unknown: '#adb5bd',
};

const StatCard = ({ icon, title, value, color }: { 
  icon: React.ReactNode; 
  title: string; 
  value: number; 
  color: string;
}) => (
  <Card shadow="sm" padding="lg" radius="md" withBorder>
    <Group justify="space-between">
      <Stack gap="xs">
        <Text size="sm" c="dimmed">
          {title}
        </Text>
        <Text size="xl" fw={700} c={color}>
          {value.toLocaleString()}
        </Text>
      </Stack>
      <Box c={color}>
        {icon}
      </Box>
    </Group>
  </Card>
);

export const Dashboard = () => {
  const { data: dashboard, isLoading, error } = useQueryDashboard();

  if (isLoading) {
    return (
      <Box p="md">
        <Text>Loading dashboard...</Text>
      </Box>
    );
  }

  if (error || !dashboard) {
    return (
      <Box p="md">
        <Text c="red">Error loading dashboard data</Text>
      </Box>
    );
  }

  // Prepare data for charts
  const levelData = Object.entries(dashboard.logsByLevel).map(([level, count]) => ({
    level,
    count,
    color: LEVEL_COLORS[level as keyof typeof LEVEL_COLORS] || LEVEL_COLORS.Unknown,
  }));

  return (
    <Box p="md">
      <Title order={2} mb="lg">
        Log Dashboard
      </Title>

      {/* Stats Cards */}
      <Grid gutter="md" mb="xl">
        <Grid.Col span={{ base: 12, md: 3 }}>
          <StatCard
            icon={<IconChartBar size={32} />}
            title="Total Logs"
            value={dashboard.totalLogs}
            color="blue"
          />
        </Grid.Col>
        <Grid.Col span={{ base: 12, md: 3 }}>
          <StatCard
            icon={<IconCalendar size={32} />}
            title="Today's Logs"
            value={dashboard.todayLogs}
            color="green"
          />
        </Grid.Col>
        <Grid.Col span={{ base: 12, md: 3 }}>
          <StatCard
            icon={<IconAlertTriangle size={32} />}
            title="Today's Errors"
            value={dashboard.todayErrorLogs}
            color="red"
          />
        </Grid.Col>
        <Grid.Col span={{ base: 12, md: 3 }}>
          <StatCard
            icon={<IconActivity size={32} />}
            title="Log Levels"
            value={Object.keys(dashboard.logsByLevel).length}
            color="grape"
          />
        </Grid.Col>
      </Grid>

      {/* Charts */}
      <Grid gutter="md">
        <Grid.Col span={{ base: 12, md: 8 }}>
          <Card shadow="sm" padding="lg" radius="md" withBorder>
            <Title order={4} mb="md">
              Logs by Level
            </Title>
            <Box h={300}>
              <ResponsiveContainer width="100%" height="100%">
                <BarChart data={levelData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="level" />
                  <YAxis />
                  <Tooltip />
                  <Bar dataKey="count" fill="#4c6ef5" />
                </BarChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid.Col>
        <Grid.Col span={{ base: 12, md: 4 }}>
          <Card shadow="sm" padding="lg" radius="md" withBorder>
            <Title order={4} mb="md">
              Level Distribution
            </Title>
            <Box h={300}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={levelData}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={({ level, percent }) => `${level} ${((percent || 0) * 100).toFixed(0)}%`}
                    outerRadius={80}
                    fill="#8884d8"
                    dataKey="count"
                  >
                    {levelData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip />
                </PieChart>
              </ResponsiveContainer>
            </Box>
          </Card>
        </Grid.Col>
      </Grid>
    </Box>
  );
};

export default Dashboard;