import { setupServer } from 'msw/node';
import { handlers } from './mocks/fetchMock';

export const server = setupServer(...handlers);
