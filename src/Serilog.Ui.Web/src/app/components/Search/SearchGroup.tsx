import { ReactNode } from 'react';
import Search from './Search';

type IProps = {
  showSearch?: boolean;
  showPaging?: boolean;
  children?: ReactNode;
};

// to remove
export const SearchGroup = (props: IProps) => {
  return (
    <>
      {props?.showSearch && <Search />}
      {props?.children}
      {/* {props?.showPaging && <Paging />} */}
    </>
  );
};
