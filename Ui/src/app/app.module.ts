import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PlayerComponent } from './pages/player/player.component';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {NgbModule, NgbRatingModule} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NgxPaginationModule} from "ngx-pagination";
import { PlayerEditModalComponent } from './modals/player-edit-modal/player-edit-modal.component';
import { DesertStormComponent } from './pages/desert-storm/desert-storm.component';
import { MarshalGuardComponent } from './pages/marshal-guard/marshal-guard.component';
import { MarshalGuardModalComponent } from './modals/marshal-guard-modal/marshal-guard-modal.component';
import { PlayerInformationComponent } from './pages/player-information/player-information.component';
import { NavigationComponent } from './navigation/navigation.component';
import { VsDuelComponent } from './pages/vs-duel/vs-duel.component';
import {NgxSpinnerModule} from "ngx-spinner";
import {spinnerInterceptor} from "./interceptors/spinner.interceptor";
import {ToastrModule} from "ngx-toastr";
import { AllianceComponent } from './pages/alliance/alliance.component';
import { LoginComponent } from './Authentication/login/login.component';
import {JwtModule} from "@auth0/angular-jwt";
import { SignUpComponent } from './Authentication/sign-up/sign-up.component';
import {jwtInterceptor} from "./interceptors/jwt.interceptor";
import { PlayerNoteModalComponent } from './modals/player-note-modal/player-note-modal.component';
import { PlayerAdmonitionModalComponent } from './modals/player-admonition-modal/player-admonition-modal.component';
import { PlayerInfoMarshalGuardComponent } from './pages/player-information/player-info-marshal-guard/player-info-marshal-guard.component';
import { PlayerInfoVsDuelComponent } from './pages/player-information/player-info-vs-duel/player-info-vs-duel.component';
import { WeekPipe } from './helpers/week.pipe';
import { PlayerInfoDesertStormComponent } from './pages/player-information/player-info-desert-storm/player-info-desert-storm.component';
import { PlayerInfoCustomEventComponent } from './pages/player-information/player-info-custom-event/player-info-custom-event.component';
import { VsDuelCreateModalComponent } from './modals/vs-duel-create-modal/vs-duel-create-modal.component';
import { VsDuelDetailComponent } from './pages/vs-duel/vs-duel-detail/vs-duel-detail.component';
import { VsDuelEditComponent } from './pages/vs-duel/vs-duel-edit/vs-duel-edit.component';
import { MarshalGuardDetailComponent } from './pages/marshal-guard/marshal-guard-detail/marshal-guard-detail.component';
import { EmailConfirmationComponent } from './Authentication/email-confirmation/email-confirmation.component';
import { InviteUserModalComponent } from './modals/invite-user-modal/invite-user-modal.component';
import { RegisterComponent } from './Authentication/register/register.component';
import { UserEditModalComponent } from './modals/user-edit-modal/user-edit-modal.component';
import { AccountComponent } from './pages/account/account.component';
import { ChangePasswordComponent } from './pages/change-password/change-password.component';
import { DesertStormDetailComponent } from './pages/desert-storm/desert-storm-detail/desert-storm-detail.component';
import { DesertStormParticipantsModalComponent } from './modals/desert-storm-participants-modal/desert-storm-participants-modal.component';
import { ForgotPasswordComponent } from './Authentication/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './Authentication/reset-password/reset-password.component';
import { CustomEventComponent } from './pages/custom-event/custom-event.component';
import { UnderDevelopmentComponent } from './helpers/under-development/under-development.component';
import { ZombieSiegeComponent } from './pages/zombie-siege/zombie-siege.component';
import { ZombieSiegeParticipantsModalComponent } from './modals/zombie-siege-participants-modal/zombie-siege-participants-modal.component';
import { ZombieSiegeDetailComponent } from './pages/zombie-siege/zombie-siege-detail/zombie-siege-detail.component';
import { CustomEventParticipantsModelComponent } from './modals/custom-event-participants-model/custom-event-participants-model.component';
import { CustomEventDetailComponent } from './pages/custom-event/custom-event-detail/custom-event-detail.component';
import { DismissPlayerComponent } from './pages/dismiss-player/dismiss-player.component';
import { PlayerDismissInformationModalComponent } from './modals/player-dismiss-information-modal/player-dismiss-information-modal.component';
import { PlayerExcelImportModalComponent } from './modals/player-excel-import-modal/player-excel-import-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    PlayerComponent,
    PlayerEditModalComponent,
    DesertStormComponent,
    MarshalGuardComponent,
    MarshalGuardModalComponent,
    PlayerInformationComponent,
    NavigationComponent,
    VsDuelComponent,
    AllianceComponent,
    LoginComponent,
    SignUpComponent,
    PlayerNoteModalComponent,
    PlayerAdmonitionModalComponent,
    PlayerInfoMarshalGuardComponent,
    PlayerInfoVsDuelComponent,
    WeekPipe,
    PlayerInfoDesertStormComponent,
    PlayerInfoCustomEventComponent,
    VsDuelCreateModalComponent,
    VsDuelDetailComponent,
    VsDuelEditComponent,
    MarshalGuardDetailComponent,
    EmailConfirmationComponent,
    InviteUserModalComponent,
    RegisterComponent,
    UserEditModalComponent,
    AccountComponent,
    ChangePasswordComponent,
    DesertStormDetailComponent,
    DesertStormParticipantsModalComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    CustomEventComponent,
    UnderDevelopmentComponent,
    ZombieSiegeComponent,
    ZombieSiegeParticipantsModalComponent,
    ZombieSiegeDetailComponent,
    CustomEventParticipantsModelComponent,
    CustomEventDetailComponent,
    DismissPlayerComponent,
    PlayerDismissInformationModalComponent,
    PlayerExcelImportModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NgbModule,
    FormsModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgbRatingModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem(''),
      }
    })
  ],
  providers: [
    provideHttpClient(withInterceptors([spinnerInterceptor, jwtInterceptor]))
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
