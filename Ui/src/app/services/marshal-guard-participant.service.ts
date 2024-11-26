import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {
  CreateMarshalGuardParticipantModel, MarshalGuardParticipantModel,
} from "../models/marshalGuardParticipant.model";
import {Observable} from "rxjs";


@Injectable({
  providedIn: 'root'
})
export class MarshalGuardParticipantService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'MarshalGuardParticipants/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  insertMarshalGuardOParticipants(createMarshalGuardParticipants: CreateMarshalGuardParticipantModel[]) : Observable<void> {
    return this._httpClient.post<void>(this._serviceUrl, createMarshalGuardParticipants);
  }

  getPlayerMarshalGuardParticipants(playerId: string, last: number): Observable<MarshalGuardParticipantModel[]> {
    let params = new HttpParams();
    params = params.append('last', last);
    return this._httpClient.get<MarshalGuardParticipantModel[]>(this._serviceUrl + 'Player/' + playerId, {params: params});
  }

  updateMarshalGuardParticipant(marshalGuardParticipantId: string, marshalGuardParticipantModel: MarshalGuardParticipantModel) : Observable<MarshalGuardParticipantModel> {
    return this._httpClient.put<MarshalGuardParticipantModel>(this._serviceUrl + marshalGuardParticipantId, marshalGuardParticipantModel);
  }

}
