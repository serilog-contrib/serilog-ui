import { isArrayGuard } from 'app/util/guards';
import { useEffect } from 'react';
import { useForm, useFormContext } from 'react-hook-form';
import { type SearchForm } from '../../types/types';
import { useQueryTableKeys } from './useQueryTableKeys';

export const searchFormInitialValues: SearchForm = {
  table: '',
  level: null,
  startDate: null,
  endDate: null,
  search: '',
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
      table: tableKeysDefaultValue,
    });
  };

  useEffect(() => {
    const tableValue = methods.getValues('table');

    if (isSuccess && data && !data.includes(tableValue)) {
      methods.resetField('table', { defaultValue: tableKeysDefaultValue });
      methods.setValue('table', tableKeysDefaultValue);
    }
  }, [data, isSuccess, methods, tableKeysDefaultValue]);

  return { methods, ...useSearchContext, reset: resetForm };
};
