import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { useMemo } from 'react';
import { fetchLogs } from '../queries/logs';
import { isObjectGuard } from '../util/guards';
import { useSearchForm } from './SearchFormContext';
import { useAuthProperties } from './useAuthProperties';

const useQueryLogsHook = () => {
  const { getAuthHeader } = useAuthProperties();
  const { getValues } = useSearchForm();
  const searchValues = useMemo(() => getValues(), [getValues]);

  return useQuery({
    enabled: false,
    queryKey: ['get-logs'],
    queryFn: async () =>
      isObjectGuard(searchValues) ? await fetchLogs(searchValues, getAuthHeader) : null,
    placeholderData: keepPreviousData,
    // TODO? fetch pre-post page on data fetch
    // onError: (err) => {
    //   console.error(err);
    // }, // TODO: notification box
    refetchOnMount: true,
    refetchOnWindowFocus: false,
    retry: 3,
  });
};

export default useQueryLogsHook;
