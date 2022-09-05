import { getBgLogLevel } from '../script/util';
import { LogLevel } from '../types/types';

describe('log level backgrounds', () => {
  it.each([
    [LogLevel.Verbose, 'bg-success'],
    [LogLevel.Debug, 'bg-success'],
    [LogLevel.Error, 'bg-danger'],
    [LogLevel.Fatal, 'bg-danger'],
    [LogLevel.Information, 'bg-primary'],
    [LogLevel.Warning, 'bg-warning'],
  ])('returns %s color for log level: %s', (logLevelEntry, expectedBg) => {
    const result = getBgLogLevel(logLevelEntry);
    expect(result).toBe(expectedBg);
  });

  it('returns default color for unrecognized enum case', () => {
    const result = getBgLogLevel('fakeEnum' as LogLevel);
    expect(result).toBe('bg-secondary');
  });
});
