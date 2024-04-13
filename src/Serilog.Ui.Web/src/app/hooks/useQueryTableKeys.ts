import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { useQueryHeaders } from './useQueryHeaders';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryTableKeys = () => {
  const { routePrefix } = useSerilogUiProps();
  const { headers: requestInit, authHeader } = useQueryHeaders();

  return useQuery({
    queryKey: ['get-keys', authHeader, routePrefix],
    queryFn: async () => {
      return routePrefix ? await fetchKeys(requestInit(), routePrefix) : [];
    },
    refetchOnMount: false,
    refetchOnReconnect: false,
    refetchOnWindowFocus: false,
    retry: false,
  });
};
