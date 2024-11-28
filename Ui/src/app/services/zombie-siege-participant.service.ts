import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateZombieSiegeParticipantModel, ZombieSiegeParticipantModel} from "../models/zombieSiegeParticipant.model";

@Injectable({
  providedIn: 'root'
})
export class ZombieSiegeParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'ZombieSiegeParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getPlayerZombieSiegeParticipants(playerId: string, last: number): Observable<ZombieSiegeParticipantModel[]> {
    let params = new HttpParams();
    params = params.append('last', last);
    return this._httpClient.get<ZombieSiegeParticipantModel[]>(this._serviceUrl + 'Player/' + playerId, {params: params});
  }

  insertZombieSiegeParticipants(zombieSiegeParticipants: CreateZombieSiegeParticipantModel[]): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl, zombieSiegeParticipants)
  }

  updateZombieSiegeParticipant(zombieSiegeParticipantId: string, zombieSiegeParticipant: ZombieSiegeParticipantModel): Observable<ZombieSiegeParticipantModel> {
    return this._httpClient.put<ZombieSiegeParticipantModel>(this._serviceUrl + zombieSiegeParticipantId, zombieSiegeParticipant);
  }
}
