import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateCustomEventModel, CustomEventDetailModel, CustomEventModel} from "../models/customEvent.model";

@Injectable({
  providedIn: 'root'
})
export class CustomEventService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'customEvents/';
  private readonly _httpClient: HttpClient = inject(HttpClient);


  getAllianceCustomEvents(allianceId: string, take: number): Observable<CustomEventModel[]> {
    let params = new HttpParams();
    params = params.append('take', take);
    return this._httpClient.get<CustomEventModel[]>(this._serviceUrl + 'Alliance/' + allianceId, {params: params});
  }

  getCustomEventDetail(customEventId: string): Observable<CustomEventDetailModel> {
    return this._httpClient.get<CustomEventDetailModel>(this._serviceUrl + 'GetCustomEventDetail/' + customEventId);
  }

  createCustomEvent(customEvent: CreateCustomEventModel): Observable<CustomEventModel> {
    return this._httpClient.post<CustomEventModel>(this._serviceUrl, customEvent);
  }

  updateCustomEvent(customEventId: string, customEvent: CustomEventModel): Observable<CustomEventModel> {
    return this._httpClient.put<CustomEventModel>(this._serviceUrl + customEventId, customEvent);
  }

  deleteCustomEvent(customEventId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + customEventId);
  }

}