import { act, render, within } from '__tests__/_setup/testing-utils';
import { Index } from 'app/components/Index';
import { MemoryRouter } from 'react-router';
import { describe, expect, it, vi } from 'vitest';

const propsMock = {
  routePrefix: '',
  blockHomeAccess: false,
  setAuthenticatedFromAccessDenied: vi.fn(),
};
vi.mock('../../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('Index', () => {
  it('renders', async () => {
    vi.useFakeTimers();
    const { container } = render(<Index />);

    await act(async () => {
      await vi.runAllTimersAsync();
    });

    expect(within(container).getByLabelText('main-app')).toBeInTheDocument();

    vi.useRealTimers();
  });

  it('not renders main app', async () => {
    propsMock.blockHomeAccess = true;

    const { container } = render(
      <MemoryRouter>
        <Index />
      </MemoryRouter>,
    );

    expect(within(container).queryByLabelText('main-app')).not.toBeInTheDocument();
    propsMock.blockHomeAccess = false;
  });
});
