import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PlayerCombatModel} from "../models/playerCombat.model";
import {PlayerCombatRecordModel} from "../models/playerCombatRecord.model";
import {PlayerCombatRecordOverviewModel} from "../models/playerCombatRecordOverview.model";

@Injectable({
  providedIn: 'root'
})

export class PlayerCombatService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'PlayerCombatRecords/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getPlayers(allianceId: string): Observable<PlayerCombatModel> {
    return this._httpClient.get<PlayerCombatModel>(this._serviceUrl + allianceId);
  }

  getPlayerRecords(playerId: string, limit?: number): Observable<PlayerCombatRecordModel[]> {
    let params = new HttpParams();

    if (limit != undefined) {
      params = params.append('limit', limit.toString());
    }
    return this._httpClient.get<PlayerCombatRecordModel[]>(this._serviceUrl + 'player/' + playerId,{params: params});
  }

  addRecord(record: PlayerCombatRecordModel): Observable<PlayerCombatRecordModel> {
    return this._httpClient.post<PlayerCombatRecordModel>(this._serviceUrl, record)
  }

  updateRecord(id: string, record: PlayerCombatRecordModel): Observable<PlayerCombatRecordModel> {
    return this._httpClient.put<PlayerCombatRecordModel>(this._serviceUrl + id, record)
  }

  deleteRecord(id: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + id)
  }

  getAllianzOverview(allianceId: string): Observable<PlayerCombatRecordOverviewModel[]> {
    return this._httpClient.get<PlayerCombatRecordOverviewModel[]>(this._serviceUrl + 'alliance-combat/' + allianceId);
  }

  exportAllianceData(allianceId: string, scope: 'latest' | 'all', format: 'excel' | 'csv'): Observable<Blob> {
    const params = new HttpParams()
      .set('scope', scope)
      .set('format', format);

    return this._httpClient.get(this._serviceUrl + 'export/' + allianceId + '/', {params: params, responseType: 'blob'});
  }
}
