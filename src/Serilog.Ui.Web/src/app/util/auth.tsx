import { Text } from '@mantine/core';
import { isValidJwtUnixDate } from 'app/util/dates';
import { decodeJwt } from 'jose';
import { AuthType } from 'types/types';
import { sendUnexpectedNotification } from './queries';

export interface IAuthPropertiesData {
  jwt_bearerToken?: string;
  basic_user?: string;
  basic_pwd?: string;
}

export const IAuthPropertiesStorageKeys = {
  basic_user: 'serilog_ui_basic_user',
  jwt_bearerToken: 'serilog_ui_jwt_bearerToken',
  basic_pwd: '',
} as const;

export const defaultAuthProps = {
  basic_pwd: '',
  basic_user: '',
  jwt_bearerToken: '',
};

const notSavedKeys: (keyof IAuthPropertiesData)[] = ['basic_pwd'];

export const clearAuth = () => {
  Object.keys(IAuthPropertiesStorageKeys).forEach((key) => {
    sessionStorage.removeItem(IAuthPropertiesStorageKeys[key]);
  });

  return defaultAuthProps;
};

export const getAuthKey = (
  props: IAuthPropertiesData,
  key: keyof IAuthPropertiesData,
) => {
  return props[key] || sessionStorage.getItem(IAuthPropertiesStorageKeys[key]) || '';
};

export const getAuthorizationHeader = (
  authType: string = '',
  props: IAuthPropertiesData,
) => {
  const authTypeToEnum = AuthType[authType];

  switch (authTypeToEnum) {
    case AuthType.Jwt:
      return props?.jwt_bearerToken ? `Bearer ${props.jwt_bearerToken}` : '';
    case AuthType.Basic:
      if (!props.basic_user || !props.basic_pwd) {
        return '';
      }

      const encodeProps = btoa(`${props.basic_user}:${props.basic_pwd}`);
      return `Basic ${encodeProps}`;
    default:
      return '';
  }
};

export const initialAuthProps: () => IAuthPropertiesData = () => ({
  basic_pwd: '',
  basic_user: getAuthKey({}, 'basic_user'),
  jwt_bearerToken: getAuthKey({}, 'jwt_bearerToken'),
});

const setAuthDataValue = (
  props: IAuthPropertiesData,
  key: keyof IAuthPropertiesData,
  value: string,
) => {
  props[key] = value;

  if (!notSavedKeys.includes(key)) {
    sessionStorage.setItem(IAuthPropertiesStorageKeys[key], value);
  }
};

/**
 * Validate a value based on the data key.
 */
const validateKey = (key: keyof IAuthPropertiesData, value: string) => {
  switch (key) {
    case 'jwt_bearerToken':
      return validateBearerToken(value);
    default:
      return { success: true, error: '' };
  }
};

/**
 * Validate a bearer token.
 */
const validateBearerToken = (bearerToken: string) => {
  try {
    const decoded = decodeJwt(bearerToken);
    const isValidExp = !decoded.exp || isValidJwtUnixDate(decoded.exp);

    return { success: isValidExp, error: 'Token expired' };
  } catch {
    return { success: false, error: 'Token invalid' };
  }
};

export const saveAuthKey = (
  props: IAuthPropertiesData,
  key: keyof IAuthPropertiesData,
  value: string,
) => {
  const validation = validateKey(key, value);

  setAuthDataValue(props, key, value);

  return { success: validation.success, error: validation.error };
};

export const checkErrors = ({
  success,
  errors,
}: {
  success: boolean;
  errors?: string[];
}) => {
  if (!success) {
    sendUnexpectedNotification(
      <Text ta="justify">
        Your authorization data could be invalid, we noticed the following errors:
        <br />
        {errors?.join(', ')}
      </Text>,
      'Auth validation',
      'yellow',
      false,
    );
  }
};
