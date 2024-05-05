import { render, within } from '@testing-library/react';
import { developmentListenersHost } from '__tests__/_setup/mocks/fetch';
import { act, render as customRender } from '__tests__/_setup/testing-utils';
import App from 'app/App';
import { routes } from 'app/routes';
import { Window } from 'happy-dom';
import { RouterProvider, createMemoryRouter } from 'react-router-dom';
import { afterAll, beforeAll, describe, expect, it, vi } from 'vitest';

const propsMock = {
  routePrefix: '',
  blockHomeAccess: false,
  setAuthenticatedFromAccessDenied: vi.fn(),
};
vi.mock('../app/hooks/useSerilogUiProps', () => {
  return {
    useSerilogUiProps: () => propsMock,
  };
});

describe('App', () => {
  let fakeHappyDomWindow: Window;

  beforeAll(async () => {
    vi.useFakeTimers();

    fakeHappyDomWindow = new Window({
      url: `${developmentListenersHost}/test-serilog-ui/`,
    });
    await fakeHappyDomWindow.happyDOM.waitUntilComplete();

    window.document = fakeHappyDomWindow.document as unknown as never;
  });
  afterAll(async () => {
    vi.useRealTimers();

    await fakeHappyDomWindow.happyDOM.abort();
    fakeHappyDomWindow.close();
  });

  it('renders null without route prefix', async () => {
    const { container } = render(
      <div>
        <App />
      </div>,
    );

    await act(async () => {
      await vi.runAllTimersAsync();
    });

    expect(within(container).queryByAltText('main-app')).not.toBeInTheDocument();
  });

  it('renders main app', async () => {
    propsMock.routePrefix = 'test-serilog-ui';

    const { container } = render(
      <div>
        <App />
      </div>,
    );

    await act(async () => {
      await vi.runAllTimersAsync();
    });

    expect(within(container).getByLabelText('main-app')).toBeInTheDocument();

    propsMock.routePrefix = '';
  });

  describe('routes', () => {
    it('sends to base routes', async () => {
      const router = createMemoryRouter(routes, {
        basename: `/test-serilog-ui/`,
        initialEntries: ['/test-serilog-ui/'],
      });
      const { container } = customRender(<RouterProvider router={router} />);

      await act(async () => {
        await vi.runAllTimersAsync();
      });

      expect(within(container).getByLabelText('main-app')).toBeInTheDocument();
    });

    it('sends to unauthorized route', async () => {
      propsMock.blockHomeAccess = true;
      const router = createMemoryRouter(routes, {
        basename: `/test-serilog-ui/`,
        initialEntries: ['/test-serilog-ui/access-denied/'],
      });
      const { container } = customRender(<RouterProvider router={router} />);

      await act(async () => {
        await vi.runAllTimersAsync();
      });

      expect(within(container).queryByLabelText('main-app')).not.toBeInTheDocument();
      expect(within(container).getByText(/not authorized/i)).toBeInTheDocument();
      propsMock.blockHomeAccess = false;
    });

    it('sends to error route on not found route', async () => {
      const router = createMemoryRouter(routes, {
        basename: `/test-serilog-ui/`,
        initialEntries: ['/test-serilog-ui/not-found-route'],
      });
      const { container } = customRender(<RouterProvider router={router} />);

      await act(async () => {
        await vi.runAllTimersAsync();
      });

      expect(within(container).queryByLabelText('main-app')).not.toBeInTheDocument();
      expect(within(container).getByText(/Page not found/im)).toBeInTheDocument();
    });
  });
});
