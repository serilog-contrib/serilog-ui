import { vi } from 'vitest';

Object.defineProperty(window, 'alert', {
  writable: true,
  value: vi.fn(),
});

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
