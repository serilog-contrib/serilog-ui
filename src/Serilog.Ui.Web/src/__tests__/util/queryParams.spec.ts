import { parseSearchParams, serializeSearchParams } from 'app/util/queryParams';
import { describe, expect, it } from 'vitest';
import {
  LogLevel,
  SearchForm,
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
        count: '20',
      });

      const result = parseSearchParams(params);

      expect(result).toEqual({
        table: 'test-table',
        level: LogLevel.Error,
        search: 'test search',
        startDate: new Date('2024-01-01T00:00:00.000Z'),
        endDate: new Date('2024-01-02T00:00:00.000Z'),
        sortOn: SortPropertyOptions.Level,
        sortBy: SortDirectionOptions.Asc,
        page: 2,
        entriesPerPage: '20',
      });
    });

    it('parses alternative parameter names', () => {
      const params = new URLSearchParams({
        key: 'test-table',
        from: '2024-01-01T00:00:00.000Z',
        to: '2024-01-02T00:00:00.000Z',
        entriesPerPage: '25',
      });

      const result = parseSearchParams(params);

      expect(result.table).toBe('test-table');
      expect(result.startDate).toEqual(new Date('2024-01-01T00:00:00.000Z'));
      expect(result.endDate).toEqual(new Date('2024-01-02T00:00:00.000Z'));
      expect(result.entriesPerPage).toBe('25');
    });

    it('handles "till" as alternative for endDate', () => {
      const params = new URLSearchParams({
        till: '2024-01-02T00:00:00.000Z',
      });

      const result = parseSearchParams(params);

      expect(result.endDate).toEqual(new Date('2024-01-02T00:00:00.000Z'));
    });

    it('ignores invalid log levels', () => {
      const params = new URLSearchParams({
        level: 'InvalidLevel',
      });

      const result = parseSearchParams(params);

      expect(result.level).toBeUndefined();
    });

    it('ignores invalid dates', () => {
      const params = new URLSearchParams({
        startDate: 'not-a-date',
        endDate: 'invalid',
      });

      const result = parseSearchParams(params);

      expect(result.startDate).toBeUndefined();
      expect(result.endDate).toBeUndefined();
    });

    it('ignores invalid page numbers', () => {
      const params = new URLSearchParams({
        page: '0',
      });

      const result = parseSearchParams(params);

      expect(result.page).toBeUndefined();
    });

    it('returns empty object for no parameters', () => {
      const params = new URLSearchParams();

      const result = parseSearchParams(params);

      expect(result).toEqual({});
    });
  });

  describe('serializeSearchParams', () => {
    const baseForm: SearchForm = {
      table: 'test-table',
      level: null,
      startDate: null,
      endDate: null,
      search: '',
      sortOn: SortPropertyOptions.Timestamp,
      sortBy: SortDirectionOptions.Desc,
      entriesPerPage: '10',
      page: 1,
    };

    it('serializes all non-default values', () => {
      const form: SearchForm = {
        ...baseForm,
        table: 'custom-table',
        level: LogLevel.Error,
        search: 'test search',
        startDate: new Date('2024-01-01T00:00:00.000Z'),
        endDate: new Date('2024-01-02T00:00:00.000Z'),
        sortOn: SortPropertyOptions.Level,
        sortBy: SortDirectionOptions.Asc,
        page: 2,
        entriesPerPage: '20',
      };

      const result = serializeSearchParams(form);

      expect(result.get('table')).toBe('custom-table');
      expect(result.get('level')).toBe('Error');
      expect(result.get('search')).toBe('test search');
      expect(result.get('startDate')).toBe('2024-01-01T00:00:00.000Z');
      expect(result.get('endDate')).toBe('2024-01-02T00:00:00.000Z');
      expect(result.get('sortOn')).toBe('Level');
      expect(result.get('sortBy')).toBe('Asc');
      expect(result.get('page')).toBe('2');
      expect(result.get('count')).toBe('20');
    });

    it('omits default values to keep URL clean', () => {
      const form: SearchForm = {
        ...baseForm,
        table: 'test-table',
        sortOn: SortPropertyOptions.Timestamp, // default
        sortBy: SortDirectionOptions.Desc, // default
        page: 1, // default
        entriesPerPage: '10', // default
      };

      const result = serializeSearchParams(form);

      expect(result.get('sortOn')).toBeNull();
      expect(result.get('sortBy')).toBeNull();
      expect(result.get('page')).toBeNull();
      expect(result.get('count')).toBeNull();
      expect(result.get('table')).toBe('test-table');
    });

    it('omits null and empty values', () => {
      const form: SearchForm = {
        ...baseForm,
        level: null,
        search: '',
        startDate: null,
        endDate: null,
      };

      const result = serializeSearchParams(form);

      expect(result.get('level')).toBeNull();
      expect(result.get('search')).toBeNull();
      expect(result.get('startDate')).toBeNull();
      expect(result.get('endDate')).toBeNull();
    });

    it('includes table even if empty string', () => {
      const form: SearchForm = {
        ...baseForm,
        table: '',
      };

      const result = serializeSearchParams(form);

      // Empty string should not be included
      expect(result.get('table')).toBeNull();
    });
  });
});
