import { render, screen } from '__tests__/_setup/testing-utils';
import { describe, expect, it, vi } from 'vitest';
import { HomePageNotAuthorized } from '../../app/components/HomePageNotAuthorized';

const propsMock = {
  authenticatedFromAccessDenied: false,
  setAuthenticatedFromAccessDenied: vi.fn(),
  blockHomeAccess: false,
};
vi.mock('react-router', async () => {
  return {
    Navigate: () => <div>Move to</div>,
  };
});
vi.mock('../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('HomePageNotAuthorized', () => {
  it('renders navigate component if block home is false', () => {
    render(<HomePageNotAuthorized />);

    expect(screen.getByText('Move to')).toBeInTheDocument();
  });

  it('renders navigate component if authenticated from access denied is true', () => {
    propsMock.authenticatedFromAccessDenied = true;
    propsMock.blockHomeAccess = true;

    render(<HomePageNotAuthorized />);

    expect(screen.getByText('Move to')).toBeInTheDocument();

    propsMock.authenticatedFromAccessDenied = false;
    propsMock.blockHomeAccess = false;
  });

  it('renders unauthorized component if block from access is true and authenticated is false', () => {
    propsMock.blockHomeAccess = true;

    render(<HomePageNotAuthorized />);

    expect(screen.queryByText('Move to')).not.toBeInTheDocument();
    expect(screen.getByText(/not authorized/i)).toBeInTheDocument();

    propsMock.blockHomeAccess = false;
  });
});
