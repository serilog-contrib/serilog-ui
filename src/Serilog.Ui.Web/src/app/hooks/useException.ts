import { useMemo } from 'react';
import { useColumnsInfo } from './useColumnsInfo';
import { useSearchForm } from './useSearchForm';
import { useSerilogUiProps } from './useSerilogUiProps';

export const useException = (logException?: string, logPropertyType?: string) => {
  const { renderExceptionAsStringKeys } = useSerilogUiProps();
  const { removeException } = useColumnsInfo();
  const { watch } = useSearchForm();

  const currentDbKey = watch('table');

  const isExceptionAsString = useMemo(
    () =>
      !!renderExceptionAsStringKeys?.length &&
      !!currentDbKey &&
      renderExceptionAsStringKeys.includes(currentDbKey),
    [currentDbKey, renderExceptionAsStringKeys],
  );

  const exceptionContent = !isExceptionAsString
    ? logException
    : logException?.replace(' at ', '\nat ');
  const logType = !isExceptionAsString ? logPropertyType : 'string';

  return {
    exceptionContent,
    logType,
    removeException,
  };
};
