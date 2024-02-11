import { createAuthHeaders } from 'app/util/queries';
import { useMemo } from 'react';
import { useAuthProperties } from './useAuthProperties';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryHeaders = () => {
  const props = useSerilogUiProps();
  const { authHeader } = useAuthProperties();

  const headers = useMemo(
    () => createAuthHeaders(props, authHeader),
    [authHeader, props],
  );

  return headers;
};
