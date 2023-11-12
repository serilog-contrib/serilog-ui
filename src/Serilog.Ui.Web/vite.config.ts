/// <reference types="vitest" />

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import viteTsconfigPaths from 'vite-tsconfig-paths';
import eslint from 'vite-plugin-eslint';
import mkcert from 'vite-plugin-mkcert';
import { checker } from 'vite-plugin-checker';

// https://vitejs.dev/config/
export default defineConfig({
  root: './src',
  build: {
    outDir: '../wwwroot/dist',
    sourcemap: false,
    rollupOptions: {
      external: ['**/__tests__/**/*', '**/*.{spec,test}.*', '**/mocks/**/*.*'],
    },
  },
  plugins: [
    react(),
    viteTsconfigPaths(),
    eslint(),
    mkcert(),
    checker({
      typescript: true,
    }),
  ],
  server: {
    https: true,
    open: false,
    port: 3001,
  },
  test: {
    environment: 'jsdom',
    globals: true,
    restoreMocks: true,
    setupFiles: './src/__tests__/setupTests.ts',
    outputFile: {
      'vitest-sonar-reporter': './reports/test-report.xml',
    },
    coverage: {
      provider: 'istanbul',
      reporter: 'lcov',
      reportsDirectory: './reports/coverage/',
      reportOnFailure: true,
    },
  },
});
