import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchLogs } from '../queries/logs';
import { useAuthProperties } from './useAuthProperties';
import { useSearchForm } from './useSearchForm';
import { useWatch } from 'react-hook-form';

const useQueryLogs = () => {
  const { fetchInfo, isHeaderReady } = useAuthProperties();
  const { getValues } = useSearchForm();

  const currentDbKey = useWatch({ name: 'table' })
  const entriesPerPage = useWatch({ name: 'entriesPerPage' })
  const page = useWatch({ name: 'page' })
  const sortBy = useWatch({ name: 'sortBy' })
  const sortOn = useWatch({ name: 'sortOn' })

  return useQuery({
    enabled: true,
    queryKey: ['get-logs', entriesPerPage, page, sortBy, sortOn, currentDbKey],
    queryFn: async () => {
      if (!isHeaderReady) return null;

      return currentDbKey
        ? await fetchLogs(getValues(), fetchInfo.headers, fetchInfo.routePrefix)
        : null;
    },
    placeholderData: keepPreviousData,
    refetchOnMount: false,
    refetchOnWindowFocus: false,
    retry: false,
  });
};

export default useQueryLogs;
