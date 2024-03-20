import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { isObjectGuard } from '../util/guards';
import { useQueryHeaders } from './useQueryHeaders';
import { useSearchForm } from './useSearchForm';
import { useSerilogUiProps } from './useSerilogUiProps';

const useQueryLogs = () => {
  const { routePrefix } = useSerilogUiProps();
  const { headers: requestInit } = useQueryHeaders();
  const { getValues } = useSearchForm();
  const searchValues = getValues();

  return useQuery({
    enabled: false,
    queryKey: ['get-logs'],
    queryFn: async () => {
      return isObjectGuard(searchValues) && searchValues.table
        ? await fetchLogs(searchValues, requestInit, routePrefix)
        : null;
    },
    placeholderData: keepPreviousData,
    refetchOnMount: true,
    refetchOnWindowFocus: false,
    retry: 3,
  });
};

export default useQueryLogs;
