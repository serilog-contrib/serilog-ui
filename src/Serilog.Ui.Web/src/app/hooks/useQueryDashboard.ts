import { useQuery } from '@tanstack/react-query';
import { useAuthProperties } from './useAuthProperties';
import { fetchDashboard } from '../queries/dashboard';

const useQueryDashboard = () => {
  const { fetchInfo, isHeaderReady } = useAuthProperties();

  return useQuery({
    enabled: isHeaderReady,
    queryKey: ['get-dashboard'],
    queryFn: async () => {
      if (!isHeaderReady) return null;
      return await fetchDashboard(fetchInfo.headers, fetchInfo.routePrefix);
    },
    refetchOnMount: false,
    refetchOnWindowFocus: false,
    retry: false,
  });
};

export default useQueryDashboard;