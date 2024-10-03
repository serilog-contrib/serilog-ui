import { isArrayGuard } from 'app/util/guards';
import { useEffect } from 'react';
import { useAuthProperties } from './useAuthProperties';
import { useQueryTableKeys } from './useQueryTableKeys';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useQueryAuth = () => {
  const { blockHomeAccess, setAuthenticatedFromAccessDenied } = useSerilogUiProps();
  const { isHeaderReady } = useAuthProperties();
  const { refetch } = useQueryTableKeys();

  useEffect(() => {
    if (!blockHomeAccess) return;

    if (!isHeaderReady) {
      setAuthenticatedFromAccessDenied(false);
      return;
    }

    void refetch().then(({ data }) => {
      setAuthenticatedFromAccessDenied(isArrayGuard(data));
    });
  }, [blockHomeAccess, isHeaderReady, refetch, setAuthenticatedFromAccessDenied]);
};
