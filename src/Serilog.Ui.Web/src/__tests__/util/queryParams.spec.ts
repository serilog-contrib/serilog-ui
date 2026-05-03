import { parseSearchParams } from 'app/util/queryParams';
import { describe, expect, it } from 'vitest';
import {
  LogLevel,
  SortDirectionOptions,
  SortPropertyOptions,
} from 'types/types';

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

    it('ignores invalid log levels and flags them for cleanup', () => {
      const params = new URLSearchParams({
        level: 'InvalidLevel',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams.level).toBeUndefined();
      expect(result.cleanedFromInvalidQueryString).toBeDefined();
      expect(result.cleanedFromInvalidQueryString!.has('level')).toBe(false);
    });

    it('ignores invalid dates and flags them for cleanup', () => {
      const params = new URLSearchParams({
        startDate: 'not-a-date',
        endDate: 'invalid',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams.startDate).toBeUndefined();
      expect(result.urlParams.endDate).toBeUndefined();
      expect(result.cleanedFromInvalidQueryString).toBeDefined();
      expect(result.cleanedFromInvalidQueryString!.has('startDate')).toBe(false);
      expect(result.cleanedFromInvalidQueryString!.has('endDate')).toBe(false);
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

    it('ignores invalid entriesPerPage values and flags them for cleanup', () => {
      const params = new URLSearchParams({
        entriesPerPage: '15',
      });

      const result = parseSearchParams(params);

      expect(result.urlParams.entriesPerPage).toBeUndefined();
      expect(result.cleanedFromInvalidQueryString).toBeDefined();
      expect(result.cleanedFromInvalidQueryString!.has('entriesPerPage')).toBe(false);
    });

    it('accepts valid entriesPerPage values', () => {
      for (const count of ['10', '25', '50', '100']) {
        const params = new URLSearchParams({ entriesPerPage: count });
        const result = parseSearchParams(params);
        expect(result.urlParams.entriesPerPage).toBe(count);
        expect(result.cleanedFromInvalidQueryString).toBeUndefined();
      }
    });

    it('returns empty urlParams for no parameters', () => {
      const params = new URLSearchParams();

      const result = parseSearchParams(params);

      expect(result.urlParams).toEqual({});
      expect(result.cleanedFromInvalidQueryString).toBeUndefined();
    });

    it('skips parameters with no value without flagging them as invalid', () => {
      const params = new URLSearchParams();

      const result = parseSearchParams(params);

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
      expect(result.cleanedFromInvalidQueryString!.get('table')).toBe('my-table');
      expect(result.cleanedFromInvalidQueryString!.get('page')).toBe('3');
      expect(result.cleanedFromInvalidQueryString!.has('level')).toBe(false);
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
