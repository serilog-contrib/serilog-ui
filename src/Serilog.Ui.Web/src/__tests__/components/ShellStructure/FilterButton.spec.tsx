import {
  act,
  render,
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

describe('FilterButton', () => {
  it('renders', async () => {
    render(<FilterButton />);

    const filterBtn = screen.getByRole('button');
    expect(filterBtn).toBeInTheDocument();

    await userEvent.click(filterBtn);

    expect(screen.getByText('Search filters')).toBeInTheDocument();
    await waitFor(() => {
      expect(screen.getByRole('form', { name: 'search-logs-form' })).toBeInTheDocument();
    });
  });

  it.each([
    { resetRes: true, times: 1 },
    { resetRes: false, times: 0 },
  ])(
    'clears search state and refetch data if reset returns $resetRes',
    async ({ resetRes, times }) => {
      useMocks.reset.mockImplementationOnce(() => resetRes);

      render(<FilterButton />);

      const filterBtn = screen.getByRole('button');

      expect(filterBtn).toBeInTheDocument();

      await userEvent.click(filterBtn);

      await userEvent.click(screen.getByRole('button', { name: 'reset filters' }));

      expect(useMocks.reset).toHaveBeenCalledOnce();
      expect(useMocks.refetch).toHaveBeenCalledTimes(times);
    },
  );

  it('closes modal on resize', async () => {
    render(<FilterButton />);

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
