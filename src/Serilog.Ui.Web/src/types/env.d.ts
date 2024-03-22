/// <reference types="vite/client" />

import { UserEvent } from '@testing-library/user-event';

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
