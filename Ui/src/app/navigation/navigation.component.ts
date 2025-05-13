import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {AuthenticationService} from "../services/authentication.service";
import {LoggedInUser} from "../models/user.model";
import {Subscription} from "rxjs";
import {environment} from "../../environments/environment";

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements OnInit, OnDestroy {

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  private _authStateChange$: Subscription | undefined;

  isShown: boolean = false;
  version: string = environment.version;
  loggedInUser: LoggedInUser | null = null;

  ngOnInit() {
    this._authStateChange$ = this._authenticationService.authStateChange.subscribe({
      next: ((response) => {
        if (response) {
          this.loggedInUser = response;
        } else {
          this.loggedInUser = null;
        }
      })
    });
  }

  onLogout() {
    this._authenticationService.logout();
  }




  ngOnDestroy() {
    if (this._authStateChange$) {
      this._authStateChange$.unsubscribe();
    }
  }

  onVersion() {
    window.open('https://github.com/TomasiDeveloping/PlayerManagement', '_blank');
  }
}
