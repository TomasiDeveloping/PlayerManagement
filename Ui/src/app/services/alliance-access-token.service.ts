import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AllianceAccessTokenModel} from "../models/allianceAccessToken.model";

@Injectable({
  providedIn: 'root'
})

export class AllianceAccessTokenService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'AllianceAccessToken/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  create(allianceId: string, expiresAt: string | null): Observable<AllianceAccessTokenModel> {
    const request = {allianceId: allianceId, expiresAt: expiresAt};
    return this._httpClient.post<AllianceAccessTokenModel>(this._serviceUrl + 'generate/', request);
  }

  getTokens(allianceId: string): Observable<AllianceAccessTokenModel[]> {
    return this._httpClient.get<AllianceAccessTokenModel[]>(this._serviceUrl + 'alliance/' + allianceId);
  }

  validate(allianceId: string): Observable<AllianceAccessTokenModel> {
    return this._httpClient.get<AllianceAccessTokenModel>(this._serviceUrl + 'validate/' + allianceId);
  }

  revokeToken(id: string): Observable<AllianceAccessTokenModel> {
    return this._httpClient.post<AllianceAccessTokenModel>(this._serviceUrl + 'revoke/' + id, {});
  }
}
