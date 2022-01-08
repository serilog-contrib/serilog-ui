import { isAfter, parseJSON } from 'date-fns/esm';
import compareAsc from 'date-fns/esm/fp/compareAsc/index';
import isBefore from 'date-fns/esm/fp/isBefore/index';
import parseISO from 'date-fns/esm/parseISO';
import compareDesc from 'date-fns/fp/compareDesc/index';
import { rest } from 'msw'
import { EncodedSeriLogObject, LogLevel, SearchParameters } from '../../types/types';
import { fakeLogs } from './samples';

export const handlers = [
    rest.get('https://127.0.0.1:1234/api/logs', (req, res, ctx) => {
        const params = getSearchParams(req.url.searchParams);
        const logs = fakeLogs.logs.filter(byLevel(LogLevel[params.level]))
            .filter(byDates(params.start, params.end))
            .filter(bySearch(params.search))
            .sort(byDirection(params.sort));
        const sliceValues = applyLimits(params.count, params.page);

        const data = {
            currentPage: params.page,
            count: params.count,
            total: logs.length,
            logs: logs.slice(sliceValues.start, sliceValues.end)
        };
        return res(ctx.status(200), ctx.json(data))
    }),
];

const getSearchParams = (params: URLSearchParams) => ({
    count: Number.parseInt(params.get(SearchParameters.Count)),
    page: Number.parseInt(params.get(SearchParameters.Page)),
    level: params.get(SearchParameters.Level),
    search: params.get(SearchParameters.Search),
    start: params.get(SearchParameters.StartDate),
    end: params.get(SearchParameters.EndDate),
    sort: params.get(SearchParameters.SortDirection),
});

const byLevel = (level?: LogLevel) =>
    (item: EncodedSeriLogObject) => level ? item.level === level : true;
const byDates = (start?: string, end?: string) => (item: EncodedSeriLogObject) => {
    if (!start && !end) return true;
    let res = true;
    const date = parseJSON(item.timestamp);
    if (start) {
        const ds = parseISO(start);
        res = isAfter(date, ds);
    }
    if (end) {
        const de = parseISO(end);
        res = isBefore(date, de);
    }
    return res;
}
const bySearch = (search: string) => (item: EncodedSeriLogObject) => search ? item.message.search(search) : true;
const byDirection = (direction: string) => (item1: EncodedSeriLogObject, item2: EncodedSeriLogObject) => {
    const first = parseJSON(item1.timestamp);
    const second = parseJSON(item2.timestamp);
    return direction === 'desc' ? compareDesc(first, second) : compareAsc(first, second);
}
const applyLimits = (limit: number, page: number) => ({ start: limit * page - limit, end: (limit * page) })