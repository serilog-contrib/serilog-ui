import { isArrayGuard } from 'app/util/guards';
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

// react-query run a refetch when any of these values change,
// as they're part of its query hash-key. 
// If on a clear fields no value was changed, we run a manual refetch
// otherwise it won't automatically run
const runManualRefetch = (getValues: () => SearchForm, tableDefault: string) => {
  const { table, entriesPerPage, page, sortBy, sortOn } = getValues();

  const propertiesToCheck = [table === tableDefault, entriesPerPage === searchFormInitialValues.entriesPerPage,
  page === searchFormInitialValues.page, sortBy === searchFormInitialValues.sortBy, sortOn === searchFormInitialValues.sortOn]

  return propertiesToCheck.every(isEqualToDefault => isEqualToDefault)
}

export const useSearchForm = () => {
  const methods = useForm<SearchForm>({
    defaultValues: searchFormInitialValues,
  });
  const useSearchContext = useFormContext<SearchForm>();

  const { data } = useQueryTableKeys();
  const tableKeysDefaultValue = isArrayGuard(data) ? data.at(0)! : '';

  const resetForm = (blankTable?: boolean) => {
    const runRefetch = runManualRefetch(useSearchContext.getValues, tableKeysDefaultValue)

    useSearchContext.reset({
      ...searchFormInitialValues,
      table: !blankTable ? tableKeysDefaultValue : null,
    });

    return runRefetch
  };

  return { methods, ...useSearchContext, reset: resetForm };
};
