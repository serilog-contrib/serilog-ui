import {
  IAuthPropertiesData,
  IAuthPropertiesStorageKeys,
  clearAuth,
  defaultAuthProps,
  getAuthKey,
  getAuthorizationHeader,
  saveAuthKey,
} from 'app/util/auth';
import { AuthType } from 'types/types';
import { describe, expect, it } from 'vitest';

describe('util: auth', () => {
  it('clearAuth: clears all items from session storage', () => {
    Object.values(IAuthPropertiesStorageKeys).forEach((val) => {
      sessionStorage.setItem(val, 'value');
    });

    const sut = clearAuth();

    expect(sut).toStrictEqual(defaultAuthProps);
    Object.values(IAuthPropertiesStorageKeys).forEach((val) => {
      expect(sessionStorage.getItem(val)).toBeNull();
    });
  });

  describe('getAuthKey', () => {
    it('returns value from props', () => {
      const sut = getAuthKey({ basic_pwd: 'value' }, 'basic_pwd');

      expect(sut).toBe('value');
    });

    it('returns value from session storage, if not found in props', () => {
      sessionStorage.setItem(IAuthPropertiesStorageKeys.jwt_bearerToken, 'token');

      const sut = getAuthKey({}, 'jwt_bearerToken');

      expect(sut).toBe('token');
    });

    it('returns empty as fallback', () => {
      const sut = getAuthKey({}, 'basic_pwd');

      expect(sut).toBe('');
    });
  });

  describe('getAuthorizationHeader', () => {
    const sample = {
      basic_pwd: 'pwd',
      basic_user: 'user',
      jwt_bearerToken: 'jwt',
    };

    it('returns jwt token header', () => {
      const sut = getAuthorizationHeader(sample, AuthType.Jwt);

      expect(sut).toBe('Bearer jwt');
    });

    it('returns basic header', () => {
      const sut = getAuthorizationHeader(sample, AuthType.Basic);

      expect(sut).toBe('Basic dXNlcjpwd2Q=');
    });

    it('returns empty if required properties for authorization type are not defined', () => {
      const suts = [
        getAuthorizationHeader({ basic_pwd: 'pwd' }, AuthType.Basic),
        getAuthorizationHeader({ basic_user: 'pwd' }, AuthType.Basic),
        getAuthorizationHeader({}, AuthType.Jwt),
      ];

      suts.forEach((i) => expect(i).toBe(''));
    });

    it('returns empty as fallback', () => {
      const sut = getAuthorizationHeader(sample, 'my-auth');

      expect(sut).toBe('');
    });
  });

  describe('saveAuthKey', () => {
    it('sets value on object and in session storage', () => {
      const props: IAuthPropertiesData = {};
      const result = saveAuthKey(props, 'basic_user', 'value');

      expect(props.basic_user).toBe('value');
      expect(
        sessionStorage.getItem(IAuthPropertiesStorageKeys.basic_user),
      ).not.toBeNull();
      expect(result.error).toBeFalsy();
    });

    it.each(['basic_pwd'])(
      'sets value on object but not in session storage for key %s',
      (property) => {
        const props: IAuthPropertiesData = {};
        const result = saveAuthKey(
          props,
          property as unknown as keyof IAuthPropertiesData,
          'value',
        );

        expect(sessionStorage.getItem(IAuthPropertiesStorageKeys[property])).toBeNull();
        expect(result.error).toBeFalsy();
      },
    );

    it.each(['jwt_bearerToken'])('validates property: %s', (property) => {
      const props: IAuthPropertiesData = {};
      const result = saveAuthKey(
        props,
        property as unknown as keyof IAuthPropertiesData,
        'value',
      );

      expect(result.error).toBeTruthy();
    });
  });
});
