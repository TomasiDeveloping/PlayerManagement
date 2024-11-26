import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {CreateDesertStormModel, DesertStormDetailModel, DesertStormModel} from "../models/desertStorm.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class DesertStormService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'DesertStorms/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  createDesertStorm(createModel: CreateDesertStormModel): Observable<DesertStormModel> {
    return this._httpClient.post<DesertStormModel>(this._serviceUrl, createModel);
  }

  getAllianceDesertStorms(allianceId: string, take: number): Observable<DesertStormModel[]> {
    let params = new HttpParams();
    params = params.append('take', take);
    return this._httpClient.get<DesertStormModel[]>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
  }

  getDesertStormDetail(desertStormId: string): Observable<DesertStormDetailModel> {
    return this._httpClient.get<DesertStormDetailModel>(this._serviceUrl + 'GetDesertStormDetail/' + desertStormId);
  }

  updateDesertStorm(desertStormId: string, desertStorm: DesertStormModel): Observable<DesertStormModel> {
    return this._httpClient.put<DesertStormModel>(this._serviceUrl + desertStormId, desertStorm);
  }

  deleteDesertStorm(desertStormId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + desertStormId);
  }
}
