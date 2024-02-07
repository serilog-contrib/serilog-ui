import { createFormContext } from '@mantine/form';
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

// You can give context variables any name
export const [SearchFormProvider, useSearchFormContext, useSearchForm] =
  createFormContext<SearchForm>();
