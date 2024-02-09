import { useForm, useFormContext } from 'react-hook-form';
import { type SearchForm } from '../../types/types';

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

  return { methods, ...useSearchContext };
};
