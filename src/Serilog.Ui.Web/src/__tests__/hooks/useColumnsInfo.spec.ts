import { renderHook } from '__tests__/_setup/testing-utils';
import { useColumnsInfo } from 'app/hooks/useColumnsInfo';
import {
  AdditionalColumnLogType,
  ColumnType,
  LogType,
  RemovableColumns,
} from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const mockForm = {
  getValues: vi.fn(),
};
vi.mock('../../app/hooks/useSearchForm', () => ({
  useSearchForm: () => mockForm,
}));

describe('useColumnsInfo', () => {
  const sample = {
    db_key: {
      additionalColumns: [
        {
          name: 'column2',
          typeName: ColumnType.string,
          codeType: AdditionalColumnLogType.None,
        },
        {
          name: 'column',
          typeName: ColumnType.datetime,
          codeType: AdditionalColumnLogType.None,
        },
      ],
      removedColumns: [RemovableColumns.exception, RemovableColumns.properties],
    },
  };

  it('renders without additional data with not found key', () => {
    mockForm.getValues.mockReturnValue('db_key_2');

    const { result } = renderHook(() => useColumnsInfo(), { columnsInfo: sample });

    expect(result.current.additionalColumn).toBeUndefined();
    expect(result.current.removeException).toBeFalsy();
    expect(result.current.removeProperties).toBeFalsy();
  });

  it('finds info from table key', () => {
    mockForm.getValues.mockReturnValue('db_key');

    const { result } = renderHook(() => useColumnsInfo('column'), {
      columnsInfo: sample,
    });

    expect(result.current.additionalColumn).toStrictEqual(
      sample.db_key.additionalColumns[1],
    );
    expect(result.current.removeException).toBeTruthy();
    expect(result.current.removeProperties).toBeFalsy();
  });

  it.each([undefined, RemovableColumns.properties])(
    'remove properties column if in removedColumns array and current column is %s',
    (currentColumn) => {
      mockForm.getValues.mockReturnValue('db_key');

      const { result } = renderHook(() => useColumnsInfo(currentColumn), {
        columnsInfo: sample,
      });

      expect(result.current.removeProperties).toBeTruthy();
    },
  );

  it.each([LogType.Json, LogType.Xml, 'none'])(
    'returns properties column with provided code type %s conversion',
    (logType) => {
      mockForm.getValues.mockReturnValue('db_key');

      const { result } = renderHook(
        () => useColumnsInfo(RemovableColumns.properties, logType),
        {
          columnsInfo: sample,
        },
      );

      expect(result.current.additionalColumn).toStrictEqual({
        name: RemovableColumns.properties,
        typeName: ColumnType.code,
        codeType: logType === LogType.Json ? 2 : 1,
      });
    },
  );
});
