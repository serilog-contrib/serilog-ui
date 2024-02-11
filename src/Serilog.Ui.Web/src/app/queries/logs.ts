import dayjs from 'dayjs';
import { type SearchForm, type SearchResult } from '../../types/types';
import { isDefinedGuard, isStringGuard } from '../util/guards';
import {
  determineHost,
  send403Notification,
  sendUnexpectedNotification,
} from '../util/queries';

export const fetchLogs = async (values: SearchForm, fetchOptions: RequestInit) => {
  const prepareUrl = prepareSearchUrl(values, values.page);
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

const prepareSearchUrl = (input: SearchForm, identifiedPage?: number) => {
  const {
    startDate,
    endDate,
    table: key,
    entriesPerPage: count,
    level,
    search: searchTerm,
  } = input;
  const page = identifiedPage ?? 1;

  if (isDefinedGuard(startDate) && isDefinedGuard(endDate)) {
    if (dayjs(startDate).isAfter(dayjs(endDate))) {
      sendUnexpectedNotification(
        'Start date cannot be greater than end date',
        'Invalid data',
        'yellow',
      );

      return { areDatesAdmitted: false, url: '' };
    }
  }

  // TODO: review dates parsing
  const startAsString = startDate?.toISOString() ?? '';
  const endAsString = endDate?.toISOString() ?? '';

  // TODO check url creation result when using
  const url = `${determineHost}/api/logs?page=${page}&count=${count}\
${queryParamIfSet('key', key)}\
${queryParamIfSet('level', level)}\
${queryParamIfSet('search', searchTerm)}\
${queryParamIfSet('startDate', startAsString)}\
${queryParamIfSet('endDate', endAsString)}`;

  return { areDatesAdmitted: true, url };
};

const queryParamIfSet = (paramName: string, paramValue?: string | null) =>
  isDefinedGuard(paramValue) && isStringGuard(paramValue)
    ? `&${paramName}=${paramValue}`
    : '';
