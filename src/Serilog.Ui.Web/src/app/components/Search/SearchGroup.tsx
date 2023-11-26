import {
  SearchFormProvider,
  searchFormInitialValues,
  useSearchForm,
} from 'app/hooks/SearchFormContext';
import { ReactNode } from 'react';
import Paging from './Paging';
import Search from './Search';

type IProps = {
  showSearch?: boolean;
  showPaging?: boolean;
  children?: ReactNode;
};

export const SearchGroup = (props: IProps) => {
  const methods = useSearchForm({
    initialValues: searchFormInitialValues,
    validate: {}, // TODO?
    // TODO: fix form input changes that doesn't reset page to 1 (as it happens on submit button)
  });
  return (
    <SearchFormProvider form={methods}>
      {props?.showSearch && <Search />}
      {props?.children}
      {props?.showPaging && <Paging />}
    </SearchFormProvider>
  );
};
