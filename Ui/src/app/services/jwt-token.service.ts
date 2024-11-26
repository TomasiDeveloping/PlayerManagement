import {inject, Injectable} from '@angular/core';
import {JwtHelperService} from "@auth0/angular-jwt";
import {DecodedTokenModel} from "../models/decodedToken.model";

@Injectable({
  providedIn: 'root'
})
export class JwtTokenService {

  private readonly _tokenHelper: JwtHelperService = inject(JwtHelperService);
  private localStorageKey = 'LastWarPlayerManagementToken';

  public setToken(token: string): void {
    localStorage.setItem(this.localStorageKey, token);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.localStorageKey);
  }

  public removeToken(): void {
    localStorage.removeItem(this.localStorageKey);
  }

  public getAllianceId(): string | null {
    const token = localStorage.getItem(this.localStorageKey);
    if (!token) {
      return null;
    }
    return this._tokenHelper.decodeToken<DecodedTokenModel>(token)!.allianceId;
  }

  public getUserId(): string | null {
    const token = localStorage.getItem(this.localStorageKey);
    if (!token) {
      return null;
    }
    return this._tokenHelper.decodeToken<DecodedTokenModel>(token)!.userId;
  }

  public getRole(): string | null {
    const token = localStorage.getItem(this.localStorageKey);
    if (!token) {
      return null;
    }
    const decodedToken = this._tokenHelper.decodeToken<DecodedTokenModel>(token)!.userId;
    // @ts-ignore
    return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  }

  public getDecodedToken(): DecodedTokenModel | null {
    const token = localStorage.getItem(this.localStorageKey);
    if (!token) {
      return null;
    }
    return this._tokenHelper.decodeToken<DecodedTokenModel>(token);
  }
}
