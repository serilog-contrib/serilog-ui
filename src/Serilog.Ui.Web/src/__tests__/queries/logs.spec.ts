import { server } from '__tests__/_setup/mocks/msw-server';
import { fakeLogs } from '__tests__/_setup/mocks/samples';
import { fetchLogs } from 'app/queries/logs';
import * as queryUtil from 'app/util/queries';
import dayjs from 'dayjs';
import { HttpResponse, http } from 'msw';
import {
  LogLevel,
  SearchForm,
  SortDirectionOptions,
  SortPropertyOptions,
} from 'types/types';
import { describe, expect, it, vi } from 'vitest';

describe('queries: logs', () => {
  const defaultReturn = {
    count: 0,
    currentPage: 1,
    logs: [],
    total: 0,
  };

  const sample: SearchForm = {
    startDate: dayjs().subtract(10, 'hour').toDate(),
    endDate: dayjs().add(10, 'hour').toDate(),
    entriesPerPage: '10',
    level: LogLevel.Information,
    page: 1,
    search: 'search',
    table: 'example-db',
    sortBy: SortDirectionOptions.Asc,
    sortOn: SortPropertyOptions.Level,
  };

  it('returns fetched data, including all query parameters', async () => {
    let req = '';
    server.use(
      http.get('*', ({ request }) => {
        req = request.url;
        return HttpResponse.json({
          ...defaultReturn,
          logs: [fakeLogs.logs[0]],
          total: 1,
        });
      }),
    );

    const sut = await fetchLogs(sample, { headers: new Headers() }, 'prefix');

    expect(sut).toStrictEqual({
      ...defaultReturn,
      logs: [fakeLogs.logs[0]],
      total: 1,
    });

    expect(
      req.endsWith(
        `logs?page=1&count=10&startDate=${sample.startDate?.toISOString()}&endDate=${sample.endDate?.toISOString()}&level=Information&search=search&sortBy=Asc&sortOn=Level&key=example-db`,
      ),
    ).toBeTruthy();
  });

  it('returns default if dates are not admitted', async () => {
    const sut = await fetchLogs(
      {
        ...sample,
        endDate: dayjs().add(1, 'day').toDate(),
        startDate: dayjs().add(2, 'day').toDate(),
      },
      { headers: new Headers() },
      'prefix',
    );

    expect(sut).toStrictEqual(defaultReturn);
  });

  it('returns default if request failed and send unexpected notification', async () => {
    server.use(http.get('*', () => new HttpResponse('', { status: 404 })));
    const spy = vi.spyOn(queryUtil, 'sendUnexpectedNotification');

    const sut = await fetchLogs(sample, { headers: new Headers() }, 'prefix');

    expect(sut).toStrictEqual(defaultReturn);
    expect(spy).toHaveBeenCalledOnce();
  });

  it('returns default if request failed and send 403 notification', async () => {
    server.use(http.get('*', () => new HttpResponse('', { status: 403 })));
    const spy = vi.spyOn(queryUtil, 'send403Notification');

    const sut = await fetchLogs(sample, { headers: new Headers() }, 'prefix');

    expect(sut).toStrictEqual(defaultReturn);
    expect(spy).toHaveBeenCalledOnce();
  });
});
