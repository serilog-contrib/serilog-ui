// https://stackoverflow.com/a/64940749/15129749
export const isNotNullGuard = <T>(value?: T | null): value is T => value !== null;

export const isStringGuard = (
  value?: string | boolean | number | null,
): value is string => (value ?? '') !== '' && typeof value === 'string';

export const isArrayGuard = <T>(value?: T[] | null): value is T[] =>
  (value?.length ?? 0) > 0;

export const isObjectGuard = <T>(value?: T | null): value is T =>
  (value as T) !== undefined && isNotNullGuard(value);

export const toNumber = (value: string) => {
  const numberTransform = Number.parseInt(value, 10);
  return Number.isInteger(numberTransform) ? numberTransform : null;
};
