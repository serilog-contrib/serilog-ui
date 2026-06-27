import {
  act,
  renderHookSerilogUiTestWrapper,
} from '__tests__/_setup/testing-utils';
import {
  useQueryParamReader,
  useQueryParamSync,
  useQuerySyncTable,
} from 'app/hooks/useQueryParamSync';
import dayjs from 'dayjs';
import * as router from 'react-router';
import { describe, expect, it, vi } from 'vitest';
import * as form from '../../app/hooks/useSearchForm';

const mockSearchParams = (startingParams?: string) => {
  const setParamsMock = vi.fn();
  const searchParams = new URLSearchParams(startingParams);
  vi.spyOn(router, 'useSearchParams').mockImplementation(() => [
    searchParams,
    setParamsMock,
  ]);
  return { p: searchParams, fn: setParamsMock };
};

describe('useQueryParamSync', () => {
  it.each(
    (['startDate', 'endDate'] as const).flatMap((d) =>
      [null, dayjs().toString()].map((v) => ({
        d,
        v,
      })),
    ),
  )('updates date param $d: $v', ({ d, v }) => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateDateParam(d)(v);
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(
      `${d}=${v ? encodeURIComponent(dayjs(v).toISOString()) : null}`,
    );
  });

  it.each([null, 'test'])('updates level param: %s', (v) => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateLevelParam(v);
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(`level=${v}`);
  });

  it('updates search param', () => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateSearchParam('test');
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(`search=test`);
  });

  it.each([null, 'test'])('updates table param: %s', (v) => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateTableParam(v);
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(`table=${v}`);
  });

  it.each(
    (['sortOn', 'sortBy', 'entriesPerPage', 'page'] as const).map((d) => ({
      d,
      v: 'test',
    })),
  )('updates $d param: $v', ({ d, v }) => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateParam(d)(v);
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(`${d}=test`);
  });

  it('updates multiple params', () => {
    const { p, fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQueryParamSync(),
    );

    result.current.updateMultipleParams({ k: 'test', k2: 2 });
    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe(`k=test&k2=2`);
  });
});

describe('useQuerySyncTable', () => {
  it('not invokes set-params on empty table', () => {
    const { fn } = mockSearchParams();
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQuerySyncTable(),
    );

    result.current.registerKeyOnQuery();

    expect(fn).not.toHaveBeenCalled();
  });

  it('not invokes set-params when query-params has table', () => {
    const { fn } = mockSearchParams('table=def');
    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQuerySyncTable(),
    );

    result.current.registerKeyOnQuery('other-table');

    expect(fn).not.toHaveBeenCalled();
  });

  it('invokes set-params when default table is provided', () => {
    const { p, fn } = mockSearchParams();

    const { result } = renderHookSerilogUiTestWrapper(() =>
      useQuerySyncTable(),
    );

    act(() => {
      result.current.registerKeyOnQuery('default');
    });

    expect(fn).toHaveBeenCalledOnce();

    fn.mock.lastCall?.[0](p);
    expect(p.toString()).toBe('table=default');
  });
});

describe('useQueryParamReader', () => {
  it('happy-path, removing invalid parameter', () => {
    const getValues = () => ({
      level: null,
      search: 'text',
      table: '',
    });
    const setValue = vi.fn();
    vi.spyOn(form, 'useSearchForm').mockImplementation(
      () => ({ getValues, setValue }) as any,
    );

    const { p, fn } = mockSearchParams();
    p.set('level', 'Warning');
    p.set('sortBy', 'invalid');
    p.set('table', 'logs');

    renderHookSerilogUiTestWrapper(() => useQueryParamReader());

    // adding values from search-params...
    expect(setValue).toHaveBeenNthCalledWith(1, 'level', 'Warning');
    expect(setValue).toHaveBeenNthCalledWith(2, 'table', 'logs');
    // removing existing values that cannot be found in search-params...
    expect(setValue).toHaveBeenNthCalledWith(3, 'search', '');

    expect(fn).toHaveBeenCalledOnce();
    // checking that invalid values have been removed from the query params...
    expect(fn.mock.lastCall?.[0].toString()).toBe('level=Warning&table=logs');
  });
});
