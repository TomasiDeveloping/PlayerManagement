import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PlayerInformationComponent} from "./pages/player-information/player-information.component";
import {PlayerComponent} from "./pages/player/player.component";
import {DesertStormComponent} from "./pages/desert-storm/desert-storm.component";
import {MarshalGuardComponent} from "./pages/marshal-guard/marshal-guard.component";
import {VsDuelComponent} from "./pages/vs-duel/vs-duel.component";
import {AllianceComponent} from "./pages/alliance/alliance.component";
import {LoginComponent} from "./Authentication/login/login.component";
import {authGuard} from "./guards/auth.guard";
import {SignUpComponent} from "./Authentication/sign-up/sign-up.component";
import {VsDuelDetailComponent} from "./pages/vs-duel/vs-duel-detail/vs-duel-detail.component";
import {VsDuelEditComponent} from "./pages/vs-duel/vs-duel-edit/vs-duel-edit.component";
import {MarshalGuardDetailComponent} from "./pages/marshal-guard/marshal-guard-detail/marshal-guard-detail.component";
import {EmailConfirmationComponent} from "./Authentication/email-confirmation/email-confirmation.component";
import {RegisterComponent} from "./Authentication/register/register.component";
import {AccountComponent} from "./pages/account/account.component";
import {ChangePasswordComponent} from "./pages/change-password/change-password.component";
import {DesertStormDetailComponent} from "./pages/desert-storm/desert-storm-detail/desert-storm-detail.component";
import {ResetPasswordComponent} from "./Authentication/reset-password/reset-password.component";
import {CustomEventComponent} from "./pages/custom-event/custom-event.component";
import {ZombieSiegeComponent} from "./pages/zombie-siege/zombie-siege.component";
import {ZombieSiegeDetailComponent} from "./pages/zombie-siege/zombie-siege-detail/zombie-siege-detail.component";
import {CustomEventDetailComponent} from "./pages/custom-event/custom-event-detail/custom-event-detail.component";
import {DismissPlayerComponent} from "./pages/dismiss-player/dismiss-player.component";
import {MvpComponent} from "./pages/mvp/mvp.component";
import {FeedbackComponent} from "./pages/feedback/feedback.component";

const routes: Routes = [
  {path: 'players', component: PlayerComponent, canActivate: [authGuard]},
  {path: 'dismiss-players', component: DismissPlayerComponent, canActivate: [authGuard]},
  {path: 'player-information/:id', component: PlayerInformationComponent, canActivate: [authGuard]},
  {path: 'marshal-guard', component: MarshalGuardComponent, canActivate: [authGuard]},
  {path: 'mvp', component: MvpComponent, canActivate: [authGuard]},
  {path: 'marshal-guard-detail/:id', component: MarshalGuardDetailComponent, canActivate: [authGuard]},
  {path: 'vs-duel', component: VsDuelComponent, canActivate: [authGuard]},
  {path: 'vs-duel-detail/:id', component: VsDuelDetailComponent, canActivate: [authGuard]},
  {path: 'vs-duel-edit/:id', component: VsDuelEditComponent, canActivate: [authGuard]},
  {path: 'desert-storm', component: DesertStormComponent, canActivate: [authGuard]},
  {path: 'desert-storm-detail/:id', component: DesertStormDetailComponent, canActivate: [authGuard]},
  { path: 'alliance', component: AllianceComponent, canActivate: [authGuard]},
  {path: 'account', component: AccountComponent, canActivate: [authGuard]},
  {path: 'change-password', component: ChangePasswordComponent, canActivate: [authGuard]},
  {path: 'feedback', component: FeedbackComponent, canActivate: [authGuard]},
  {path: 'custom-event', component: CustomEventComponent, canActivate: [authGuard]},
  {path: 'custom-event-detail/:id', component: CustomEventDetailComponent, canActivate: [authGuard]},
  {path: 'zombie-siege', component: ZombieSiegeComponent, canActivate: [authGuard]},
  {path: 'zombie-siege-detail/:id', component: ZombieSiegeDetailComponent, canActivate: [authGuard]},
  {path: 'login', component: LoginComponent},
  {path: 'confirm-email', component: EmailConfirmationComponent},
  {path: 'sign-up', component: SignUpComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'reset-password', component: ResetPasswordComponent},
  {path: '', redirectTo: 'players', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
