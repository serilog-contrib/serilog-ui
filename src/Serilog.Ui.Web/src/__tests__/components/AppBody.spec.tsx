import { fakeLogs } from '__tests__/_setup/mocks/samples';
import { act, render, screen } from '__tests__/_setup/testing-utils';
import AppBody from 'app/components/AppBody';
import { describe, expect, it, vi } from 'vitest';

const mockQuery = {
  data: fakeLogs,
  refetch: vi.fn(),
};
vi.mock('../../app/hooks/useQueryLogs', () => ({
  default: () => mockQuery,
}));

describe('AppBody', () => {
  it('renders', async () => {
    vi.useFakeTimers();
    window.innerWidth = 1400;
    render(<AppBody hideMobileResults />);
    await act(async () => {
      await vi.advanceTimersToNextTimerAsync();
    });
    await act(async () => {
      await vi.advanceTimersToNextTimerAsync();
    });
    expect(screen.getByLabelText('paging-left-column')).toBeInTheDocument();
    vi.useRealTimers();
  });
});
