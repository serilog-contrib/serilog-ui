import { type AuthProperties } from '../Authorization/AuthProperties';
import {
  createAuthHeaders as createDefaultRequestInit,
  determineHost,
} from '../util/queries';

export const fetchKeys = async (authProps: AuthProperties) => {
  const url = `${determineHost}/api/keys`;

  const requestInit = createDefaultRequestInit(authProps);

  try {
    const req = await fetch(url, requestInit);

    if (req.ok) return await (req.json() as Promise<string[]>);

    return await Promise.reject(new Error('Failed to fetch.'));
  } catch (error) {
    console.warn(error);
    if (error.status === 403) {
      alert(
        "You are not authorized you to access logs.\r\nYou are not logged in or you don't have enough permissions to perform the requested operation.",
      );
      return [] as string[];
    }
    alert(error.message);
    return [] as string[];
  }
};
