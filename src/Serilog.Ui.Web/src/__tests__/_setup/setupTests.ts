import '__tests__/_setup/mocks/globals';
import { server } from '__tests__/_setup/msw-server';
import { afterAll, afterEach, beforeAll, beforeEach } from 'vitest';
import { UserEvent, userEvent } from './testing-utils';

export let userEventInstance: UserEvent;

// Establish API mocking before all tests.
beforeAll(() => {
  server.listen();
});

// Clean up after the tests are finished.
afterAll(() => {
  server.close();
});

beforeEach(() => {
  userEventInstance = userEvent.setup();
});

// Reset any request handlers that we may add during the tests,
// so they don't affect other tests.
afterEach(() => server.resetHandlers());
