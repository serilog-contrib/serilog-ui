import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { useQueryHeaders } from './useQueryHeaders';

export const useQueryTableKeys = () => {
  const requestInit = useQueryHeaders();

  return useQuery<string[]>({
    queryKey: ['get-keys', requestInit],
    queryFn: async () => {
      return await fetchKeys(requestInit);
    },
  });
};
