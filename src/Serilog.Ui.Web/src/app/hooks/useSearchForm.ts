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
  isUtc: false,
  entriesPerPage: '10',
  page: 1,
};

export const useSearchForm = () => {
  const methods = useForm<SearchForm>({
    defaultValues: searchFormInitialValues,
  });
  const useSearchContext = useFormContext<SearchForm>();
  const { data } = useQueryTableKeys();
  const tableKeysDefaultValue = isArrayGuard(data) ? data.at(0)! : '';

  const resetForm = () => {
    useSearchContext.reset({
      isUtc: useSearchContext.getValues('isUtc'),
      table: tableKeysDefaultValue,
    });
  };

  useEffect(() => {
    const tableValue = methods.getValues('table');

    if (data && !data.includes(tableValue)) {
      methods.resetField('table', { defaultValue: tableKeysDefaultValue });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tableKeysDefaultValue]);

  return { methods, ...useSearchContext, reset: resetForm };
};
