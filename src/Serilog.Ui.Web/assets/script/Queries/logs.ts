import { isAfter } from 'date-fns';
import { type SearchResult, type SearchForm } from '../../types/types';
import { isDefinedGuard, isStringGuard } from '../util/guards';
import { createAuthHeaders, determineHost } from '../util/queries';
import { AuthProperties } from '../Authorization/AuthProperties';

export const fetchLogs = async (values: SearchForm) => {
  console.log(values, values.page);

  const prepareUrl = prepareSearchUrl(values, values.page);
  if (!prepareUrl.areDatesAdmitted) return;

  const authProps = new AuthProperties();

  try {
    // TODO: test auth
    const req = await fetch(prepareUrl.url, createAuthHeaders(authProps));
    console.log(req)
    if (req.ok) return (await req.json()) as SearchResult;

    return await Promise.reject(new Error('Failed to fetch.'));
  } catch (error) {
    console.warn(error);
    if (error.status === 403) {
      alert(
        "You are not authorized you to access logs.\r\nYou are not logged in or you don't have enough permissions to perform the requested operation.",
      );
      return;
    }
    alert(error.message);
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
    if (isAfter(startDate, endDate)) {
      alert('Start date cannot be greater than end date');
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
