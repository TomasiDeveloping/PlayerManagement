import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateDesertStormParticipantModel, DesertStormParticipantModel} from "../models/desertStormParticipant.model";

@Injectable({
  providedIn: 'root'
})
export class DesertStormParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'DesertStormParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  insertDesertStormOParticipants(createDesertStormParticipants: CreateDesertStormParticipantModel[]): Observable<void> {
    return this._httpClient.post<void>(this._serviceUrl, createDesertStormParticipants);
  }

  getPlayerDesertStormParticipants(playerId: string, last: number): Observable<DesertStormParticipantModel[]> {
    let params = new HttpParams();
    params = params.append('last', last);
    return this._httpClient.get<DesertStormParticipantModel[]>(this._serviceUrl + 'Player/' + playerId, {params: params});
  }

  updateDesertStormParticipant(desertStormParticipantId: string, desertStormParticipantModel: DesertStormParticipantModel): Observable<DesertStormParticipantModel> {
    return this._httpClient.put<DesertStormParticipantModel>(this._serviceUrl + desertStormParticipantId, desertStormParticipantModel);
  }

}
