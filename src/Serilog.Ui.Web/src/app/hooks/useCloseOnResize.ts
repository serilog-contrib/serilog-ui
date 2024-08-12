import { useEffect } from 'react';

export const useCloseOnResize = (close: () => void) => {
  useEffect(() => {
    window.addEventListener('resize', close);

    return () => window.removeEventListener('resize', close);
  }, [close]);
};
