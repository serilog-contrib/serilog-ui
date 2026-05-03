import type { SearchResult } from 'types/types';
import {
  renderSerilogUiTestWrapper,
  screen,
  userEvent,
} from '__tests__/_setup/testing-utils';
import { PagingRightColumn } from 'app/components/Search/PagingRightColumn';
import { useQueryParamReader } from 'app/hooks/useQueryParamSync';
import { describe, expect, it, vi } from 'vitest';

const defaultReturn: () => SearchResult = () => ({
  count: 10,
  currentPage: 1,
  logs: [],
  total: 1,
});
const mockQueryLogs = {
  data: defaultReturn(),
};
vi.mock('../../../app/hooks/useQueryLogs', async () => {
  return {
    default: () => mockQueryLogs,
  };
});
vi.mock('react-router', async () => {
  return {
    useSearchParams: () => mockQueryLogs,
  };
});

const PagingRightTester = () => {
  useQueryParamReader();
  return (
    <>
      <PagingRightColumn />
    </>
  );
};

describe('pagingRightColumn', () => {
  it('renders correctly with no data', () => {
    renderSerilogUiTestWrapper(<PagingRightTester />);

    expect(screen.getByRole('button', { name: '1' })).toBeInTheDocument();
    expect(
      screen.getByRole('button', { name: 'pagination-dialog' }),
    ).toBeInTheDocument();
    expect(
      screen.getByRole('button', { name: 'pagination-dialog' }),
    ).toBeDisabled();
  });

  it('renders pagination correctly', () => {
    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    renderSerilogUiTestWrapper(<PagingRightTester />);
    expect(screen.getByRole('button', { name: '2' })).toBeInTheDocument();
    expect(
      screen.getByRole('button', { name: 'pagination-dialog' }),
    ).not.toBeDisabled();
  });

  it('calls onChange on pagination button click', async () => {
    const activePageBtn = () => screen.getByRole('button', { current: 'page' });
    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    renderSerilogUiTestWrapper(<PagingRightTester />);
    expect(activePageBtn().textContent).toBe('1');
    await userEvent.click(screen.getByRole('button', { name: '2' }));

    expect(activePageBtn().textContent).toBe('2');
  });

  it('calls onChange when changing page in the modal', async () => {
    const activePageBtn = () => screen.getByRole('button', { current: 'page' });

    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    renderSerilogUiTestWrapper(<PagingRightTester />);
    expect(activePageBtn().textContent).toBe('1');

    await userEvent.click(
      screen.getByRole('button', { name: 'pagination-dialog' }),
    );
    await userEvent.type(screen.getByPlaceholderText('1'), '[Backspace]2');

    const setPage = screen.getByRole('button', { name: 'set-page-dialog' });
    await userEvent.click(setPage);

    expect(activePageBtn().textContent).toBe('2');
  });
});
