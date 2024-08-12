import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';
import { UiApiError } from './errors';

export const fetchKeys = async (
  fetchOptions: RequestInit,
  routePrefix?: string,
  notify: boolean = false,
) => {
  const url = `${determineHost(routePrefix)}/api/keys`;

  const req = await fetch(url, fetchOptions);

  if (req.ok) return await (req.json() as Promise<string[]>);

  const reject = () => Promise.reject(new UiApiError(req.status, 'Failed to fetch'));

  if (!notify) return await reject();

  if (req?.status === 403) {
    send403Notification();
  } else {
    sendUnexpectedNotification('Failed to fetch');
  }

  return await reject();
};
