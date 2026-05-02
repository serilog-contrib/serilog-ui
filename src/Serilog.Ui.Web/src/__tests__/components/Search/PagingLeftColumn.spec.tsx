import {
  renderSerilogUiTestWrapper,
  screen,
  userEvent,
  within,
} from '__tests__/_setup/testing-utils';
import { PagingLeftColumn } from 'app/components/Search/PagingLeftColumn';
import { PagingRightColumn } from 'app/components/Search/PagingRightColumn';
import { useQueryParamReader } from 'app/hooks/useQueryParamSync';
import { SearchResult } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const defaultReturn: SearchResult = {
  count: 10,
  currentPage: 1,
  logs: [],
  total: 1000,
};
const mockQueryLogs = {
  data: defaultReturn,
};
vi.mock('../../../app/hooks/useQueryLogs', () => {
  return {
    default: () => mockQueryLogs,
  };
});
vi.mock('../../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => ({
      disabledSortOnKeys: ['test-key'],
    }),
  };
});
const watchMock = vi.fn();
vi.mock('../../../app/hooks/useSearchForm', async () => {
  const actual = await vi.importActual('../../../app/hooks/useSearchForm');
  return {
    useSearchForm: () => ({
      ...actual,
      watch: watchMock,
    }),
  };
});
const PagingLeftTester = () => {
  useQueryParamReader();
  return <PagingLeftColumn />;
};

describe('Paging', () => {
  it('renders correctly', () => {
    renderSerilogUiTestWrapper(<PagingLeftTester />);

    expect(screen.getByLabelText('paging-left-column')).toBeInTheDocument();
    expect(screen.getAllByDisplayValue('10')).toHaveLength(2);
    expect(screen.getAllByDisplayValue('Timestamp')).toHaveLength(2);
    expect(screen.getByRole('button', { name: 'sortBy' })).toBeInTheDocument();
  });

  it('changes entries per page value', async () => {
    renderSerilogUiTestWrapper(
      <>
        <PagingLeftTester />
        <PagingRightColumn />
      </>,
    );

    // [we're on page 1, inputXPage 10]
    const activePageBtn = () => screen.getByRole('button', { current: 'page' });
    expect(activePageBtn().innerText).toBe('1');
    const pageBtn = screen.getByRole('button', { name: '2' });
    const inputEntriesPerPage = screen.getByRole<HTMLInputElement>('textbox', {
      name: 'entriesPerPage',
    });

    await userEvent.click(pageBtn);

    // [make sure our component moved to page 2]
    expect(inputEntriesPerPage.value).toBe('10');
    expect(activePageBtn().innerText).toBe('2');

    await userEvent.click(inputEntriesPerPage);

    const listBox = screen.getByRole('listbox');
    const selectOption = within(listBox).getByRole('option', {
      name: '25',
    });

    await userEvent.selectOptions(listBox, selectOption);

    // [make sure our component moved to inputXPage 25, while going to page 1]
    expect(inputEntriesPerPage.value).toBe('25');
    expect(activePageBtn().innerText).toBe('1');
  });

  it('changes sort on value', async () => {
    renderSerilogUiTestWrapper(<PagingLeftTester />);

    const sortOn = screen.getByRole<HTMLInputElement>('textbox', {
      name: 'sortOn',
    });
    expect(sortOn.value).toBe('Timestamp');

    await userEvent.click(sortOn);

    const listBox = screen.getByRole('listbox');
    const selectOption = within(listBox).getByRole('option', {
      name: 'Level',
    });

    await userEvent.selectOptions(listBox, selectOption);

    expect(sortOn.value).toBe('Level');
  });

  it('changes sort by value', async () => {
    renderSerilogUiTestWrapper(<PagingLeftTester />);

    const sortBy = screen.getByRole('button', {
      name: 'sortBy',
    });
    expect(sortBy.innerHTML).toContain('sort-descending');

    await userEvent.click(sortBy);

    expect(sortBy.innerHTML).toContain('sort-ascending');
  });

  it('disables the sort on field', async () => {
    watchMock.mockReturnValue('test-key');
    renderSerilogUiTestWrapper(<PagingLeftTester />);

    const sortOn = screen.getByRole<HTMLInputElement>('textbox', {
      name: 'sortOn',
    });
    expect(sortOn).toBeDisabled();
  });
});
