import { isArrayGuard } from 'app/util/guards';
import { parseSearchParams } from 'app/util/queryParams';
import { useEffect } from 'react';
import { useForm, useFormContext } from 'react-hook-form';
import { useSearchParams } from 'react-router';
import {
  SortDirectionOptions,
  SortPropertyOptions,
  type SearchForm,
} from '../../types/types';
import { useQueryTableKeys } from './useQueryTableKeys';

export const searchFormInitialValues: SearchForm = {
  table: '',
  level: null,
  startDate: null,
  endDate: null,
  search: '',
  sortBy: SortDirectionOptions.Desc,
  sortOn: SortPropertyOptions.Timestamp,
  entriesPerPage: '10',
  page: 1,
};

// react-query run a refetch when any of these values change,
// as they're part of its query hash-key.
// If on a clear fields no value was changed, we run a manual refetch
// otherwise it won't automatically run
const runManualRefetch = (getValues: () => SearchForm, tableDefault: string) => {
  const { table, entriesPerPage, page, sortBy, sortOn } = getValues();

  const propertiesToCheck = [
    table === tableDefault,
    entriesPerPage === searchFormInitialValues.entriesPerPage,
    page === searchFormInitialValues.page,
    sortBy === searchFormInitialValues.sortBy,
    sortOn === searchFormInitialValues.sortOn,
  ];

  return propertiesToCheck.every((isEqualToDefault) => isEqualToDefault);
};

export const useSearchForm = () => {
  const methods = useForm<SearchForm>({
    defaultValues: searchFormInitialValues,
  });
  const useSearchContext = useFormContext<SearchForm>();
  const [searchParams, setSearchParams] = useSearchParams();

  const { data } = useQueryTableKeys();
  const tableKeysDefaultValue = isArrayGuard(data) ? data.at(0)! : '';

  const resetForm = (blankTable?: boolean) => {
    const runRefetch = runManualRefetch(
      useSearchContext.getValues,
      tableKeysDefaultValue,
    );

    const tableValue = !blankTable
      ? new URLSearchParams({ table: tableKeysDefaultValue })
      : '';
    setSearchParams(tableValue);

    return runRefetch;
  };

  useEffect(() => {
    const { urlParams, cleanedFromInvalidQueryString } = parseSearchParams(searchParams);
    const urlParamKeys = Object.keys(urlParams);
    const currentAppValues = methods.getValues();

    // we set values for anything coming from the URL
    urlParamKeys.forEach((e) => {
      const paramExists = e in currentAppValues;
      if (!paramExists) return;

      if (urlParams[e] === currentAppValues[e]) return;
      methods.setValue(e as keyof SearchForm, urlParams[e]);
    });

    // if the URL doesn't have the key, we need to remove it from the application search
    const resetMissingValues = Object.keys(currentAppValues).filter(
      (value) => !urlParamKeys.includes(value),
    );
    resetMissingValues.forEach((element) => {
      methods.setValue(element as keyof SearchForm, searchFormInitialValues[element]);
    });

    // if there were invalid parameters, we need to set a cleaned query string
    if (!cleanedFromInvalidQueryString) return;
    setSearchParams(cleanedFromInvalidQueryString);

    // no need to register the useEffect for methods/setSearchParams
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [searchParams]);

  return { methods, ...useSearchContext, reset: resetForm };
};
