import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {
  LeaderboardParticipationEventModel,
  LeaderboardPointAndParticipationEventModel,
  LeaderboardPointEventModel
} from "../models/customEventLeaderboard.model";

@Injectable({
  providedIn: 'root'
})
export class CustomEventLeaderboardService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'customEventLeaderboards/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getPointEvent(customEventCategoryId: string): Observable<LeaderboardPointEventModel[]> {
    return this._httpClient.get<LeaderboardPointEventModel[]>(this._serviceUrl + 'point/' + customEventCategoryId);
  }

  getParticipationEvent(customEventCategoryId: string): Observable<LeaderboardParticipationEventModel[]> {
    return this._httpClient.get<LeaderboardParticipationEventModel[]>(this._serviceUrl + 'participation/' + customEventCategoryId);
  }

  getPointAndParticipationEvent(customEventCategoryId: string): Observable<LeaderboardPointAndParticipationEventModel[]> {
    return this._httpClient.get<LeaderboardPointAndParticipationEventModel[]>(this._serviceUrl + 'point-and-participation/' + customEventCategoryId);

  }
}
