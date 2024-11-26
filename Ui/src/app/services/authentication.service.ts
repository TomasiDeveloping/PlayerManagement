import {inject, Injectable} from '@angular/core';
import {JwtTokenService} from "./jwt-token.service";
import {LoggedInUser} from "../models/user.model";
import {BehaviorSubject, map, Observable} from "rxjs";
import {Router} from "@angular/router";
import {DecodedTokenModel} from "../models/decodedToken.model";
import Swal from "sweetalert2";
import {LoginRequestModel, LoginResponseModel} from "../models/login.model";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {SignUpRequestModel} from "../models/signUp.model";
import {ConfirmEmailRequestModel} from "../models/confirmEmailRequest.model";
import {EmailConfirmationRequestModel} from "../models/emailConfirmationRequest.model";
import {InviteUserModel} from "../models/inviteUser.model";
import {RegisterUserModel} from "../models/registerUser.model";
import {ForgotPasswordModel} from "../models/forgotPassword.model";
import {ResetPasswordModel} from "../models/resetPassword.model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Authentications/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  private readonly _tokenHelperService: JwtTokenService = inject(JwtTokenService);
  private readonly _router: Router = inject(Router);

  private userSubject$: BehaviorSubject<LoggedInUser | null> = new BehaviorSubject<LoggedInUser | null>(null);
  private tokenExpirationTimer: any;

  get user() {
    return this.userSubject$.value;
  }

  get authStateChange() : Observable<LoggedInUser | null> {
    return this.userSubject$.asObservable();
  }

  public autoLogin(): void {
    const decodedToke: DecodedTokenModel | null = this._tokenHelperService.getDecodedToken();

    if (decodedToke) {
      const expiryDate = new Date(decodedToke.exp * 1000);
      if (expiryDate <= new Date) {
        this.logout();
      } else {
        const loggedInUser: LoggedInUser = this.createLoggedInUser(decodedToke);
        this.clearTokenExpirationTimer();
        this.setTokenExpirationTimer(decodedToke.exp);
        this.userSubject$.next(loggedInUser);
      }
    }
  }

  public login(loginRequest: LoginRequestModel): Observable<LoginResponseModel> {
    return this._httpClient.post<LoginResponseModel>(this._serviceUrl + 'Login', loginRequest)
      .pipe(map((response) => {
        if (response.token) {
          this._tokenHelperService.setToken(response.token);
          const decodedToken: DecodedTokenModel | null = this._tokenHelperService.getDecodedToken();
          if (decodedToken) {
            const loggedInUser: LoggedInUser = this.createLoggedInUser(decodedToken);
            this.clearTokenExpirationTimer();
            this.setTokenExpirationTimer(decodedToken.exp);
            this.userSubject$.next(loggedInUser);
          }
        }
        return response;
      }))
  }

  public logout(): void {
    this._tokenHelperService.removeToken();
    this.userSubject$.next(null);
    this._router.navigate(['/login']).then();
  }

  public forgotPassword(forgotPasswordModel: ForgotPasswordModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ForgotPassword', forgotPasswordModel);
  }

  public resetPassword(resetPasswordModel: ResetPasswordModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ResetPassword', resetPasswordModel);
  }

  public signUp(signUpModel: SignUpRequestModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'SignUp', signUpModel);
  }

  public registerUser(registerUserModel: RegisterUserModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'RegisterUser', registerUserModel);
  }

  public confirmEmail(confirmEmailRequest: ConfirmEmailRequestModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ConfirmEmail', confirmEmailRequest)
  }

  public resendConfirmationEmail(emailConfirmation: EmailConfirmationRequestModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'ResendConfirmationEmail', emailConfirmation)
  }

  public inviteUser(inviteUserModel: InviteUserModel): Observable<boolean> {
    return this._httpClient.post<boolean>(this._serviceUrl + 'InviteUser', inviteUserModel);
  }

  private createLoggedInUser(decodedToke: DecodedTokenModel): LoggedInUser {
    return {
      id: decodedToke.userId,
      userName: decodedToke.playerName,
      email: decodedToke.email,
      allianceId: decodedToke.allianceId,
      allianceName: decodedToke.allianceName
    };
  }

  private clearTokenExpirationTimer(): void {
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
    this.tokenExpirationTimer = null;
  }

  private setTokenExpirationTimer(exp: number): void {
    const duration: number = new Date(exp * 1000).getTime() - new Date().getTime();
    this.tokenExpirationTimer = setTimeout(() => {
      Swal.fire('Session expired!', 'The session has expired and you will be logged out automatically', 'info').then(() => {
        this.logout();
      })
    }, duration);
  }
}
