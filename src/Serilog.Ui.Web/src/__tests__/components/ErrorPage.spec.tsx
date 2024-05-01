import { render, screen } from '__tests__/_setup/testing-utils';
import { ErrorBoundaryPage } from 'app/components/ErrorPage';
import { describe, expect, it, vi } from 'vitest';

const mockFn = vi.hoisted(() => vi.fn());
vi.mock('react-router-dom', async () => {
  return {
    Link: ({ children }) => <a>{children}</a>,
    isRouteErrorResponse: mockFn,
    useRouteError: () => '',
  };
});

describe('ErrorPage', () => {
  it('renders unexpected error', async () => {
    render(<ErrorBoundaryPage />);

    expect(
      screen.getByText('unexpected error occurred', { exact: false }),
    ).toBeInTheDocument();
  });

  it('renders route error response', async () => {
    mockFn.mockImplementation(() => true);
    render(<ErrorBoundaryPage />);

    expect(
      screen.queryByText('unexpected error occurred', { exact: false }),
    ).not.toBeInTheDocument();
    expect(screen.queryByText('Page not found', { exact: false })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Home' })).toBeInTheDocument();
  });
});
