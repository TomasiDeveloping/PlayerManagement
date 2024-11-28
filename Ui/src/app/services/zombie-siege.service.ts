import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateZombieSiegeModel, ZombieSiegeDetailModel, ZombieSiegeModel} from "../models/zombieSiege.model";

@Injectable({
  providedIn: 'root'
})
export class ZombieSiegeService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'ZombieSieges/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getZombieSiegeDetail(zombieSiegeId: string): Observable<ZombieSiegeDetailModel> {
    return this._httpClient.get<ZombieSiegeDetailModel>(this._serviceUrl + 'GetZombieSiegeDetail/' + zombieSiegeId);
  }

  getAllianceZombieSieges(allianceId: string, take: number): Observable<ZombieSiegeModel[]> {
    let params = new HttpParams();
    params = params.append('take', take);
    return this._httpClient.get<ZombieSiegeModel[]>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
  }

  createZombieSiege(createZombieSiege: CreateZombieSiegeModel): Observable<ZombieSiegeModel> {
    return this._httpClient.post<ZombieSiegeModel>(this._serviceUrl, createZombieSiege);
  }

  updateZombieSiege(zombieSiegeId: string, zombieSiege: ZombieSiegeModel): Observable<ZombieSiegeModel> {
    return this._httpClient.put<ZombieSiegeModel>(this._serviceUrl + zombieSiegeId, zombieSiege);
  }

  deleteZombieSiege(zombieSiegeId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + zombieSiegeId);
  }
}
