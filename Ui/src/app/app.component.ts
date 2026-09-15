import {Component, inject, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {AuthenticationService} from "./services/authentication.service";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AppComponent implements OnInit {
  title = 'Last War - Player Management';

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  ngOnInit() {
    this._authenticationService.autoLogin();
  }
}
