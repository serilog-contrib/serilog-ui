import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { useQueryHeaders } from './useQueryHeaders';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryTableKeys = () => {
  const { routePrefix } = useSerilogUiProps();
  const { headers: requestInit, authHeader } = useQueryHeaders();

  return useQuery<string[]>({
    queryKey: ['get-keys', authHeader],
    queryFn: async () => {
      return await fetchKeys(requestInit, routePrefix);
    },
    refetchOnMount: false,
    refetchOnReconnect: false,
    refetchOnWindowFocus: false,
  });
};
