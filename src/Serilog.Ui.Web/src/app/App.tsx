import { ColorSchemeScript, MantineProvider } from '@mantine/core';
import { Notifications } from '@mantine/notifications';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { RouterProvider, createBrowserRouter } from 'react-router';
import { theme } from 'style/theme';
import { AuthPropertiesProvider } from './hooks/useAuthProperties';
import { useSerilogUiProps } from './hooks/useSerilogUiProps';
import { routes } from './routes';

const App = () => {
  const { routePrefix } = useSerilogUiProps();
  const queryClient = new QueryClient();

  const router = createBrowserRouter(routes, { basename: `/${routePrefix}/` });

  if (!routePrefix) return null;

  return (
    <>
      <ColorSchemeScript defaultColorScheme="auto" />
      <MantineProvider defaultColorScheme="auto" theme={theme}>
        <Notifications />
        <QueryClientProvider client={queryClient}>
          <AuthPropertiesProvider>
            <RouterProvider router={router} />
          </AuthPropertiesProvider>
        </QueryClientProvider>
      </MantineProvider>
    </>
  );
};

export default App;
