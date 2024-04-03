import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';
import { UiApiError } from './errors';

export const fetchKeys = async (fetchOptions: RequestInit, routePrefix?: string) => {
  const url = `${determineHost(routePrefix)}/api/keys`;

  try {
    const req = await fetch(url, fetchOptions);

    if (req.ok) return await (req.json() as Promise<string[]>);

    return await Promise.reject(new UiApiError(req.status, 'Failed to fetch'));
  } catch (error) {
    const err = error as UiApiError;

    err?.code === 403 ? send403Notification() : sendUnexpectedNotification(err.message);
    return [];
  }
};
