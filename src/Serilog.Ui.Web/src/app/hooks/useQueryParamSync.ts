import { getSearchableDate } from 'app/util/dates';
import { parseSearchParams } from 'app/util/queryParams';
import { useEffect } from 'react';
import { useSearchParams } from 'react-router';
import type { SearchForm } from '../../types/types';
import { searchFormInitialValues, useSearchForm } from './useSearchForm';

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

export const useQuerySyncTable = () => {
  const [searchParams, setSearchParams] = useSearchParams();

  const registerKeyOnQuery = (defaultTable?: string) => {
    if (!defaultTable) return;
    if (searchParams.get('table')) return;

    setSearchParams((prev) => {
      prev.set('table', defaultTable);
      return prev;
    });
  };

  return { registerKeyOnQuery };
};

export const useQueryParamReader = () => {
  const { getValues, setValue } = useSearchForm();
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    const { urlParams, cleanedFromInvalidQueryString } = parseSearchParams(searchParams);
    const urlParamKeys = Object.keys(urlParams);
    const currentAppValues = getValues();

    // we set values for anything coming from the URL
    urlParamKeys.forEach((e) => {
      const paramExists = e in currentAppValues;
      if (!paramExists) return;

      if (urlParams[e] === currentAppValues[e]) return;
      setValue(e as keyof SearchForm, urlParams[e]);
    });

    // if the URL doesn't have the key, we need to remove it from the application search
    const resetMissingValues = Object.keys(currentAppValues).filter(
      (value) => !urlParamKeys.includes(value),
    );
    resetMissingValues.forEach((element) => {
      setValue(element as keyof SearchForm, searchFormInitialValues[element]);
    });

    // if there were invalid parameters, we need to set a cleaned query string
    if (!cleanedFromInvalidQueryString) return;
    setSearchParams(cleanedFromInvalidQueryString);

    // no need to register the useEffect for methods/setSearchParams
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [searchParams]);
};
