import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {AuthenticationService} from "../services/authentication.service";
import {LoggedInUser} from "../models/user.model";
import {Subscription} from "rxjs";
import {environment} from "../../environments/environment";

declare var kofiWidgetOverlay: any;

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
          this.loadKoFiWidget();
        } else {
          this.loggedInUser = null;
          this.removeKoFiWidget();
        }
      })
    });
  }


  onLogout() {
    this._authenticationService.logout();
    this.removeKoFiWidget();
  }

  loadKoFiWidget() {
    if (document.getElementById('ko-fi-script')) return;

    const script = document.createElement('script');
    script.src = 'https://storage.ko-fi.com/cdn/scripts/overlay-widget.js';
    script.onload = () => {
      kofiWidgetOverlay.draw('tomasideveloping', {
        'type': 'floating-chat',
        'floating-chat.donateButton.text': 'Donate',
        'floating-chat.donateButton.background-color': '#794bc4',
        'floating-chat.donateButton.text-color': '#fff'
      });
    };
    document.body.appendChild(script);
  }

  removeKoFiWidget(): void {
    const widget = document.querySelector('.kofi-widget');
    if (widget) widget.remove();

    const script = document.getElementById('ko-fi-script');
    if (script) script.remove();
  }

  ngOnDestroy() {
    this.removeKoFiWidget();
    if (this._authStateChange$) {
      this._authStateChange$.unsubscribe();
    }
  }

  onVersion() {
    window.open('https://github.com/TomasiDeveloping/PlayerManagement', '_blank');
  }
}
