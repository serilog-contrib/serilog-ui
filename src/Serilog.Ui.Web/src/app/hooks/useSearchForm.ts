import { isArrayGuard } from 'app/util/guards';
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

export const useSearchForm = () => {
  const methods = useForm<SearchForm>({
    defaultValues: searchFormInitialValues,
  });
  const useSearchContext = useFormContext<SearchForm>();
  const [, setSearchParams] = useSearchParams();

  const { data } = useQueryTableKeys();
  const tableKeysDefaultValue = isArrayGuard(data) ? data.at(0)! : '';

  const resetForm = () => {
    const tableValue = tableKeysDefaultValue
      ? new URLSearchParams({ table: tableKeysDefaultValue })
      : '';
    setSearchParams(tableValue);
  };

  return { methods, ...useSearchContext, reset: resetForm };
};
