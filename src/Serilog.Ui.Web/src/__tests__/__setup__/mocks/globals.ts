window.ResizeObserver =
  window.ResizeObserver ||
  class ResizeObserver {
    disconnect(): void {
      /* noop */
    }
    observe(): void {
      /* noop */
    }
    unobserve(): void {
      /* noop */
    }
  };

// https://blog.lysender.com/2023/06/jest-react-testing-window-matchmedia-is-not-a-function/
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: (query) => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: () => {},
    removeListener: () => {},
    addEventListener: () => {},
    removeEventListener: () => {},
    dispatchEvent: () => {},
  }),
});
