import dayjs from 'dayjs';
import { http, HttpResponse } from 'msw';
import {
  AuthType,
  EncodedSeriLogObject,
  LogLevel,
  SearchParameters,
  SortDirectionOptions,
  SortPropertyOptions,
} from '../../../types/types';
import { dbKeysMock, fakeLogs, fakeLogs2ndTable, fakeLogs3rdTable } from './samples';
import { defaultAuthType } from '../../../app/hooks/useSerilogUiProps.tsx';

export const developmentListenersHost = ['https://localhost:3001'];

const tableLogs = (table: string | null) => {
  if (table === dbKeysMock[2]) return fakeLogs3rdTable;
  if (table === dbKeysMock[1]) return fakeLogs2ndTable;
  return fakeLogs;
};

export const handlers = developmentListenersHost.flatMap((dlh) => [
  http.get(`${dlh}/api/logs`, ({ request }) => {
    const auth = request.headers.get('authorization');
    if (defaultAuthType !== AuthType.Custom && !auth) return HttpResponse.error();

    const req = new URL(request.url);
    const params = getSearchParams(req.searchParams);

    const logs = tableLogs(params.table)
      .logs.filter(byLevel(LogLevel[params.level || '']))
      .filter(byDates(params.start || '', params.end || ''))
      .filter(bySearch(params.search || ''))
      .sort(
        byDirection(
          params.sortOn ?? SortPropertyOptions.Timestamp,
          params.sortBy ?? SortDirectionOptions.Desc,
        ),
      );

    const sliceValues = applyLimits(params.count, params.page);

    const data = {
      currentPage: params.page,
      count: params.count,
      total: logs.length,
      logs: logs.slice(sliceValues.start, sliceValues.end),
    };
    return HttpResponse.json(data);
  }),
  http.get(`${dlh}/api/keys`, ({ request }) => {
    const auth = request.headers.get('authorization');

    return defaultAuthType !== AuthType.Custom && !auth ? HttpResponse.error() : HttpResponse.json(dbKeysMock);
  }),
]);

const getSearchParams = (params: URLSearchParams) => ({
  count: Number.parseInt(params.get(SearchParameters.Count) || ''),
  page: Number.parseInt(params.get(SearchParameters.Page) || ''),
  level: params.get(SearchParameters.Level),
  search: params.get(SearchParameters.Search),
  start: params.get(SearchParameters.StartDate),
  end: params.get(SearchParameters.EndDate),
  sortOn: params.get(SearchParameters.SortProperty),
  sortBy: params.get(SearchParameters.SortDirection),
  table: params.get(SearchParameters.Table),
});

const byLevel = (level?: LogLevel) => (item: EncodedSeriLogObject) =>
  level ? item.level === level : true;
const byDates = (start?: string, end?: string) => (item: EncodedSeriLogObject) => {
  if (!start && !end) return true;
  let after = true;
  let before = true;

  const date = dayjs(item.timestamp);
  if (start) {
    const ds = dayjs(start);
    after = date.isAfter(ds);
  }
  if (end) {
    const de = dayjs(end);
    before = date.isBefore(de);
  }
  return after && before;
};
const bySearch = (search: string) => (item: EncodedSeriLogObject) =>
  search ? item.message.toLowerCase().search(search.toLowerCase()) > -1 : true;

const byDirection =
  (on: string, direction: string) =>
  (item1: EncodedSeriLogObject, item2: EncodedSeriLogObject) => {
    if (on === SortPropertyOptions.Timestamp) {
      const first = dayjs(item1.timestamp);
      const second = dayjs(item2.timestamp);

      const sortAsc = () => (first.isAfter(second) ? 1 : -1);
      const sortDesc = () => (first.isAfter(second) ? -1 : 1);
      return direction === SortDirectionOptions.Desc ? sortDesc() : sortAsc();
    }

    if (on === SortPropertyOptions.Message) {
      return direction === SortDirectionOptions.Desc
        ? item2.message.localeCompare(item1.message)
        : item1.message.localeCompare(item2.message);
    }

    return direction === SortDirectionOptions.Desc
      ? item2.level.localeCompare(item1.level)
      : item1.level.localeCompare(item2.level);
  };

const applyLimits = (limit: number, page: number) => ({
  start: limit * page - limit,
  end: limit * page,
});
