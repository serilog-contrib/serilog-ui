// https://stackoverflow.com/a/64940749/15129749
export const isDefinedGuard = <T>(value?: T | null): value is T => value !== null;

export const isStringGuard = (
  value?: string | boolean | number | null,
): value is string => (value ?? '') !== '' && typeof value === 'string';

export const isArrayGuard = <T>(value?: T[]): value is T[] => (value?.length ?? 0) > 0;

export const isObjectGuard = <T>(value?: T | null): value is T =>
  (value as T) !== undefined && isDefinedGuard(value);
