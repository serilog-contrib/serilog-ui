/// <reference types="vite/client" />

import { UserEvent } from '@testing-library/user-event';
import { ToBeSameDate } from '__tests__/_setup/vitest-extended';

interface ImportMetaEnv {
  readonly VITE_APP_TITLE: string;
  // more env variables...
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}

declare global {
  export interface Window {
    userEventLibApi: UserEvent;
  }
}

interface CustomAsymmetricMatchers {
  toBeSameDate: ToBeSameDate;
}

declare module 'vitest' {
  interface AsymmetricMatchersContaining extends CustomAsymmetricMatchers {
    toBeSameDate: ToBeSameDate;
  }
}
