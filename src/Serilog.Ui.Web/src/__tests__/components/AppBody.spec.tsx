import { act, render, within } from '__tests__/_setup/testing-utils';
import AppBody from 'app/components/AppBody';
import { describe, expect, it, vi } from 'vitest';

describe('AppBody', () => {
  it('renders', async () => {
    vi.useFakeTimers();
    const { container } = render(<AppBody hideMobileResults />);

    await act(async () => {
      await vi.runAllTimersAsync();
    });

    expect(within(container).getByLabelText('paging-left-column')).toBeInTheDocument();

    vi.useRealTimers();
  });
});
