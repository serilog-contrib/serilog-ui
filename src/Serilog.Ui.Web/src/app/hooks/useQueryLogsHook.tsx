import { useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../Queries/logs';
import { useAuthProperties } from './useAuthProperties';
import { useSearchFormContext } from './SearchFormContext';
import { isObjectGuard } from '../util/guards';

const useQueryLogsHook = () => {
  const { authProps } = useAuthProperties();
  const form = useSearchFormContext();

  return useQuery({
    queryKey: ['get-logs', authProps.bearerToken, form.values],
    queryFn: async () =>
      isObjectGuard(form.values)
        ? await fetchLogs(form.values)
        : null,
    keepPreviousData: true,
    // TODO? fetch pre-post page on data fetch
    onError: (err) => {
      console.error(err);
    }, // TODO: notification box
    refetchOnWindowFocus: false,
    retry: 1,
  });
};

export default useQueryLogsHook;
