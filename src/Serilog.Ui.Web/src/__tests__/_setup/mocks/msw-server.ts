import { setupServer } from 'msw/node';
import { handlers } from './fetch';

export const server = setupServer(...handlers);
