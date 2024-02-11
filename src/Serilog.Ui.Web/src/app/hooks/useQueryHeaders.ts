import { createRequestInit } from 'app/util/queries';
import { useMemo } from 'react';
import { useAuthProperties } from './useAuthProperties';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryHeaders = () => {
  const props = useSerilogUiProps();
  const { authHeader } = useAuthProperties();

  const headers = useMemo(
    () => createRequestInit(props, authHeader),
    [authHeader, props],
  );

  return { headers, authHeader };
};
