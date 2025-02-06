import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ApiKeyModel, CreateApiKeyModel, UpdateApiKeyModel} from "../models/apiKey.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiKeyService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'ApiKeys/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAllianceApiKey(apiKeyId: string): Observable<ApiKeyModel> {
    return this._httpClient.get<ApiKeyModel>(this._serviceUrl + apiKeyId);
  }

  createApiKey(createApiKey: CreateApiKeyModel): Observable<ApiKeyModel> {
    return this._httpClient.post<ApiKeyModel>(this._serviceUrl, createApiKey);
  }

  updateApiKey(apiKeyId: string, updateApiKey: UpdateApiKeyModel): Observable<ApiKeyModel> {
    return this._httpClient.put<ApiKeyModel>(this._serviceUrl + apiKeyId, updateApiKey);
  }

  deleteApiKey(apiKeyId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + apiKeyId);
  }
}
