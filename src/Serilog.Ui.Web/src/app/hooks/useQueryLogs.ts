import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { useQueryHeaders } from './useQueryHeaders';
import { useSearchForm } from './useSearchForm';
import { useSerilogUiProps } from './useSerilogUiProps';

const useQueryLogs = () => {
  const { routePrefix } = useSerilogUiProps();
  const { headers: requestInit } = useQueryHeaders();
  const { getValues, watch } = useSearchForm();
  const currentDbKey = watch('table');

  return useQuery({
    enabled: false,
    queryKey: ['get-logs'],
    queryFn: async () => {
      return currentDbKey
        ? await fetchLogs(getValues(), requestInit(), routePrefix)
        : null;
    },
    placeholderData: keepPreviousData,
    refetchOnMount: true,
    refetchOnWindowFocus: false,
    retry: false,
  });
};

export default useQueryLogs;
