import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {VsDuelDetailModel, VsDuelModel} from "../models/vsDuel.model";

@Injectable({
  providedIn: 'root'
})
export class VsDuelService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'VsDuels/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getVsDuel(vsDuelId: string): Observable<VsDuelModel> {
    return this._httpClient.get<VsDuelModel>(this._serviceUrl + vsDuelId);
  }

  public getAllianceVsDuels(allianceId: string, take: number): Observable<VsDuelModel[]> {
    let params = new HttpParams();
    params = params.append('take', take);
    return this._httpClient.get<VsDuelModel[]>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
  }

  public getVsDuelDetail(vsDuelId: string): Observable<VsDuelDetailModel> {
    return this._httpClient.get<VsDuelDetailModel>(this._serviceUrl + 'GetDetailVsDuel/' + vsDuelId);
  }

  public createVsDuel(vsDuel: VsDuelModel): Observable<VsDuelModel> {
    return this._httpClient.post<VsDuelModel>(this._serviceUrl, vsDuel);
  }

  public updateVsDuel(vsDuelId: string, vsDuel: VsDuelModel): Observable<VsDuelModel> {
    return this._httpClient.put<VsDuelModel>(this._serviceUrl + vsDuelId, vsDuel);
  }

  public deleteVsDuel(vsDuelId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + vsDuelId);
  }
}
