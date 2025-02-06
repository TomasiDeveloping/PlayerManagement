import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  CreatePlayerModel,
  DismissPlayerInformationModel,
  PlayerModel, PlayerMvpModel,
  UpdatePlayerModel
} from "../models/player.model";
import {ExcelImportResponseModel} from "../models/excelImportResponse.model";
import {PagedResponseModel} from "../models/pagedResponse.model";

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

  public getDismissedPlayers(allianceId: string, pageNumber: number, pageSize: number): Observable<PagedResponseModel<PlayerModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    return this._httpClient.get<PagedResponseModel<PlayerModel>>(this._serviceUrl + 'Alliance/dismiss/' + allianceId, {params: params});
  }

  public getDismissPlayerInformation(playerId: string): Observable<DismissPlayerInformationModel> {
    return this._httpClient.get<DismissPlayerInformationModel>(this._serviceUrl + 'DismissInformation/' + playerId);
  }

  public getAllianceMvpPlayers(allianceId: string, playerType: string): Observable<PlayerMvpModel[]> {
    let params = new HttpParams();
    params = params.append("allianceId", allianceId);
    params = params.append("playerType", playerType);
    return this._httpClient.get<PlayerMvpModel[]>(this._serviceUrl + 'Mvp', {params: params});
  }

  public updatePlayer(playerId: string, player: UpdatePlayerModel): Observable<PlayerModel> {
    return this._httpClient.put<PlayerModel>(this._serviceUrl + playerId, player);
  }

  public dismissPlayer(playerId: string, reason: string): Observable<PlayerModel> {
    const dismissRequest = {
      id: playerId,
      dismissalReason: reason
    };
    return this._httpClient.put<PlayerModel>(this._serviceUrl + playerId + '/dismiss', dismissRequest);
  }

  public reactivePlayer(playerId: string): Observable<PlayerModel> {
    const reactiveRequest = {
      id: playerId,
    };
    return this._httpClient.put<PlayerModel>(this._serviceUrl + playerId + '/reactive', reactiveRequest);
  }

  public insertPlayer(player: CreatePlayerModel): Observable<PlayerModel> {
    return this._httpClient.post<PlayerModel>(this._serviceUrl, player);
  }

  public uploadPlayerFromExcel(allianceId: string, excelFile: File): Observable<ExcelImportResponseModel> {
    const formData = new FormData();
    formData.append('excelFile', excelFile);
    formData.append('allianceId', allianceId);

    return this._httpClient.post<ExcelImportResponseModel>(this._serviceUrl + 'ExcelImport', formData);
  }

  public deletePlayer(playerId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + playerId);
  }
}
