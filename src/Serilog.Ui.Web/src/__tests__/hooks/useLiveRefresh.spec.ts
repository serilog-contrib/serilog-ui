import { act, renderHook } from '__tests__/_setup/testing-utils';
import { describe, expect, it } from 'vitest';
import { useLiveRefresh } from '../../app/hooks/useLiveRefresh';

describe('useLiveRefresh', () => {
  it('returns base properties', () => {
    const { result } = renderHook(() => useLiveRefresh());

    expect(result.current.isLiveRefreshRunning).toBeFalsy();
    expect(result.current.liveRefreshLabel).toBe('');
    expect(result.current.refetchInterval).toBe(0);
  });

  it.each([
    { refetch: 5000, label: '5s', time: 'five' },
    { refetch: 15000, label: '15s', time: 'fifteen' },
    { refetch: 30000, label: '30s', time: 'thirty' },
    { refetch: 60000, label: '1m', time: 'sixty' },
    { refetch: 120000, label: '2m', time: 'onehundredtwenty' },
    { refetch: 300000, label: '5m', time: 'threehundred' },
    { refetch: 900000, label: '15m', time: 'ninehundred' },
  ])('starts fetch interval', ({ label, refetch, time }) => {
    const { result } = renderHook(() => useLiveRefresh());

    act(() => {
      result.current.startLiveRefresh(time);
    });

    expect(result.current.isLiveRefreshRunning).toBeTruthy();
    expect(result.current.liveRefreshLabel).toBe(label);
    expect(result.current.refetchInterval).toBe(refetch);
  });

  it('stops fetch interval', () => {
    const { result } = renderHook(() => useLiveRefresh());

    act(() => {
      result.current.startLiveRefresh('five');
    });
    expect(result.current.refetchInterval).toBe(5000);

    act(() => {
      result.current.stopLiveRefresh();
    });

    expect(result.current.isLiveRefreshRunning).toBeFalsy();
    expect(result.current.refetchInterval).toBe(0);
  });

  it('does not activate on invalid time', () => {
    const { result } = renderHook(() => useLiveRefresh());

    act(() => {
      result.current.startLiveRefresh('five');
    });
    expect(result.current.refetchInterval).toBe(5000);

    act(() => {
      result.current.startLiveRefresh(null);
    });

    expect(result.current.isLiveRefreshRunning).toBeFalsy();
    expect(result.current.refetchInterval).toBe(0);
  });

  it('set activation time to 0 on unexpected time', () => {
    const { result } = renderHook(() => useLiveRefresh());

    act(() => {
      result.current.startLiveRefresh('uhm');
    });

    expect(result.current.isLiveRefreshRunning).toBeFalsy();
    expect(result.current.refetchInterval).toBe(0);
  });
});
