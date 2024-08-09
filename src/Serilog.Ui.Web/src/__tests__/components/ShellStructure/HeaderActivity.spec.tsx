import { render, screen, waitFor } from '__tests__/_setup/testing-utils';
import HeaderActivity from 'app/components/ShellStructure/HeaderActivity';
import { AuthType } from 'types/types';
import { describe, expect, it, vi } from 'vitest';

describe('HeaderActivity', () => {
  it('renders', async () => {
    window.innerWidth = 800;

    render(<HeaderActivity toggleMobile={vi.fn()} />, AuthType.Jwt);

    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Authorize' })).toBeInTheDocument();
    });
    await waitFor(() => {
      expect(screen.getByRole('button', { name: 'Filter' })).toBeInTheDocument();
    });
  });
});
