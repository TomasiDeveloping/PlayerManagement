import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UpdateUserModel, UserModel} from "../models/user.model";
import {ChangePasswordModel} from "../models/changePassword.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Users/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  getUser(userId: string): Observable<UserModel> {
    return this._httpClient.get<UserModel>(this._serviceUrl + userId);
  }
  getAllianceUsers(allianceId: string): Observable<UserModel[]> {
    return this._httpClient.get<UserModel[]>(this._serviceUrl + 'Alliance/' + allianceId);
  }

  updateUser(userId: string, updateUser: UpdateUserModel): Observable<UserModel> {
    return this._httpClient.put<UserModel>(this._serviceUrl + userId, updateUser);
  }

  changeUserPassword(changeUserPasswordModel: ChangePasswordModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ChangeUserPassword', changeUserPasswordModel);
  }

  deleteUser(userId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + userId);
  }
}
