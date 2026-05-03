import {
  act,
  renderSerilogUiTestWrapper,
  screen,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from '__tests__/_setup/testing-utils';
import FilterButton from 'app/components/ShellStructure/FilterButton';
import { describe, expect, it, vi } from 'vitest';

const useMocks = {
  reset: vi.fn(),
  refetch: vi.fn(),
  watch: vi.fn(),
  handleSubmit: vi.fn(),
};
vi.mock('../../../app/hooks/useQueryLogs', () => {
  return {
    default: () => ({ ...useMocks }),
    useQueryLogs: () => ({ ...useMocks }),
  };
});
vi.mock('../../../app/hooks/useSearchForm', () => {
  return {
    useSearchForm: () => ({
      ...useMocks,
    }),
  };
});

describe('filterButton', () => {
  it('renders', async () => {
    renderSerilogUiTestWrapper(<FilterButton />);

    const filterBtn = screen.getByRole('button');
    expect(filterBtn).toBeInTheDocument();

    await userEvent.click(filterBtn);

    expect(screen.getByText('Search filters')).toBeInTheDocument();
    await waitFor(() => {
      expect(screen.getByRole('form', { name: 'search-logs-form' })).toBeInTheDocument();
    });
  });

  it('closes modal on resize', async () => {
    renderSerilogUiTestWrapper(<FilterButton />);

    const filterBtn = screen.getByRole('button');
    await userEvent.click(filterBtn);

    const modalTitle = screen.queryByText('Search filters');
    expect(modalTitle).toBeInTheDocument();

    act(() => {
      window.dispatchEvent(new Event('resize'));
    });

    await waitForElementToBeRemoved(modalTitle);
  });
});
