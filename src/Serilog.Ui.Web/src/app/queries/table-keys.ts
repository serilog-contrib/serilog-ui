import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';
import { UiApiError } from './errors';

export const fetchKeys = async (fetchOptions: RequestInit, routePrefix?: string) => {
  const url = `${determineHost(routePrefix)}/api/keys`;

  const req = await fetch(url, fetchOptions);

  if (req.ok) return await (req.json() as Promise<string[]>);

  req?.status === 403
    ? send403Notification()
    : sendUnexpectedNotification('Failed to fetch');

  return await Promise.reject(new UiApiError(req.status, 'Failed to fetch'));
};
