import { isAfter, isBefore, parseJSON, compareAsc, parseISO, compareDesc } from 'date-fns';
import { rest } from 'msw'
import { EncodedSeriLogObject, LogLevel, SearchParameters } from '../../../types/types';
import { fakeLogs } from './samples';

const developmentListenersHost = [
    "https://127.0.0.1:1234",
    "https://localhost:1234",
    "http://127.0.0.1:80", 
    "http://localhost",
    "http://localhost:80"];

export const handlers = [
    ...developmentListenersHost.map(dlh =>
        rest.get(`${dlh}/api/logs`, (req, res, ctx) => {
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
        })
    ),
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
    let after = true;
    let before = true;

    const date = parseJSON(item.timestamp);
    if (start) {
        const ds = parseISO(start);
        after = isAfter(date, ds);
    }
    if (end) {
        const de = parseISO(end);
        before = isBefore(date, de);
    }
    return after && before;
}
const bySearch = (search: string) => (item: EncodedSeriLogObject) => search ? item.message.toLowerCase().search(search.toLowerCase()) > -1 : true;
const byDirection = (direction: string) => (item1: EncodedSeriLogObject, item2: EncodedSeriLogObject) => {
    const first = parseJSON(item1.timestamp);
    const second = parseJSON(item2.timestamp);
    return direction === 'desc' ? compareDesc(first, second) : compareAsc(first, second);
}
const applyLimits = (limit: number, page: number) => ({ start: limit * page - limit, end: (limit * page) })