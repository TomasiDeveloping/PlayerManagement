import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateSquadModel, SquadModel, UpdateSquadModel} from "../models/squad.model";

@Injectable({
  providedIn: 'root'
})
export class SquadService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Squads/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getPlayerSquads(playerId: string): Observable<SquadModel[]> {
    return this._httpClient.get<SquadModel[]>(this._serviceUrl + 'player/' + playerId)
  }

  createSquad(createSquad: CreateSquadModel): Observable<SquadModel> {
    return this._httpClient.post<SquadModel>(this._serviceUrl, createSquad);
  }

  updateSquad(squadId: string, updateSquad: UpdateSquadModel): Observable<SquadModel> {
    return this._httpClient.put<SquadModel>(this._serviceUrl + squadId, updateSquad);
  }

  deleteSquad(squadId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + squadId);
  }
}
