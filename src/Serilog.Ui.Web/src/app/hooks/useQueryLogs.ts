import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { useWatch } from 'react-hook-form';
import { fetchLogs } from '../queries/logs';
import { useAuthProperties } from './useAuthProperties';
import { useLiveRefresh } from './useLiveRefresh';
import { useSearchForm } from './useSearchForm';

const useQueryLogs = () => {
  const { fetchInfo, isHeaderReady } = useAuthProperties();
  const { getValues } = useSearchForm();
  const {
    isLiveRefreshRunning,
    liveRefreshLabel,
    refetchInterval,
    startLiveRefresh,
    stopLiveRefresh,
  } = useLiveRefresh();

  const currentDbKey = useWatch({ name: 'table' });
  const entriesPerPage = useWatch({ name: 'entriesPerPage' });
  const page = useWatch({ name: 'page' });
  const sortBy = useWatch({ name: 'sortBy' });
  const sortOn = useWatch({ name: 'sortOn' });

  return {
    ...useQuery({
      enabled: true,
      queryKey: ['get-logs', entriesPerPage, page, sortBy, sortOn, currentDbKey],
      queryFn: async () => {
        if (!isHeaderReady) return null;
        const values = getValues();

        return currentDbKey
          ? await fetchLogs(values, fetchInfo.headers, fetchInfo.routePrefix)
          : null;
      },
      placeholderData: keepPreviousData,
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      retry: false,
      refetchInterval,
    }),
    isLiveRefreshRunning,
    liveRefreshLabel,
    startLiveRefresh,
    stopLiveRefresh,
  };
};

export default useQueryLogs;
