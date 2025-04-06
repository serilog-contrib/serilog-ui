import { act, render, screen, userEvent } from '__tests__/_setup/testing-utils';
import { RefreshButton } from 'app/components/Refresh/RefreshButton';
import { liveRefreshOptions } from 'app/hooks/useLiveRefresh';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

vi.mock('react-hook-form', async () => {
  const actual =
    await vi.importActual<typeof import('react-hook-form')>('react-hook-form');

  return { ...actual, useWatch: () => 'page' };
});

const headers = () => {
  const head = new Headers();
  head.append('authorization', 'test');
  return head;
};
vi.mock('../../../app/hooks/useAuthProperties', () => ({
  useAuthProperties: () => ({
    fetchInfo: {
      headers: { headers: headers() },
      routePrefix: '',
    },
    isHeaderReady: true,
  }),
}));

describe('RefreshButton', () => {
  beforeEach(() => {
    vi.useFakeTimers({ shouldAdvanceTime: true });
  });
  afterEach(vi.useRealTimers);

  it('renders', async () => {
    render(<RefreshButton />);

    const durationSelector = screen.getByLabelText('refresh-duration-selector');
    expect(durationSelector).toBeInTheDocument();

    await userEvent.click(durationSelector);
    await act(vi.advanceTimersToNextTimerAsync);

    const times = liveRefreshOptions.map((lro) => lro.value);
    times.forEach((time) => {
      expect(screen.getByLabelText('refresh-duration-' + time)).toBeInTheDocument();
    });
  });

  it('runs live feed activies with refetch sample', async () => {
    const spy = vi.spyOn(global, 'fetch');
    render(<RefreshButton />);

    const durationSelector = screen.getByLabelText('refresh-duration-selector');
    await userEvent.click(durationSelector);
    await act(vi.advanceTimersToNextTimerAsync);

    const sampleOpt = liveRefreshOptions[5];
    const timeSelector = screen.getByLabelText('refresh-duration-' + sampleOpt.value);
    await userEvent.click(timeSelector);
    await act(vi.advanceTimersToNextTimerAsync);

    expect(spy).toHaveBeenCalledOnce();

    await act(async () => {
      await vi.advanceTimersByTimeAsync(1000 * 300 + 1);
    });
    expect(spy).toHaveBeenCalledTimes(2);

    const durationStopper = screen.getByLabelText('refresh-duration-cancel-button');
    expect(screen.queryByLabelText('refresh-duration-selector')).not.toBeInTheDocument();
    expect(durationStopper).toBeInTheDocument();

    await userEvent.click(durationStopper);
    await act(vi.advanceTimersToNextTimerAsync);

    expect(
      screen.queryByLabelText('refresh-duration-cancel-button'),
    ).not.toBeInTheDocument();
    expect(screen.getByLabelText('refresh-duration-selector')).toBeInTheDocument();
  });
});
