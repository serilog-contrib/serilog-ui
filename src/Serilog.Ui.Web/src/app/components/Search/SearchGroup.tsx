import { ReactNode } from 'react';
import Paging from './Paging';
import Search from './Search';

type IProps = {
  showSearch?: boolean;
  showPaging?: boolean;
  children?: ReactNode;
};

export const SearchGroup = (props: IProps) => {
  return (
    <>
      {props?.showSearch && <Search />}
      {props?.children}
      {props?.showPaging && <Paging />}
    </>
  );
};
