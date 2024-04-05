import { AdditionalColumn, ColumnType, LogType, RemovableColumns } from 'types/types';
import { useSearchForm } from './useSearchForm';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useColumnsInfo = (currentColumn = '', logPropertyType = '') => {
  const { getValues } = useSearchForm();
  const { columnsInfo } = useSerilogUiProps();

  const currentTable = getValues('table');
  const hasInfoOnCurrentTable = columnsInfo && columnsInfo[currentTable];

  // treating Properties as an additional column, in the rendering
  const propertiesColumn: AdditionalColumn[] = [
    {
      name: RemovableColumns.properties,
      typeName: ColumnType.code,
      codeType: logPropertyType as LogType,
    },
  ];
  const additionalColumns = hasInfoOnCurrentTable
    ? [...columnsInfo[currentTable].AdditionalColumns, ...propertiesColumn]
    : propertiesColumn;

  const additionalColumn = additionalColumns.find((p) => p.name === currentColumn);

  const removeColumn = (col: RemovableColumns) =>
    hasInfoOnCurrentTable && columnsInfo[currentTable].RemovedColumns.includes(col);

  const removeException = removeColumn(RemovableColumns.exception);

  const removeProperties =
    removeColumn(RemovableColumns.properties) &&
    (!currentColumn || currentColumn === RemovableColumns.properties);

  return { additionalColumn, removeException, removeProperties };
};
