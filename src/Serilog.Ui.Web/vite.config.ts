/// <reference types="vitest" />

import react from '@vitejs/plugin-react-swc';
import { defineConfig } from 'vite';
import { checker } from 'vite-plugin-checker';
import mkcert from 'vite-plugin-mkcert';
import viteTsconfigPaths from 'vite-tsconfig-paths';
import type { UserConfig as VitestUserConfigInterface } from 'vitest/config';

const vitestConfig: VitestUserConfigInterface = {
  test: {
    environment: 'happy-dom',
    environmentOptions: {
      happyDOM: {
        url: 'https://localhost:3001',
      },
    },
    globals: true,
    restoreMocks: true,
    setupFiles: './__tests__/_setup/setupTests.ts',
    outputFile: {
      'vitest-sonar-reporter': './reports/test-report.xml',
      junit: './reports/test-junit-report.xml',
    },
    coverage: {
      provider: 'istanbul',
      reporter: 'lcov',
      reportsDirectory: './reports/coverage/',
      reportOnFailure: true,
    },
  },
};

// https://vitejs.dev/config/
export default defineConfig((env) => ({
  base: './',
  root: './src',
  build: {
    outDir: '../wwwroot/dist',
    emptyOutDir: true,
    sourcemap: false,
    rollupOptions: {
      external: ['**/__tests__/**/*', '**/*.{spec,test}.*', '**/mocks/**/*.*'],
      output: {
        manualChunks(id) {
          if (id.includes('node_modules')) {
            return id.toString().split('node_modules/')[1].split('/')[0].toString();
          }
        },
      },
    },
  },
  plugins: [
    env.mode !== 'development' && react(),
    env.mode === 'development' &&
      react({
        jsxImportSource: '@welldone-software/why-did-you-render',
      }),
    viteTsconfigPaths(),
    mkcert(),
    env.mode !== 'test' &&
      checker({
        typescript: true,
        eslint: {
          lintCommand: 'eslint',
        },
      }),
  ],
  server: {
    open: 'serilog-ui/',
    port: 3001,
  },

  test: vitestConfig.test,
}));
