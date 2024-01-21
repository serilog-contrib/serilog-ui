import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { isObjectGuard } from '../util/guards';
import { useSearchFormContext } from './SearchFormContext';
import { useAuthProperties } from './useAuthProperties';

const useQueryLogsHook = () => {
  const { getAuthHeader } = useAuthProperties();
  const form = useSearchFormContext();

  return useQuery({
    queryKey: ['get-logs', form.values],
    queryFn: async () =>
      isObjectGuard(form.values) ? await fetchLogs(form.values, getAuthHeader) : null,
    placeholderData: keepPreviousData,
    // TODO? fetch pre-post page on data fetch
    // onError: (err) => {
    //   console.error(err);
    // }, // TODO: notification box
    refetchOnWindowFocus: false,
    retry: 1,
  });
};

export default useQueryLogsHook;
