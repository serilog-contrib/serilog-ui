import { render, screen, userEvent } from '__tests__/_setup/testing-utils';
import { PagingRightColumn } from 'app/components/Search/PagingRightColumn';
import { ControllerRenderProps, FieldValues } from 'react-hook-form';
import { SearchResult } from 'types/types';
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

describe('PagingRightColumn', () => {
  const fieldMock: () => ControllerRenderProps<FieldValues, 'page'> = () => ({
    onChange: vi.fn(),
    onBlur: vi.fn(),
    value: '1',
    name: 'page',
    ref: () => null,
  });

  it('renders correctly with no data', () => {
    render(<PagingRightColumn field={fieldMock()} />);

    expect(screen.getByRole('button', { name: '1' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'pagination-dialog' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'pagination-dialog' })).toBeDisabled();
  });

  it('renders pagination correctly', () => {
    const field = fieldMock();
    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    render(<PagingRightColumn field={field} />);
    expect(screen.getByRole('button', { name: '2' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'pagination-dialog' })).not.toBeDisabled();

    field.value = '2';
  });

  it('calls onChange on pagination button click', async () => {
    const field = fieldMock();
    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    render(<PagingRightColumn field={field} />);
    await userEvent.click(screen.getByRole('button', { name: '2' }));

    expect(field.onChange).toHaveBeenCalledTimes(1);
  });

  it('calls onChange when changing page in the modal', async () => {
    const field = fieldMock();
    mockQueryLogs.data.count = 10;
    mockQueryLogs.data.total = 30;

    render(<PagingRightColumn field={field} />);

    await userEvent.click(screen.getByRole('button', { name: 'pagination-dialog' }));
    await userEvent.type(screen.getByPlaceholderText('1'), '2');

    const setPage = screen.getByRole('button', { name: 'set-page-dialog' });
    await userEvent.click(setPage);

    expect(field.onChange).toHaveBeenCalledTimes(1);
  });
});
