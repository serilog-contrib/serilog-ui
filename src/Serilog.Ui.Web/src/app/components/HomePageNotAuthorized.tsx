import { Box, Text } from '@mantine/core';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { Navigate } from 'react-router-dom';
import AuthorizeButton from './Authorization/AuthorizeButton';

export const HomePageNotAuthorized = () => {
  const { authenticatedFromAccessDenied, blockHomeAccess, routePrefix } =
    useSerilogUiProps();

  if (!routePrefix) return null;

  if (!blockHomeAccess || authenticatedFromAccessDenied)
    return <Navigate to={`/${routePrefix}/`} replace />;

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
      <Text size="xl" fw="bold">
        You&apos;re not authorized to access the logs homepage!
      </Text>
      <Box h="100%">
        <AuthorizeButton />
      </Box>
    </Box>
  );
};
