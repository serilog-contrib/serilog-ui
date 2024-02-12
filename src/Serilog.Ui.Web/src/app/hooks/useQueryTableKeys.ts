import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { useQueryHeaders } from './useQueryHeaders';

export const useQueryTableKeys = () => {
  const { headers: requestInit, authHeader } = useQueryHeaders();

  return useQuery<string[]>({
    queryKey: ['get-keys', authHeader],
    queryFn: async () => {
      return await fetchKeys(requestInit);
    },
    refetchOnMount: false,
    refetchOnReconnect: false,
    refetchOnWindowFocus: false,
  });
};
