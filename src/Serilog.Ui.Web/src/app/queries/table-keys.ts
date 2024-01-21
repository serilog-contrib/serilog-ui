import {
  createAuthHeaders as createDefaultRequestInit,
  determineHost,
} from '../util/queries';

export const fetchKeys = async (getAuthHeader: () => string | undefined) => {
  const url = `${determineHost}/api/keys`;

  const requestInit = createDefaultRequestInit(getAuthHeader());

  try {
    const req = await fetch(url, requestInit);

    if (req.ok) return await (req.json() as Promise<string[]>);

    return await Promise.reject(new Error('Failed to fetch.'));
  } catch (error) {
    console.warn(error);
    const err = error as Error & { status?: number; message?: string };

    if (err?.status === 403) {
      alert(
        "You are not authorized you to access logs.\r\nYou are not logged in or you don't have enough permissions to perform the requested operation.",
      );
      return [] as string[];
    }
    alert(err?.message);
    return [] as string[];
  }
};
