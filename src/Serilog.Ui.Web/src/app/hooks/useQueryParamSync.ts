import { getSearchableDate } from 'app/util/dates';
import { useSearchParams } from 'react-router';
import type { SearchForm } from '../../types/types';

const parameterizeKeyValues = (key: keyof SearchForm, value: unknown) => {
  if (!value) return value;
  if (key === 'startDate' || key === 'endDate') return getSearchableDate(value as Date);

  return value;
};

export const useQueryParamSync = () => {
  const [, setSearchParams] = useSearchParams();

  const updateParams =
    <T>(key: keyof SearchForm) =>
    (value: T) => {
      const newValues = parameterizeKeyValues(key, value);

      setSearchParams((prev) => {
        prev.set(key, newValues as string);
        return prev;
      });
    };

  const updateDateParam = (key: 'startDate' | 'endDate') =>
    updateParams<Date | null>(key);
  const updateLevelParam = updateParams<string | null>('level');
  const updateParam = <T>(key: 'page' | 'entriesPerPage' | 'sortBy' | 'sortOn') =>
    updateParams<T>(key);
  const updateSearchParam = updateParams<string>('search');
  const updateTableParam = updateParams<string | null>('table');

  return {
    updateDateParam,
    updateLevelParam,
    updateParam,
    updateSearchParam,
    updateTableParam,
  };
};
