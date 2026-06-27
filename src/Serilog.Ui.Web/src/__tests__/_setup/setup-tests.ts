/* eslint-disable perfectionist/sort-imports */
import './mocks/globals';
import './vitest-extended';
import type { UserEvent } from './testing-utils';
import { server } from './mocks/msw-server';
import { afterAll, afterEach, beforeAll, beforeEach } from 'vitest';
import { userEvent } from './testing-utils';

// eslint-disable-next-line import/no-mutable-exports
export let userEventInstance: UserEvent

// Establish API mocking before all tests.
beforeAll(() => {
  server.listen();
});

beforeEach(() => {
  userEventInstance = userEvent.setup()
})

// Reset any request handlers that we may add during the tests,
// so they don't affect other tests.
afterEach(() => server.resetHandlers());

// Clean up after the tests are finished.
afterAll(() => {
  server.close();
});
