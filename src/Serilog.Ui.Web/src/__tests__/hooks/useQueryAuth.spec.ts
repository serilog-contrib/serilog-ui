import { renderHook, waitFor } from '__tests__/_setup/testing-utils';
import { useQueryAuth } from 'app/hooks/useQueryAuth';
import { describe, expect, it, vi } from 'vitest';

const mockedProps = {
  blockHomeAccess: true,
  setAuthenticatedFromAccessDenied: vi.fn(),
};
const mockAuthProps = {
  authHeader: '',
};
const mockTableKeys = {
  refetch: vi.fn(),
};

vi.mock('../../app/hooks/useSerilogUiProps', () => ({
  useSerilogUiProps: () => mockedProps,
}));
vi.mock('../../app/hooks/useAuthProperties', () => ({
  useAuthProperties: () => mockAuthProps,
}));
vi.mock('../../app/hooks/useQueryTableKeys', () => ({
  useQueryTableKeys: () => mockTableKeys,
}));

describe('useQueryAuth', () => {
  it('not runs request if home is not blocked', () => {
    mockedProps.blockHomeAccess = false;

    renderHook(() => useQueryAuth());

    expect(mockedProps.setAuthenticatedFromAccessDenied).not.toHaveBeenCalled();
    expect(mockTableKeys.refetch).not.toHaveBeenCalled();
  });

  it('not runs request if home is blocked but header is not ready', () => {
    mockedProps.blockHomeAccess = true;

    renderHook(() => useQueryAuth());

    expect(mockedProps.setAuthenticatedFromAccessDenied).toHaveBeenCalledWith(false);
    expect(mockTableKeys.refetch).not.toHaveBeenCalled();
  });

  it.each([[], null])(
    'runs request and set false if response is not an array with element',
    async (response) => {
      mockedProps.blockHomeAccess = true;
      mockAuthProps.authHeader = 'ready';
      mockTableKeys.refetch.mockResolvedValueOnce({ data: response });

      renderHook(() => useQueryAuth());

      expect(mockTableKeys.refetch).toHaveBeenCalledOnce();
      await waitFor(() => {
        expect(mockedProps.setAuthenticatedFromAccessDenied).toHaveBeenCalledWith(false);
      });
    },
  );

  it('runs request and set true if response is array with element', async () => {
    mockedProps.blockHomeAccess = true;
    mockAuthProps.authHeader = 'ready';
    mockTableKeys.refetch.mockResolvedValueOnce({ data: ['key'] });

    renderHook(() => useQueryAuth());

    expect(mockTableKeys.refetch).toHaveBeenCalledOnce();
    await waitFor(() => {
      expect(mockedProps.setAuthenticatedFromAccessDenied).toHaveBeenCalledWith(true);
    });
  });
});
