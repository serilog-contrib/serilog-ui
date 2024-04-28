import { handlers } from '__tests__/_setup/mocks/fetch';
import { setupServer } from 'msw/node';

export const server = setupServer(...handlers);
