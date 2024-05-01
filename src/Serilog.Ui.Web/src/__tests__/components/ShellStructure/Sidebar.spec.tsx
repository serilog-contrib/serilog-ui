import { render, screen, waitFor } from '__tests__/_setup/testing-utils';
import Sidebar from 'app/components/ShellStructure/Sidebar';
import { describe, expect, it, vi } from 'vitest';

const propsMock = {
  showBrand: true,
};
vi.mock('../../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('Sidebar', () => {
  it('renders', async () => {
    window.innerWidth = 350;

    render(<Sidebar />);

    expect(screen.getByRole('button', { name: 'Home', hidden: true })).toBeInTheDocument;

    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Filter', hidden: true }))
        .toBeInTheDocument;
    });
    await waitFor(() => {
      expect(screen.getByLabelText('paging-left-column')).toBeInTheDocument();
    });
    await waitFor(() => {
      expect(screen.getByText('Serilog UI')).toBeInTheDocument();
    });
  });
});
