import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { useAuthProperties } from './useAuthProperties';
import { useSearchForm } from './useSearchForm';

const useQueryLogs = () => {
  const { fetchInfo, isHeaderReady } = useAuthProperties();
  const { getValues, watch } = useSearchForm();
  const currentDbKey = watch('table');

  return useQuery({
    enabled: false,
    queryKey: ['get-logs'],
    queryFn: async () => {
      if (!isHeaderReady) return null;
      
      return currentDbKey
        ? await fetchLogs(getValues(), fetchInfo.headers, fetchInfo.routePrefix)
        : null;
    },
    placeholderData: keepPreviousData,
    refetchOnMount: true,
    refetchOnWindowFocus: false,
    retry: false,
  });
};

export default useQueryLogs;
