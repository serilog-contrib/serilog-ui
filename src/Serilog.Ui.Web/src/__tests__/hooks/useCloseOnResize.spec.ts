import { renderHook } from '__tests__/_setup/testing-utils';
import { useCloseOnResize } from 'app/hooks/useCloseOnResize';
import { describe, expect, it, vi } from 'vitest';

describe('useCloseOnResize', () => {
  it('invokes callback on window resize', () => {
    const closeMock = vi.fn();
    renderHook(() => useCloseOnResize(closeMock));

    window.dispatchEvent(new Event('resize'));
    window.dispatchEvent(new Event('resize'));

    expect(closeMock).toHaveBeenCalledTimes(2);
  });
});
