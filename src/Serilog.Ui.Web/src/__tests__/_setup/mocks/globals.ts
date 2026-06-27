import { vi } from 'vitest';
import '@testing-library/jest-dom/vitest';

const { getComputedStyle: getCompStyle } = window;
window.getComputedStyle = (elt) => getCompStyle(elt);

class ResizeObserverMock {
  observe() {}
  unobserve() {}
  disconnect() {}
}

window.ResizeObserver = ResizeObserverMock;

// https://blog.lysender.com/2023/06/jest-react-testing-window-matchmedia-is-not-a-function/
Object.defineProperty(window, 'matchMedia', {
  writable: true,
  value: (query) => ({
    matches: false,
    media: query,
    onchange: null,
    addListener: vi.fn(),
    removeListener: vi.fn(),
    addEventListener: vi.fn(),
    removeEventListener: vi.fn(),
    dispatchEvent: vi.fn(),
  }),
});

Object.defineProperty(window.document, 'fonts', {
  writable: true,
  value: {
    addEventListener: () => {},
    removeEventListener: () => {},
  },
});
