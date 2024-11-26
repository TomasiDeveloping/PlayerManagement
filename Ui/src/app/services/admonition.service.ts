import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AdmonitionModel} from "../models/admonition.model";

@Injectable({
  providedIn: 'root'
})
export class AdmonitionService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Admonitions/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAdmonition(admonitionId: string): Observable<AdmonitionModel> {
    return this._httpClient.get<AdmonitionModel>(this._serviceUrl + admonitionId);
  }

  getPlayerAdmonitions(playerId: string): Observable<AdmonitionModel[]> {
    return this._httpClient.get<AdmonitionModel[]>(this._serviceUrl + 'Player/' + playerId);
  }

  createAdmonition(admonition: AdmonitionModel): Observable<AdmonitionModel> {
    return this._httpClient.post<AdmonitionModel>(this._serviceUrl, admonition);
  }

  updateAdmonition(admonitionId: string, admonition: AdmonitionModel): Observable<AdmonitionModel> {
    return this._httpClient.put<AdmonitionModel>(this._serviceUrl + admonitionId, admonition);
  }

  deleteAdmonition(admonitionId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + admonitionId);
  }
}
