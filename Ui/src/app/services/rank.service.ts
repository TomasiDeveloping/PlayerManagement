import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {RankModel} from "../models/rank.model";

@Injectable({
  providedIn: 'root'
})
export class RankService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Ranks/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getRanks(): Observable<RankModel[]> {
    return this._httpClient.get<RankModel[]>(this._serviceUrl);
  }
}
