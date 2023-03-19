/** @type {import('ts-jest/dist/types').InitialOptionsTsJest} */
module.exports = {
  preset: 'ts-jest',
  setupFilesAfterEnv: ['<rootDir>/assets/__tests__/util/jest-setup.ts'],
  testEnvironment: 'jsdom',
  testPathIgnorePatterns: ['<rootDir>/assets/__tests__/util/'],
  testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.[aajt]sx?$',
};