import { checkErrors, validateKey } from 'app/util/auth';
import { useEffect, useState } from 'react';
import { AuthType } from 'types/types';
import { useAuthProperties } from './useAuthProperties';
import { useSerilogUiProps } from './useSerilogUiProps';

/**
 * Run validation on JWT token every minute, to let user know if token expires
 */
export function useJwtTimeout(): void {
  const MINUTE = 60000;
  const [errorSet, stopErrorCheck] = useState(false);
  const { authType } = useSerilogUiProps();
  const { jwt_bearerToken } = useAuthProperties();

  // when token changes, validation can resume
  useEffect(() => {
    stopErrorCheck(false);
  }, [jwt_bearerToken]);

  useEffect(() => {
    // if error was already spotted, we don't start checking until jwt is changed
    if (authType !== AuthType.Jwt || !jwt_bearerToken || errorSet) return;

    const id = setTimeout(() => {
      const result = validateKey('jwt_bearerToken', jwt_bearerToken);

      checkErrors(result);
      if (result.error) stopErrorCheck(true);
    }, MINUTE);

    return () => {
      clearTimeout(id);
    };
  }, [authType, errorSet, jwt_bearerToken]);
}
