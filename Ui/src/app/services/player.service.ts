import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreatePlayerModel, PlayerModel, UpdatePlayerModel} from "../models/player.model";

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Players/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getPlayer(playerId: string): Observable<PlayerModel> {
    return this._httpClient.get<PlayerModel>(this._serviceUrl + playerId);
  }

  public getAlliancePlayer(allianceId: string): Observable<PlayerModel[]> {
    return this._httpClient.get<PlayerModel[]>(this._serviceUrl + 'Alliance/' + allianceId);
  }

  public updatePlayer(playerId: string, player: UpdatePlayerModel): Observable<PlayerModel> {
    return this._httpClient.put<PlayerModel>(this._serviceUrl + playerId, player);
  }

  public insertPlayer(player: CreatePlayerModel): Observable<PlayerModel> {
    return this._httpClient.post<PlayerModel>(this._serviceUrl, player);
  }

  public deletePlayer(playerId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + playerId);
  }
}
