import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {VsDuelLeagueModel} from "../models/vsDuelLeague.model";

@Injectable({
  providedIn: 'root'
})
export class VsDuelLeagueService {

  private readonly _serviceUrl: string = environment.apiBaseUrl + 'VsDuelLeagues/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getVsDuelLeagues(): Observable<VsDuelLeagueModel[]> {
    return this._httpClient.get<VsDuelLeagueModel[]>(this._serviceUrl);
  }
}
