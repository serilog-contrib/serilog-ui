import { vi } from 'vitest';

// ref: https://jestjs.io/docs/manual-mocks#mocking-methods-which-are-not-implemented-in-jsdom
// ref: https://github.com/jsdom/jsdom/issues/2524

// TODO: check if required for testing

// Object.defineProperty(window, 'TextEncoder', {
//   writable: true,
//   value: util.TextEncoder,
// });
// Object.defineProperty(window, 'TextDecoder', {
//   writable: true,
//   value: util.TextDecoder,
// });

Object.defineProperty(window, 'alert', {
  writable: true,
  value: vi.fn(),
});
