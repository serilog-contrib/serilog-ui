import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';

export const fetchKeys = async (fetchOptions: RequestInit) => {
  const url = `${determineHost}/api/keys`;

  try {
    const req = await fetch(url, fetchOptions);

    if (req.ok) return await (req.json() as Promise<string[]>);

    return await Promise.reject(new Error('Failed to fetch.'));
  } catch (error) {
    console.warn(error);
    const err = error as Error & { status?: number; message?: string };

    err?.status === 403 ? send403Notification() : sendUnexpectedNotification(err.message);
    return [];
  }
};
