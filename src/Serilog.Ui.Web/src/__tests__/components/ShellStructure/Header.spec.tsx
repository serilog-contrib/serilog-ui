import { render, screen, waitFor } from '__tests__/_setup/testing-utils';
import Head from 'app/components/ShellStructure/Header';
import { AuthType } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

const propsMock = {
  showBrand: true,
};
vi.mock('../../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('Header', () => {
  it('renders', async () => {
    window.innerWidth = 800;

    render(<Head toggleMobile={vi.fn()} isMobileOpen />, AuthType.Jwt);

    expect(screen.getByLabelText('home-icon-btn')).toBeInTheDocument();
    expect(screen.getByLabelText('color-scheme-changer')).toBeInTheDocument();
    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Filter' })).toBeInTheDocument();
    });
    await waitFor(() => {
      expect(screen.getByText('Serilog UI')).toBeInTheDocument();
    });
  });
});
