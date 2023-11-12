import { AppShell } from '@mantine/core';
import { type SearchForm } from '../../types/types';
import { SearchFormProvider, useSearchForm } from '../hooks/SearchFormContext';
import Paging from './Search/Paging';
import Search from './Search/Search';
import SerilogResults from './Table/Table';

const formInitialValues: SearchForm = {
  table: '',
  entriesPerPage: 10,
  level: null,
  startDate: null,
  endDate: null,
  search: '',
  page: 1,
};

const AppBody = () => {
  const methods = useSearchForm({
    initialValues: formInitialValues,
    validate: {}, // TODO?
    // TODO: fix form input changes that doesn't reset page to 1 (as it happens on submit button)
  });
  return (
    <AppShell.Main>
      <SearchFormProvider form={methods}>
        <Search />
        <SerilogResults />
        <Paging />
      </SearchFormProvider>
    </AppShell.Main>
  );
};

export default AppBody;
