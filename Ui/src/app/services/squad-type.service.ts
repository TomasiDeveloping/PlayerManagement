import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {SquadTypeModel} from "../models/squadType.model";

@Injectable({
  providedIn: 'root'
})
export class SquadTypeService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'SquadTypes/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getSquadTypes(): Observable<SquadTypeModel[]> {
    return this._httpClient.get<SquadTypeModel[]>(this._serviceUrl);
  }
}
