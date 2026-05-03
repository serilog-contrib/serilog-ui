import type { SearchForm } from '../../types/types';
import dayjs from 'dayjs';
import {
  LogLevel,

  SortDirectionOptions,
  SortPropertyOptions,
} from '../../types/types';

interface ParamParser<T> {
  canHandle: (key: string) => boolean;
  extract: (
    value: string | null,
    key: string,
  ) => {
    valued?: boolean;
    invalid?: boolean;
    value: T | null;
  };
}

const DateParser: ParamParser<Date> = {
  canHandle: k => ['startDate', 'endDate'].includes(k),
  extract: (value) => {
    if (!value) {
      return { valued: false, value: null };
    }

    const date = dayjs(value);
    if (!date.isValid()) {
      return { invalid: true, value: null };
    }
    return { valued: true, value: date.toDate() };
  },
};

enum entriesPerPage {
  Ten = '10',
  TwentyFive = '25',
  Fifty = '50',
  OneHundred = '100',
}
const EnumParser: ParamParser<LogLevel | SortPropertyOptions | SortDirectionOptions> = {
  canHandle: k => ['entriesPerPage', 'level', 'sortBy', 'sortOn'].includes(k),
  extract: (value, k) => {
    if (!value) {
      return { valued: false, value: null };
    }
    let values: string[] = [];
    switch (k) {
      case 'entriesPerPage':
        values = Object.values(entriesPerPage);
        break;
      case 'level':
        values = Object.values(LogLevel);
        break;
      case 'sortBy':
        values = Object.values(SortDirectionOptions);
        break;
      case 'sortOn':
        values = Object.values(SortPropertyOptions);
        break;
    }
    if (!values.includes(value)) {
      return { invalid: true, value: null };
    }

    return {
      valued: true,
      value: value as LogLevel | SortPropertyOptions | SortDirectionOptions,
    };
  },
};
const NumberParser: ParamParser<number> = {
  canHandle: k => ['page'].includes(k),
  extract: (value) => {
    if (!value) {
      return { valued: false, value: null };
    }

    const pageNum = Number.parseInt(value, 10);
    if (!Number.isNaN(pageNum) && pageNum > 0) {
      return { valued: true, value: pageNum };
    }
    return { invalid: true, value: null };
  },
};
const StringParser: ParamParser<string> = {
  canHandle: k => ['search', 'table'].includes(k),
  extract: (value) => {
    return { valued: !!value, value };
  },
};

const keys = [
  'endDate',
  'entriesPerPage',
  'level',
  'page',
  'search',
  'sortBy',
  'sortOn',
  'startDate',
  'table',
] as (keyof SearchForm)[];
const validators = [DateParser, EnumParser, NumberParser, StringParser];

export const parseSearchParams = (searchParams: URLSearchParams) => {
  const getParam = (key: string) => searchParams.get(key);

  return keys.reduce<{
    urlParams: Record<string, string | Date | number | null>;
    cleanedFromInvalidQueryString: URLSearchParams | undefined;
  }>(
    (prev, curr) => {
      const validator = validators.find(x => x.canHandle(curr));
      if (!validator) {
        return prev;
      }
      const data = validator.extract(getParam(curr), curr);

      if (data.valued && data.value !== null) {
        prev.urlParams[curr] = data.value;
      }

      if (data.invalid) {
        prev.cleanedFromInvalidQueryString ??= new URLSearchParams(
          searchParams.toString(),
        );
        prev.cleanedFromInvalidQueryString.delete(curr);
      }

      return prev;
    },
    {
      urlParams: {},
      cleanedFromInvalidQueryString: undefined,
    },
  );
};
