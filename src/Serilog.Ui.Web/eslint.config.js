import antfu from '@antfu/eslint-config'
import vitest from '@vitest/eslint-plugin'
import testingLibrary from 'eslint-plugin-testing-library'

export default antfu({
  lessOpinionated: true,
  react: true,
  stylistic: {
    quotes: 'single',
    semi: false,
    overrides: {
      'style/semi': 'off',
      'style/brace-style': 'off',
      'style/member-delimiter-style': 'off',
      'antfu/no-top-level-await': 'off',
    },
  },
  ignores: ['obj/*', 'obj/*/**', 'bin/*', 'bin/*/**', 'wwwroot/*', 'wwwroot/*/**', 'coverage/*', 'coverage/*/**', '*.html', '**/*.html/**', 'src/mockServiceWorker.js', 'src/reports/*', 'src/reports/*/**'],
}, {
  files: ['**/__tests__/**/*', '**/*.{spec,test}.*'],
  plugins: { vitest, testingLibrary },
  ...vitest.configs.recommended,
  ...testingLibrary.configs['flat/react'],
  rules: {
    // https://github.com/testing-library/eslint-plugin-testing-library
    'testing-library/await-async-queries': 'error',
    'testing-library/await-async-utils': 'error',
    'testing-library/no-await-sync-queries': 'error',
    'testing-library/no-container': 'error',
    'testing-library/no-debugging-utils': 'error',
    'testing-library/no-dom-import': ['error', 'react'],
    'testing-library/no-node-access': ['error', { allowContainerFirstChild: true }],
    'testing-library/no-promise-in-fire-event': 'error',
    'testing-library/no-render-in-lifecycle': 'error',
    'testing-library/no-unnecessary-act': 'error',
    'testing-library/no-wait-for-multiple-assertions': 'error',
    'testing-library/no-wait-for-side-effects': 'error',
    'testing-library/no-wait-for-snapshot': 'error',
    'testing-library/prefer-find-by': 'error',
    'testing-library/prefer-presence-queries': 'error',
    'testing-library/prefer-query-by-disappearance': 'error',
    'testing-library/prefer-screen-queries': 'error',
    'testing-library/render-result-naming-convention': 'error',
  },
})
