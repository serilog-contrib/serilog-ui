import { useQuery } from '@tanstack/react-query';
import { fetchKeys } from 'app/queries/table-keys';
import { isArrayGuard } from 'app/util/guards';
import { useEffect } from 'react';
import { useForm, useFormContext } from 'react-hook-form';
import { type SearchForm } from '../../types/types';
import { useAuthProperties } from './useAuthProperties';

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
  const { authHeader } = useAuthProperties();

  const queryTableKeys = useQuery<string[]>({
    queryKey: ['get-keys'],
    queryFn: async () => {
      return await fetchKeys(authHeader);
    },
    staleTime: Infinity,
  });
  const tableKeysDefaultValue = isArrayGuard(queryTableKeys.data)
    ? queryTableKeys.data.at(0)!
    : '';

  useEffect(() => {
    methods.resetField('table', { defaultValue: tableKeysDefaultValue });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [tableKeysDefaultValue]);

  return { methods, ...useSearchContext, queryTableKeys };
};
