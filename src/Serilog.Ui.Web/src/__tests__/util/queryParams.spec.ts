import { parseSearchParams } from 'app/util/queryParams';
import {
  LogLevel,
  SortDirectionOptions,
  SortPropertyOptions,
} from 'types/types';
import { describe, expect, it } from 'vitest';

describe('util: queryParams', () => {
  describe('parseSearchParams', () => {
    it('parses all supported query parameters', () => {
      const params = new URLSearchParams({
        table: 'test-table',
        level: 'Error',
        search: 'test search',
        startDate: '2024-01-01T00:00:00.000Z',
        endDate: '2024-01-02T00:00:00.000Z',
        sortOn: 'Level',
        sortBy: 'Asc',
        page: '2',
        entriesPerPage: '25',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams).toEqual({
        table: 'test-table',
        level: LogLevel.Error,
        search: 'test search',
        startDate: new Date('2024-01-01T00:00:00.000Z'),
        endDate: new Date('2024-01-02T00:00:00.000Z'),
        sortOn: SortPropertyOptions.Level,
        sortBy: SortDirectionOptions.Asc,
        page: 2,
        entriesPerPage: '25',
      });
      expect(result.cleanedFromInvalidQueryString).toBeUndefined();
    });

    it('preserves valid params while cleaning invalid ones', () => {
      const params = new URLSearchParams({
        table: 'my-table',
        level: 'NotALevel',
        page: '3',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams.table).toBe('my-table');
      expect(result.urlParams.page).toBe(3);
      expect(result.urlParams.level).toBeUndefined();
      expect(result.cleanedFromInvalidQueryString).toBeDefined();
      expect(result.cleanedFromInvalidQueryString?.get('table') ?? true).toBe(
        'my-table',
      );
      expect(result.cleanedFromInvalidQueryString?.get('page') ?? true).toBe(
        '3',
      );
      expect(
        result.cleanedFromInvalidQueryString?.has('level') ?? true,
      ).toBeFalsy();
    });

    describe('invalid parameters cleanup', () => {
      it('ignores invalid dates and flags them for cleanup', () => {
        const params = new URLSearchParams({
          startDate: 'not-a-date',
          endDate: 'invalid',
        });

        const result = parseSearchParams(params);

        expect(result.urlParams.startDate).toBeUndefined();
        expect(result.urlParams.endDate).toBeUndefined();
        expect(result.cleanedFromInvalidQueryString).toBeDefined();
        expect(
          result.cleanedFromInvalidQueryString?.has('startDate') ?? true,
        ).toBeFalsy();
        expect(
          result.cleanedFromInvalidQueryString?.has('endDate') ?? true,
        ).toBeFalsy();
      });

      it('ignores invalid page numbers and flags them for cleanup', () => {
        const params = new URLSearchParams({
          page: '0',
        });

        const result = parseSearchParams(params);

        expect(result.urlParams.page).toBeUndefined();
        expect(result.cleanedFromInvalidQueryString).toBeDefined();
        expect(result.cleanedFromInvalidQueryString!.has('page')).toBe(false);
      });

      it('ignores non-numeric page values and flags them for cleanup', () => {
        const params = new URLSearchParams({
          page: 'abc',
        });

        const result = parseSearchParams(params);

        expect(result.urlParams.page).toBeUndefined();
        expect(result.cleanedFromInvalidQueryString).toBeDefined();
      });

      it.each(['level', 'entriesPerPage', 'sortBy', 'sortOn'])(
        'ignores invalid enum key for [%s] and flags them for cleanup',
        (key) => {
          const params = new URLSearchParams({
            [key]: 'invalid',
          });

          const result = parseSearchParams(params);

          expect(result.urlParams[key]).toBeUndefined();
          expect(result.cleanedFromInvalidQueryString).toBeDefined();
          expect(
            result.cleanedFromInvalidQueryString?.has(key) ?? true,
          ).toBeFalsy();
        },
      );
    });

    const mapper = (k: string) => (v: string) => ({ k, v });
    const loglevels = Object.values(LogLevel).map(mapper('level'));
    const by = Object.values(SortDirectionOptions).map(mapper('sortBy'));
    const on = Object.values(SortPropertyOptions).map(mapper('sortOn'));
    const entries = ['10', '25', '50', '100'].map(mapper('entriesPerPage'));
    it.each([...loglevels, ...entries, ...by, ...on])(
      'accepts valid $k value: $v',
      ({ k, v }) => {
        const params = new URLSearchParams({ [k]: v });
        const result = parseSearchParams(params);
        expect(result.urlParams[k]).toBe(v);
        expect(result.cleanedFromInvalidQueryString).toBeUndefined();
      },
    );

    it('returns empty urlParams for no parameters', () => {
      const params = new URLSearchParams();

      const result = parseSearchParams(params);

      expect(result.urlParams).toEqual({});
      expect(result.cleanedFromInvalidQueryString).toBeUndefined();
    });

    it('ignores unknown query parameters', () => {
      const params = new URLSearchParams({
        table: 'test-table',
        unknownParam: 'value',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams).toEqual({ table: 'test-table' });
      expect(result.urlParams).not.toHaveProperty('unknownParam');
    });
  });
});
