import { immerable } from 'immer';
import { AuthType } from '../../types/types';
import { decodeJwt } from 'jose';

export class AuthProperties {
  public [immerable] = true;

  authType?: AuthType;
  routePrefix?: string;
  private _bearerToken: string | null;

  public get bearerToken() {
    return this._bearerToken ?? sessionStorage.getItem('serilogui_token') ?? '';
  }

  public set bearerToken(bearerToken: string) {
    this._bearerToken = bearerToken;

    sessionStorage.setItem('serilogui_token', bearerToken);
  }

  constructor() {
    /** TODO: remove development helper line */
    sessionStorage.clear();
    /******/

    let auth: string | undefined;
    ({ authType: auth, routePrefix: this.routePrefix } = window.config);

    this.authType = AuthType[auth ?? ''];
  }

  /**
   * Validate a bearer token.
   */
  validateToken(bearerToken: string) {
    try {
      // TODO: Decode jwt token in method
      // and send component notification, if it's not valid anymore
      const decoded = decodeJwt(bearerToken);
      console.log(decoded);
      console.log(decoded.iat);
    } catch {}
  }
}
