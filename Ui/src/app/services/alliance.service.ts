import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AllianceModel} from "../models/alliance.model";

@Injectable({
  providedIn: 'root'
})
export class AllianceService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Alliances/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getAlliance(allianceId: string): Observable<AllianceModel> {
    return this._httpClient.get<AllianceModel>(this._serviceUrl + allianceId);
  }

  updateAlliance(allianceId: string, alliance: AllianceModel): Observable<AllianceModel> {
    return this._httpClient.put<AllianceModel>(this._serviceUrl + allianceId, alliance);
  }
}
