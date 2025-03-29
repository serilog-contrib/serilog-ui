import { Box, Button, Text } from '@mantine/core';
import { IconArrowBarRight, IconBackspace, IconMoodSad } from '@tabler/icons-react';
import { serilogUiUrl } from 'app/util/prettyPrints';
import { Link, isRouteErrorResponse, useRouteError } from 'react-router';

export const ErrorBoundaryPage = () => {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    return (
      <Box
        display="grid"
        w="100%"
        h="100vh"
        style={{
          justifyItems: 'center',
          alignItems: 'center',
          alignContent: 'center',
          gap: '1em',
        }}
      >
        <Text size="xl" fw="bold" display="flex" style={{ alignItems: 'center' }}>
          {error.status >= 500 ? 'An unexpected error occurred!' : 'Page not found!'}
          <IconMoodSad />
        </Text>
        <Link to={`/`} style={{ textDecoration: 'none' }}>
          <Button size="lg" display="block">
            <IconBackspace />
            <Text size="lg" p="0.6em" fw="bold">
              Home
            </Text>
          </Button>
        </Link>
      </Box>
    );
  }

  return (
    <Box
      display="grid"
      w="100%"
      h="100vh"
      style={{
        justifyItems: 'center',
        alignItems: 'center',
        alignContent: 'center',
        gap: '1em',
      }}
    >
      <div>
        <Text
          ta="justify"
          display="flex"
          mb="sm"
          style={{ justifyContent: 'center', alignItems: 'center' }}
        >
          An unexpected error occurred!!! <IconMoodSad />
        </Text>
        <Text ta="justify" display="flex" mb="sm">
          If you know how to replicate the issue, please open an issue to
        </Text>
        <Text
          display="flex"
          fz="lg"
          style={{ justifyContent: 'center', alignItems: 'center' }}
        >
          <Link target="_blank" to={serilogUiUrl}>
            serilog-ui
          </Link>
          <IconArrowBarRight fontSize="0.4em" />
        </Text>
      </div>
    </Box>
  );
};
