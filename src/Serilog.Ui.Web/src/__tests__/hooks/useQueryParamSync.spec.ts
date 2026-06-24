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
import { useSearchParams } from 'react-router';
import { describe, expect, it, vi } from 'vitest';
import * as form from '../../app/hooks/useSearchForm';

const useSut = () => {
  const [searchP] = useSearchParams();
  const p = useQueryParamSync();
  return { searchP, ...p };
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
    const { result } = renderHookSerilogUiTestWrapper(useSut);

    act(() => {
      result.current.updateDateParam(d)(v);
    });

    expect(result.current.searchP.toString()).toBe(
      `${d}=${v ? encodeURIComponent(dayjs(v).toISOString()) : null}`,
    );
  });

  it.each([null, 'test'])('updates level param: %s', (v) => {
    const { result } = renderHookSerilogUiTestWrapper(useSut);

    act(() => {
      result.current.updateLevelParam(v);
    });

    expect(result.current.searchP.toString()).toBe(`level=${v}`);
  });

  it('updates search param', () => {
    const { result } = renderHookSerilogUiTestWrapper(useSut);
    act(() => {
      result.current.updateSearchParam('test');
    });
    expect(result.current.searchP.toString()).toBe(`search=test`);
  });

  it.each([null, 'test'])('updates table param: %s', (v) => {
    const { result } = renderHookSerilogUiTestWrapper(useSut);
    act(() => {
      result.current.updateTableParam(v);
    });
    expect(result.current.searchP.toString()).toBe(`table=${v}`);
  });

  it.each(
    (['sortOn', 'sortBy', 'entriesPerPage', 'page'] as const).map((d) => ({
      d,
      v: 'test',
    })),
  )('updates $d param: $v', ({ d, v }) => {
    const { result } = renderHookSerilogUiTestWrapper(useSut);

    act(() => {
      result.current.updateParam(d)(v);
    });
    expect(result.current.searchP.toString()).toBe(`${d}=test`);
  });

  it('updates multiple params', () => {
    const { result } = renderHookSerilogUiTestWrapper(useSut);

    act(() => {
      result.current.updateMultipleParams({ k: 'test', k2: 2 });
    });
    expect(result.current.searchP.toString()).toBe(`k=test&k2=2`);
  });
});

describe('useQuerySyncTable', () => {
  const useSutSyncTable = () => {
    const [searchP, setSp] = useSearchParams();
    const p = useQuerySyncTable();
    return { searchP, setSp, ...p };
  };
  it('not invokes set-params on empty table', () => {
    const { result } = renderHookSerilogUiTestWrapper(useSutSyncTable);

    act(() => {
      result.current.registerKeyOnQuery();
    });

    expect(result.current.searchP.toString()).toBe('');
  });

  it('not invokes set-params when query-params has table', () => {
    const { result } = renderHookSerilogUiTestWrapper(useSutSyncTable);
    act(() => {
      result.current.setSp({ table: 'def' });
    });
    act(() => {
      result.current.registerKeyOnQuery('other-table');
    });

    expect(result.current.searchP.toString()).toBe('table=def');
  });

  it('invokes set-params when default table is provided', () => {
    const { result } = renderHookSerilogUiTestWrapper(useSutSyncTable);

    act(() => {
      result.current.registerKeyOnQuery('default-3');
    });

    expect(result.current.searchP.toString()).toBe('table=default-3');
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
      () => ({ getValues, setValue } as any),
    );

    const { result } = renderHookSerilogUiTestWrapper(() => {
      const [a, b] = useSearchParams();
      useQueryParamReader();
      return { a, b };
    });
    // clearing the mock, to remove the first render mocked calls
    setValue.mockClear();

    act(() => {
      result.current.b({
        level: 'Warning',
        sortBy: 'invalid',
        table: 'logs',
      });
    });

    // adding values from search-params...
    expect(setValue).toHaveBeenNthCalledWith(1, 'level', 'Warning');
    expect(setValue).toHaveBeenNthCalledWith(2, 'table', 'logs');
    // // removing existing values that cannot be found in search-params...
    expect(setValue).toHaveBeenNthCalledWith(3, 'search', '');

    // checking that invalid values have been removed from the query params...
    expect(result.current.a.toString()).toBe('level=Warning&table=logs');
  });
});
