import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { isObjectGuard } from '../util/guards';
import { useQueryHeaders } from './useQueryHeaders';
import { useSearchForm } from './useSearchForm';

const useQueryLogsHook = () => {
  const requestInit = useQueryHeaders();
  const { getValues } = useSearchForm();
  const searchValues = getValues();

  return useQuery({
    enabled: false,
    queryKey: ['get-logs'],
    queryFn: async () => {
      return isObjectGuard(searchValues)
        ? await fetchLogs(searchValues, requestInit)
        : null;
    },
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
