import { setupWorker } from 'msw/browser';
import { handlers } from './mocks/fetchMock';

// This configures a Service Worker with the given request handlers.
export const worker = setupWorker(...handlers);
