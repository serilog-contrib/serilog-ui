/* eslint-disable testing-library/no-container */
import { fakeLogs } from '__tests__/_setup/mocks/samples';
import { render, screen } from '__tests__/_setup/testing-utils';
import SerilogResults from 'app/components/Table/SerilogResults';
import { describe, expect, it, vi } from 'vitest';

const mockQuery = {
  data: fakeLogs,
  refetch: vi.fn(),
};
vi.mock('../../../app/hooks/useQueryLogs', () => ({
  default: () => mockQuery,
}));

describe('SerilogResults', () => {
  it('renders', () => {
    const { container } = render(<SerilogResults />);

    expect(screen.getByRole('table')).toBeInTheDocument();
    expect(container.getElementsByTagName('th')).toHaveLength(7);
  });
});
