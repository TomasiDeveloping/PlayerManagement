import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateZombieSiegeModel, ZombieSiegeDetailModel, ZombieSiegeModel} from "../models/zombieSiege.model";
import {PagedResponseModel} from "../models/pagedResponse.model";

@Injectable({
  providedIn: 'root'
})
export class ZombieSiegeService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'ZombieSieges/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getZombieSiegeDetail(zombieSiegeId: string): Observable<ZombieSiegeDetailModel> {
    return this._httpClient.get<ZombieSiegeDetailModel>(this._serviceUrl + 'GetZombieSiegeDetail/' + zombieSiegeId);
  }

  getAllianceZombieSieges(allianceId: string, pageNumber: number, pageSize: number): Observable<PagedResponseModel<ZombieSiegeModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    return this._httpClient.get<PagedResponseModel<ZombieSiegeModel>>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
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
