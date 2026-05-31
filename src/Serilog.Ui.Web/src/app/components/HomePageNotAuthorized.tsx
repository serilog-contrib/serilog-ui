import { Box, Text } from '@mantine/core';
import { useQueryAuth } from 'app/hooks/useQueryAuth';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { queryParamsStatePreserverKey } from 'app/util/queryParams';
import { Navigate, useLocation } from 'react-router';
import AuthorizeButton from './Authorization/AuthorizeButton';

const getPreviousQuery = (state: object) => {
  if (!state || !(queryParamsStatePreserverKey in state)) {
    return '';
  }
  const previousQuery = state[queryParamsStatePreserverKey] as string;
  return previousQuery.startsWith('?') ? previousQuery : `?${previousQuery}`;
};

export const HomePageNotAuthorized = () => {
  const { authenticatedFromAccessDenied, blockHomeAccess } =
    useSerilogUiProps();
  useQueryAuth();
  const location = useLocation();

  if (!blockHomeAccess || authenticatedFromAccessDenied) {
    const returnToHomePath = `/${getPreviousQuery(location.state)}`;
    return <Navigate to={returnToHomePath} replace />;
  }

  return (
    <Box
      display='grid'
      w='100%'
      h='100vh'
      style={{
        justifyItems: 'center',
        alignItems: 'center',
        alignContent: 'center',
        gap: '1em',
      }}>
      <Text size='xl' fw='bold'>
        You&apos;re not authorized to access the logs homepage!
      </Text>
      <Box h='100%'>
        <AuthorizeButton />
      </Box>
    </Box>
  );
};
