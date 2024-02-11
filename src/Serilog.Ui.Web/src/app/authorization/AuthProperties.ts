import { decodeJwt } from 'jose';
import { AuthType } from 'types/types';

export interface IAuthPropertiesData {
  jwt_bearerToken?: string;
  basic_user?: string;
  basic_pwd?: string;
}
const notSavedKeys: (keyof IAuthPropertiesData)[] = ['basic_pwd'];

export const initialAuthProps = {
  basic_pwd: '',
  basic_user: '',
  jwt_bearerToken: '',
};

export const getAuthKey = (props: IAuthPropertiesData, key: keyof IAuthPropertiesData) =>
  props[key] ?? sessionStorage.getItem(`serilog_ui_${key}`) ?? '';

export const getAuthorizationHeader = (
  authType: string = '',
  props: IAuthPropertiesData,
) => {
  const authTypeToEnum = AuthType[authType];

  switch (authTypeToEnum) {
    case AuthType.Jwt:
      return props?.jwt_bearerToken ? `Bearer ${props.jwt_bearerToken}` : '';
    case AuthType.Basic:
      return 'TODO';
    default:
      return '';
  }
};

const setAuthDataValue = (
  props: IAuthPropertiesData,
  key: keyof IAuthPropertiesData,
  value: string,
) => {
  props[key] = value;

  if (!notSavedKeys.includes(key)) {
    sessionStorage.setItem(`serilog_ui_${key}`, value);
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
    // TODO: Decode jwt token in method
    // and send component notification, if it's not valid anymore
    const decoded = decodeJwt(bearerToken);
    console.log(decoded);
    console.log(decoded.iat);
    return { success: true, error: '' };
  } catch {
    return { success: false, error: 'Token invalid.' };
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
