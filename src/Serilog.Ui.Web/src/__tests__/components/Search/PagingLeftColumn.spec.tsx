import { render, screen, userEvent, within } from '__tests__/_setup/testing-utils';
import { PagingLeftColumn } from 'app/components/Search/PagingLeftColumn';
import { SearchResult } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const defaultReturn: SearchResult = {
  count: 10,
  currentPage: 1,
  logs: [],
  total: 1,
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

describe('Paging', () => {
  it('renders correctly', () => {
    render(<PagingLeftColumn changePage={vi.fn()} />);

    expect(screen.getByLabelText('paging-left-column')).toBeInTheDocument();
    expect(screen.getAllByDisplayValue('10')).toHaveLength(2);
    expect(screen.getAllByDisplayValue('Timestamp')).toHaveLength(2);
    expect(screen.getByRole('button', { name: 'sortBy' })).toBeInTheDocument();
  });

  it('changes entries per page value', async () => {
    const mockChangePage = vi.fn();
    render(<PagingLeftColumn changePage={mockChangePage} />);

    const inputEntriesPerPage = screen.getByRole<HTMLInputElement>('textbox', {
      name: 'entriesPerPage',
    });
    expect(inputEntriesPerPage.value).toBe('10');

    await userEvent.click(inputEntriesPerPage);

    const listBox = screen.getByRole('listbox');
    const selectOption = within(listBox).getByRole('option', {
      name: '25',
    });

    await userEvent.selectOptions(listBox, selectOption);

    expect(mockChangePage).toHaveBeenCalledTimes(1);
    expect(inputEntriesPerPage.value).toBe('25');
  });

  it('changes sort on value', async () => {
    const mockChangePage = vi.fn();
    render(<PagingLeftColumn changePage={mockChangePage} />);

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
    const mockChangePage = vi.fn();
    render(<PagingLeftColumn changePage={mockChangePage} />);

    const sortBy = screen.getByRole('button', {
      name: 'sortBy',
    });
    expect(sortBy.innerHTML).toContain('sort-descending');

    await userEvent.click(sortBy);

    expect(sortBy.innerHTML).toContain('sort-ascending');
  });

  it('disables the sort on field', async () => {
    watchMock.mockReturnValue('test-key');
    render(<PagingLeftColumn changePage={vi.fn()} />);

    const sortOn = screen.getByRole<HTMLInputElement>('textbox', {
      name: 'sortOn',
    });
    expect(sortOn).toBeDisabled();
  });
});
