import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { isObjectGuard } from '../util/guards';
import { useAuthProperties } from './useAuthProperties';
import { useSearchForm } from './useSearchForm';

const useQueryLogsHook = () => {
  const { authHeader } = useAuthProperties();
  const { getValues } = useSearchForm();
  const searchValues = getValues();

  return useQuery({
    enabled: false,
    queryKey: ['get-logs', { authHeader }],
    queryFn: async () => {
      return isObjectGuard(searchValues)
        ? await fetchLogs(searchValues, authHeader)
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
