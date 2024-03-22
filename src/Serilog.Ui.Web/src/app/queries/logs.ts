import dayjs from 'dayjs';
import { type SearchForm, type SearchResult } from '../../types/types';
import { isDefinedGuard, isStringGuard } from '../util/guards';
import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';

export const fetchLogs = async (
  values: SearchForm,
  fetchOptions: RequestInit,
  routePrefix?: string,
) => {
  const prepareUrl = prepareSearchUrl(values, routePrefix);
  if (!prepareUrl.areDatesAdmitted) return;

  try {
    const req = await fetch(prepareUrl.url, fetchOptions);

    if (req.ok) return await (req.json() as Promise<SearchResult>);

    return await Promise.reject(new Error('Failed to fetch.'));
  } catch (error: unknown) {
    console.warn(error);
    const err = error as Error & { status?: number; message?: string };
    err?.status === 403 ? send403Notification() : sendUnexpectedNotification(err.message);
  }
};

const prepareSearchUrl = (input: SearchForm, routePrefix?: string) => {
  const { entriesPerPage: count, page, table: key, ...inputData } = { ...input };

  const startDate = inputData.startDate;
  const endDate = inputData.endDate;
  if (
    isDefinedGuard(startDate) &&
    isDefinedGuard(endDate) &&
    dayjs(startDate).isAfter(dayjs(endDate))
  ) {
    sendUnexpectedNotification(
      'Start date cannot be greater than end date',
      'Invalid data',
      'yellow',
    );

    return { areDatesAdmitted: false, url: '' };
  }

  const url = `${determineHost(routePrefix)}/api/logs?page=${page}&count=${count}`;

  const startAsString = startDate?.toISOString() ?? '';
  inputData['startDate'] = startAsString as unknown as Date;
  const endAsString = endDate?.toISOString() ?? '';
  inputData['endDate'] = endAsString as unknown as Date;
  inputData['key'] = key;

  const urlWithOptionalParams = Object.keys(inputData).reduce(
    (prev, curr) => `${prev}${queryParamIfSet(curr, inputData[curr])}`,
    url,
  );

  return { areDatesAdmitted: true, url: urlWithOptionalParams };
};

const queryParamIfSet = (paramName: string, paramValue?: string | null) =>
  isDefinedGuard(paramValue) && isStringGuard(paramValue)
    ? `&${paramName}=${paramValue}`
    : '';
