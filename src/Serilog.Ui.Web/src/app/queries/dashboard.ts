import { determineHost, send403Notification, sendUnexpectedNotification } from '../util/queries';
import { UiApiError } from './errors';

export interface DashboardData {
  totalLogs: number;
  logsByLevel: Record<string, number>;
  todayLogs: number;
  todayErrorLogs: number;
}

const defaultDashboard: DashboardData = {
  totalLogs: 0,
  logsByLevel: {},
  todayLogs: 0,
  todayErrorLogs: 0,
};

export const fetchDashboard = async (
  fetchOptions: RequestInit,
  routePrefix?: string,
): Promise<DashboardData> => {
  try {
    const url = `${determineHost(routePrefix)}/api/dashboard`;
    const req = await fetch(url, fetchOptions);

    if (req.ok) return await (req.json() as Promise<DashboardData>);

    return await Promise.reject(new UiApiError(req.status, 'Failed to fetch dashboard'));
  } catch (error: unknown) {
    const err = error as UiApiError;
    if (err?.code === 403) {
      send403Notification();
    } else {
      sendUnexpectedNotification(err.message);
    }

    return defaultDashboard;
  }
};