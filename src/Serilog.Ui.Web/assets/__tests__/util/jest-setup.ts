import './jest-mocks'
import 'whatwg-fetch';
import '@testing-library/jest-dom';
import { server } from './mocks/msw-server';
import { renderSerilogUiDom } from './dom-renderer';

// Establish API mocking before all tests.
beforeAll(() => {
  server.listen();
});

// Clean up after the tests are finished.
afterAll(() => server.close());

// Load the DOM before each test and setup userEvent.
// If the window.config should be overriden, either
// - re-call the method in the test itself with params
// - set the config manually in the test
beforeEach(async () => {
  await renderSerilogUiDom();
});

// Reset any request handlers that we may add during the tests,
// so they don't affect other tests.
afterEach(() => server.resetHandlers());
