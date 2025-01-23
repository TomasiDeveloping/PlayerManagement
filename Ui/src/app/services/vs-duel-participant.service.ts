import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {VsDuelParticipantDetailModel, VsDuelParticipantModel} from "../models/vsDuelParticipant.model";
import {Observable} from "rxjs";
import {MarshalGuardParticipantModel} from "../models/marshalGuardParticipant.model";

@Injectable({
  providedIn: 'root'
})
export class VsDuelParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'VsDuelParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);


  getVsDuelParticipantsDetail(playerId: string, last: number): Observable<VsDuelParticipantDetailModel[]> {
    let params = new HttpParams();
    params = params.append('last', last);
    return this._httpClient.get<VsDuelParticipantDetailModel[]>(this._serviceUrl + 'Player/' + playerId, {params: params});
  }

  updateVsDuelParticipant(vsDuelParticipantId: string, vsDuelParticipant: VsDuelParticipantModel): Observable<VsDuelParticipantModel> {
    return this._httpClient.put<VsDuelParticipantModel>(this._serviceUrl + vsDuelParticipantId, vsDuelParticipant);
  }

}
