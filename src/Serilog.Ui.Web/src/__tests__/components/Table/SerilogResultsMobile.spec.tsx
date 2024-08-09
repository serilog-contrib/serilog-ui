/* eslint-disable testing-library/no-container */
import { fakeLogs } from '__tests__/_setup/mocks/samples';
import { render, screen } from '__tests__/_setup/testing-utils';
import SerilogResultsMobile from 'app/components/Table/SerilogResultsMobile';
import { describe, expect, it, vi } from 'vitest';

const mockQuery = {
  data: null as object | null,
  refetch: vi.fn(),
};
vi.mock('../../../app/hooks/useQueryLogs', () => ({
  default: () => mockQuery,
}));

describe('SerilogResultsMobile', () => {
  it('renders', () => {
    render(<SerilogResultsMobile />);

    expect(screen.getByRole('blockquote')).toBeInTheDocument();
    expect(screen.getByText(/No results/)).toBeInTheDocument();
  });

  it('renders with data', () => {
    const sample = fakeLogs.logs.slice(0, 1);
    mockQuery.data = { ...fakeLogs, logs: sample };
    render(<SerilogResultsMobile />);

    expect(screen.queryByText(/No results/i)).not.toBeInTheDocument();
    expect(screen.getByText(sample[0].rowNo)).toBeInTheDocument();
  });
});
