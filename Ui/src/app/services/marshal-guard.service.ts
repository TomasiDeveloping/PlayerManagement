import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {
  CreateMarshalGuardModel,
  MarshalGuardDetailModel,
  MarshalGuardModel,
  UpdateMarshalGuardModel
} from "../models/marshalGuard.model";
import {Observable} from "rxjs";
import {PagedResponseModel} from "../models/pagedResponse.model";

@Injectable({
  providedIn: 'root'
})
export class MarshalGuardService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'MarshalGuards/';
  private readonly _httpClient: HttpClient = inject(HttpClient);


  public insertMarshalGuards(createMarshalGuards: CreateMarshalGuardModel): Observable<MarshalGuardModel> {
    return this._httpClient.post<MarshalGuardModel>(this._serviceUrl, createMarshalGuards);
  }

  public getMarshalGuardDetail(marshalGuardId: string): Observable<MarshalGuardDetailModel> {
    return this._httpClient.get<MarshalGuardDetailModel>(this._serviceUrl + 'GetMarshalGuardDetail/' + marshalGuardId);
  }

  public getAllianceMarshalGuards(allianceId: string, pageNumber: number, pageSize: number): Observable<PagedResponseModel<MarshalGuardModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    return this._httpClient.get<PagedResponseModel<MarshalGuardModel>>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
  }

  public updateMarshalGuard(marshalGuardId: string, marshalGuard: UpdateMarshalGuardModel): Observable<MarshalGuardModel> {
    return this._httpClient.put<MarshalGuardModel>(this._serviceUrl + marshalGuardId, marshalGuard);
  }

  public deleteMarshalGuard(marshalGuardId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + marshalGuardId);
  }

}
