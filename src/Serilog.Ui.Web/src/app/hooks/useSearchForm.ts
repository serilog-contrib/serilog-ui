import { isArrayGuard } from 'app/util/guards';
import { useEffect } from 'react';
import { useForm, useFormContext } from 'react-hook-form';
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

export const useSearchForm = () => {
  const methods = useForm<SearchForm>({
    defaultValues: searchFormInitialValues,
  });
  const useSearchContext = useFormContext<SearchForm>();

  const { data, isSuccess } = useQueryTableKeys();
  const tableKeysDefaultValue = isArrayGuard(data) ? data.at(0)! : '';

  const resetForm = () => {
    useSearchContext.reset({
      ...searchFormInitialValues,
      table: tableKeysDefaultValue,
    });
  };

  useEffect(() => {
    if (!useSearchContext) return;

    const tableValue = useSearchContext.getValues('table');
    if (isSuccess && data && !tableValue) {
      useSearchContext.setValue('table', tableKeysDefaultValue);
    }
  }, [data, isSuccess, tableKeysDefaultValue, useSearchContext]);

  return { methods, ...useSearchContext, reset: resetForm };
};
