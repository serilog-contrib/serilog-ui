import { useEffect, useRef } from 'react';
import { useSearchParams } from 'react-router';
import { parseSearchParams, serializeSearchParams } from 'app/util/queryParams';
import { useSearchForm } from './useSearchForm';
import { useQueryTableKeys } from './useQueryTableKeys';
import { isArrayGuard } from 'app/util/guards';
import type { SearchForm } from '../../types/types';

/**
 * Custom hook to synchronize form state with URL query parameters
 */
export const useQueryParamSync = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const { setValue, watch, getValues } = useSearchForm();
  const { data: tableKeys } = useQueryTableKeys();
  const isInitialized = useRef(false);

  // Initialize form from URL params on mount
  useEffect(() => {
    if (isInitialized.current || !tableKeys || !isArrayGuard(tableKeys)) return;

    const urlParams = parseSearchParams(searchParams);
    
    // If there are URL params, initialize the form with them
    if (Object.keys(urlParams).length > 0) {
      const tableKeysDefaultValue = tableKeys.at(0)!;
      
      // Validate and apply URL params to the form
      Object.entries(urlParams).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          // Special handling for table - ensure it's valid
          if (key === 'table') {
            if (typeof value === 'string' && tableKeys.includes(value)) {
              setValue(key as keyof SearchForm, value);
            }
          } else {
            setValue(key as keyof SearchForm, value as SearchForm[keyof SearchForm]);
          }
        }
      });
      
      // Ensure table has a value if not set from URL
      const currentTable = getValues('table');
      if (!currentTable) {
        setValue('table', tableKeysDefaultValue);
      }
    }
    
    isInitialized.current = true;
  }, [tableKeys, searchParams, setValue, getValues]);

  // Update URL when form values change
  useEffect(() => {
    if (!isInitialized.current) return;

    const subscription = watch((formValues) => {
      const params = serializeSearchParams(formValues as SearchForm);
      
      // Only update if params are different from current URL
      if (params.toString() !== searchParams.toString()) {
        setSearchParams(params, { replace: true });
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, searchParams, setSearchParams]);
};
