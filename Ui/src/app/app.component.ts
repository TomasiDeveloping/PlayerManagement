import {Component, inject, OnInit} from '@angular/core';
import {AuthenticationService} from "./services/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Last War - Player Management';

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  ngOnInit() {
    this._authenticationService.autoLogin();
  }
}
