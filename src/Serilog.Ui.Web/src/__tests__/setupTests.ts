import userEvent, { UserEvent } from '@testing-library/user-event';
import '__tests__/_setup/mocks/globals';
import { server } from '__tests__/_setup/msw-server';
import { afterAll, afterEach, beforeAll } from 'vitest';

export let userEventInstance: UserEvent;

// Establish API mocking before all tests.
beforeAll(() => {
  userEventInstance = userEvent.setup();
  server.listen();
});

// Clean up after the tests are finished.
afterAll(() => {
  server.close();
});

// Reset any request handlers that we may add during the tests,
// so they don't affect other tests.
afterEach(() => server.resetHandlers());
