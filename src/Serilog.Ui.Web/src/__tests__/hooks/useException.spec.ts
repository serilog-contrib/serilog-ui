import { renderHook } from '__tests__/_setup/testing-utils';
import { useException } from 'app/hooks/useException';
import { describe, expect, it, vi } from 'vitest';

const mockForm = {
  watch: vi.fn(),
  getValues: vi.fn(),
};
const mockColInfo = { removeException: vi.fn() };
vi.mock('../../app/hooks/useSearchForm', () => ({
  useSearchForm: () => mockForm,
}));
vi.mock('../../app/hooks/useColumnsInfo', () => ({
  useColumnsInfo: () => mockColInfo,
}));

describe('useException', () => {
  it('returns properties untouched', () => {
    mockForm.getValues.mockImplementation(() => 'test-db');
    mockForm.watch.mockImplementation(() => 'test-db');

    const { result } = renderHook(() => useException('my-string untouched', 'log'));

    expect(result.current.logType).toBe('log');
    expect(result.current.exceptionContent).toBe('my-string untouched');
  });

  it('returns string type, with modified content', () => {
    mockForm.getValues.mockImplementation(() => 'exception-string-sample');
    mockForm.watch.mockImplementation(() => 'exception-string-sample');
    mockColInfo.removeException.mockReturnValue(true);

    const { result } = renderHook(() =>
      useException('my-string modified with at sample', 'log'),
    );

    expect(result.current.removeException).toBeTruthy();
    expect(result.current.logType).toBe('string');
    expect(result.current.exceptionContent).toBe('my-string modified with at sample');
  });
});
