import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { isArrayGuard } from 'app/util/guards';
import { useAuthProperties } from './useAuthProperties';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryTableKeys = (shouldNotify = false) => {
  const { blockHomeAccess, setAuthenticatedFromAccessDenied } = useSerilogUiProps();
  const { authHeader, isHeaderReady, fetchInfo } = useAuthProperties();

  return useQuery({
    queryKey: ['get-keys', fetchInfo.routePrefix, authHeader],
    queryFn: async () => {
      if (!isHeaderReady) return [];

      if (fetchInfo?.routePrefix === undefined) return [];

      const result = await fetchKeys(
        fetchInfo.headers,
        fetchInfo.routePrefix,
        shouldNotify,
      );

      if (blockHomeAccess) {
        setAuthenticatedFromAccessDenied(isArrayGuard(result));
      }
      return result;
    },
    refetchOnMount: false,
    refetchOnReconnect: false,
    refetchOnWindowFocus: false,
    retry: false,
  });
};
