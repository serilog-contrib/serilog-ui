import { render, screen } from '__tests__/_setup/testing-utils';
import BasicModal from 'app/components/Authorization/BasicModal';
import { searchFormInitialValues } from 'app/hooks/useSearchForm';
import { FormProvider, useForm } from 'react-hook-form';
import { SearchForm } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

describe('Basic Modal', () => {
  it('renders', () => {
    const methods = useForm<SearchForm>({
      defaultValues: searchFormInitialValues,
    });
    render(
      <FormProvider {...methods}>
        <BasicModal onClose={vi.fn()} />
      </FormProvider>,
    );

    expect(screen.getAllByRole('button')).toHaveLength(2);
  });
});
