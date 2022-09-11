import { setupServer } from 'msw/node';
import { handlers } from './fetchMock';

export const server = setupServer(...handlers);
