/// <reference types="vitest" />

import react from '@vitejs/plugin-react-swc';
import { defineConfig, splitVendorChunkPlugin } from 'vite';
import { checker } from 'vite-plugin-checker';
import eslint from 'vite-plugin-eslint';
import mkcert from 'vite-plugin-mkcert';
import viteTsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig((env) => ({
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
    env.mode !== 'test' && eslint(),
    env.mode === 'production' && splitVendorChunkPlugin(), // TODO chunking and lazy load
    mkcert(),
    checker({
      typescript: true,
    }),
  ],
  server: {
    open: false,
    port: 3001,
  },
  test: {
    environment: 'jsdom',
    globals: true,
    restoreMocks: true,
    setupFiles: './__tests__/setupTests.ts',
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
}));
