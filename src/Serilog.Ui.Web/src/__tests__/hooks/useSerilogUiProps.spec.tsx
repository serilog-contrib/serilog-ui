import type { ReactNode } from 'react';
import { renderHook as testingLibraryRenderHook } from '@testing-library/react';
import { renderHookSerilogUiTestWrapper as renderHookCustom } from '__tests__/_setup/testing-utils';
import { SerilogUiPropsProvider } from 'app/contexts/SerilogUiPropsProvider';
import { useSerilogUiProps } from 'app/hooks/useSerilogUiProps';
import { AuthType, RemovableColumns } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const TestProvider = ({ children }: { children: ReactNode }) => (
  <SerilogUiPropsProvider>{children}</SerilogUiPropsProvider>
);

describe('useSerilogUiProps', () => {
  it('decodes and sets dom-defined properties', async () => {
    const { result } = renderHookCustom(() => useSerilogUiProps(), {
      authType: AuthType.Custom,
      columnsInfo: {
        key: {
          additionalColumns: [],
          removedColumns: [RemovableColumns.exception],
        },
      },
    });

    expect(result.current.authType).toBe(AuthType.Custom);
    expect(result.current.authenticatedFromAccessDenied).toBeFalsy();
    expect(result.current.blockHomeAccess).toBeFalsy();
    expect(result.current.columnsInfo).toStrictEqual({
      key: {
        additionalColumns: [],
        removedColumns: [RemovableColumns.exception],
      },
    });
    expect(result.current.disabledSortOnKeys).toStrictEqual([
      'disabled-sort-db',
    ]);
    expect(result.current.homeUrl).toBe('https://test-google.com');
    expect(result.current.isUtc).toBeFalsy();
    expect(result.current.routePrefix).toBe('test-serilog-ui');
    expect(result.current.showBrand).toBeUndefined();
  });

  it('sets defaults if no decodable id is found', () => {
    const { result } = testingLibraryRenderHook(() => useSerilogUiProps(), {
      wrapper: ({ children }: { children: ReactNode }) => (
        <TestProvider>{children}</TestProvider>
      ),
    });

    expect(result.current.authType).toBe(AuthType.Jwt);
    expect(result.current.blockHomeAccess).toBeFalsy();
    expect(result.current.columnsInfo).toStrictEqual({});
    expect(result.current.disabledSortOnKeys).toStrictEqual([]);
    expect(result.current.homeUrl).toBe('https://google.com');
    expect(result.current.routePrefix).toBe('serilog-ui');
    expect(result.current.showBrand).toBeTruthy();
  });

  it('sets defaults if decodable id fails parse', () => {
    const warnMock = vi.fn();
    console.warn = warnMock;

    const { result, rerender } = testingLibraryRenderHook(
      () => useSerilogUiProps(),
      {
        wrapper: ({ children }: { children: ReactNode }) => (
          <div>
            <div hidden id='serilog-ui-props'>
              some text that is not a json...
            </div>
            <div>
              <TestProvider>{children}</TestProvider>
            </div>
          </div>
        ),
        initialProps: undefined,
      },
    );
    rerender();

    expect(warnMock).toHaveBeenCalledOnce();

    expect(result.current.authType).toBe(AuthType.Jwt);
    expect(result.current.blockHomeAccess).toBeFalsy();
    expect(result.current.columnsInfo).toStrictEqual({});
    expect(result.current.disabledSortOnKeys).toStrictEqual([]);
    expect(result.current.homeUrl).toBe('https://google.com');
    expect(result.current.routePrefix).toBe('serilog-ui');
    expect(result.current.showBrand).toBeTruthy();
  });
});
