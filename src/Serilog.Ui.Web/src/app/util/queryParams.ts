import { LogLevel, type SearchForm, SortDirectionOptions, SortPropertyOptions } from '../../types/types';
import { searchFormInitialValues } from '../hooks/useSearchForm';

/**
 * Parses URL search parameters and converts them into a SearchForm object
 */
export const parseSearchParams = (searchParams: URLSearchParams): Partial<SearchForm> => {
  const result: Partial<SearchForm> = {};

  // Parse table (key parameter)
  const table = searchParams.get('table') || searchParams.get('key');
  if (table) {
    result.table = table;
  }

  // Parse level
  const level = searchParams.get('level');
  if (level && Object.values(LogLevel).includes(level as LogLevel)) {
    result.level = level as LogLevel;
  }

  // Parse search text
  const search = searchParams.get('search');
  if (search) {
    result.search = search;
  }

  // Parse start date
  const startDate = searchParams.get('startDate') || searchParams.get('from');
  if (startDate) {
    const date = new Date(startDate);
    if (!isNaN(date.getTime())) {
      result.startDate = date;
    }
  }

  // Parse end date
  const endDate = searchParams.get('endDate') || searchParams.get('to') || searchParams.get('till');
  if (endDate) {
    const date = new Date(endDate);
    if (!isNaN(date.getTime())) {
      result.endDate = date;
    }
  }

  // Parse sortOn
  const sortOn = searchParams.get('sortOn');
  if (sortOn && Object.values(SortPropertyOptions).includes(sortOn as SortPropertyOptions)) {
    result.sortOn = sortOn as SortPropertyOptions;
  }

  // Parse sortBy
  const sortBy = searchParams.get('sortBy');
  if (sortBy && Object.values(SortDirectionOptions).includes(sortBy as SortDirectionOptions)) {
    result.sortBy = sortBy as SortDirectionOptions;
  }

  // Parse page
  const page = searchParams.get('page');
  if (page) {
    const pageNum = parseInt(page, 10);
    if (!isNaN(pageNum) && pageNum > 0) {
      result.page = pageNum;
    }
  }

  // Parse entries per page
  const entriesPerPage = searchParams.get('count') || searchParams.get('entriesPerPage');
  if (entriesPerPage) {
    const count = parseInt(entriesPerPage, 10);
    // Validate it's a positive number
    if (!isNaN(count) && count > 0) {
      result.entriesPerPage = entriesPerPage;
    }
  }

  return result;
};

/**
 * Serializes a SearchForm object into URL search parameters
 */
export const serializeSearchParams = (form: SearchForm): URLSearchParams => {
  const params = new URLSearchParams();

  if (form.table) {
    params.set('table', form.table);
  }

  if (form.level) {
    params.set('level', form.level);
  }

  if (form.search) {
    params.set('search', form.search);
  }

  if (form.startDate) {
    params.set('startDate', form.startDate.toISOString());
  }

  if (form.endDate) {
    params.set('endDate', form.endDate.toISOString());
  }

  if (form.sortOn && form.sortOn !== searchFormInitialValues.sortOn) {
    params.set('sortOn', form.sortOn);
  }

  if (form.sortBy && form.sortBy !== searchFormInitialValues.sortBy) {
    params.set('sortBy', form.sortBy);
  }

  if (form.page && form.page !== searchFormInitialValues.page) {
    params.set('page', form.page.toString());
  }

  if (form.entriesPerPage && form.entriesPerPage !== searchFormInitialValues.entriesPerPage) {
    params.set('count', form.entriesPerPage);
  }

  return params;
};
