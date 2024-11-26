import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {VsDuelParticipantModel} from "../models/vsDuelParticipant.model";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class VsDuelParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'VsDuelParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  updateVsDuelParticipant(vsDuelParticipantId: string, vsDuelParticipant: VsDuelParticipantModel): Observable<VsDuelParticipantModel> {
    return this._httpClient.put<VsDuelParticipantModel>(this._serviceUrl + vsDuelParticipantId, vsDuelParticipant);
  }

}
