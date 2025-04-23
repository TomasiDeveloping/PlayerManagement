import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  CreateCustomEventCategoryModel,
  CustomEventCategoryModel,
  UpdateCustomEventCategoryModel
} from "../models/customEventCategory.model";

@Injectable({
  providedIn: 'root'
})
export class CustomEventCategoryService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'customEventCategories/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAllianceCustomEventCategories(allianceId: string): Observable<CustomEventCategoryModel[]> {
    return this._httpClient.get<CustomEventCategoryModel[]>(this._serviceUrl + 'Alliance/' + allianceId);
  }

  getCustomEventCategory(customEventCategoryId: string): Observable<CustomEventCategoryModel> {
    return this._httpClient.get<CustomEventCategoryModel>(this._serviceUrl  + customEventCategoryId);
  }

  createCustomEventCategory(customEventCategory: CreateCustomEventCategoryModel): Observable<CustomEventCategoryModel> {
    return this._httpClient.post<CustomEventCategoryModel>(this._serviceUrl, customEventCategory);
  }

  updateCustomEvent(customEventCategoryId: string, customEvent: UpdateCustomEventCategoryModel): Observable<CustomEventCategoryModel> {
    return this._httpClient.put<CustomEventCategoryModel>(this._serviceUrl + customEventCategoryId, customEvent);
  }

  deleteCustomEventCategory(customEventCategoryId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + customEventCategoryId );
  }
}
