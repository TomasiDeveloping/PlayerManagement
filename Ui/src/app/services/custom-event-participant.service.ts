import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {CustomEventParticipantModel} from "../models/customEventParticipant.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CustomEventParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'customEventParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  insertCustomEventParticipants(customEventParticipants: CustomEventParticipantModel[]): Observable<void> {
    return this._httpClient.post<void>(this._serviceUrl, customEventParticipants);
  }

  updateCustomEventParticipant(customEventParticipantId: string, customEventParticipantModel: CustomEventParticipantModel): Observable<CustomEventParticipantModel> {
    return this._httpClient.put<CustomEventParticipantModel>(this._serviceUrl + customEventParticipantId, customEventParticipantModel);
  }

}
