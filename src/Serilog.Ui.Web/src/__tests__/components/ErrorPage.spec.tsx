import { render, screen } from '__tests__/_setup/testing-utils';
import { ErrorBoundaryPage } from 'app/components/ErrorPage';
import { ReactNode } from 'react';
import { describe, expect, it, vi } from 'vitest';

const mockFn = vi.hoisted(() => vi.fn());
vi.mock('react-router', async () => {
  return {
    Link: ({ children }: { children: ReactNode }) => <a>{children}</a>,
    isRouteErrorResponse: mockFn,
    useRouteError: () => '',
  };
});

describe('ErrorPage', () => {
  it('renders unexpected error', async () => {
    render(<ErrorBoundaryPage />);

    expect(screen.getByText(/unexpected error occurred/i)).toBeInTheDocument();
  });

  it('renders route error response', async () => {
    mockFn.mockImplementation(() => true);
    render(<ErrorBoundaryPage />);

    expect(screen.queryByText(/unexpected error occurred/i)).not.toBeInTheDocument();
    expect(screen.getByText(/Page not found/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Home' })).toBeInTheDocument();
  });
});
