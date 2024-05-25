import { render, screen } from '__tests__/_setup/testing-utils';
import Paging from 'app/components/Search/Paging';
import { SearchResult } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const defaultReturn: () => SearchResult = () => ({
  count: 10,
  currentPage: 1,
  logs: [],
  total: 1,
});
const mockQueryLogs = {
  data: defaultReturn(),
  refetch: vi.fn(),
};
vi.mock('../../../app/hooks/useQueryLogs', async () => {
  return {
    default: () => mockQueryLogs,
  };
});

describe('Paging', () => {
  it('renders correctly', () => {
    render(<Paging />);

    expect(screen.getByLabelText('paging-left-column')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: '1' })).toBeInTheDocument();
  });
});
