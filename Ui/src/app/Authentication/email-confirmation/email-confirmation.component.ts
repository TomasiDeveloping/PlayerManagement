import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {AuthenticationService} from "../../services/authentication.service";
import {ConfirmEmailRequestModel} from "../../models/confirmEmailRequest.model";
import {EmailConfirmationRequestModel} from "../../models/emailConfirmationRequest.model";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrl: './email-confirmation.component.css'
})
export class EmailConfirmationComponent implements OnInit {

  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  public showSuccess: boolean = false;
  public showError: boolean = false;
  public showResendSuccess: boolean = false;

  public email: string | null = null;

  ngOnInit() {

    const token = this._activatedRoute.snapshot.queryParams['token'];
    this.email = this._activatedRoute.snapshot.queryParams['email'];

    if (!token || !this.email) {
      this.showError = true;
      return;
    }

    this.confirmEmail(this.email, token);
  }

  onSendNewConfirmationEmail() {
    if (!this.email) {
      this._toastr.error('No email address found. Please contact support for assistance.', 'Missing Email');
    }
    const emailConfirmation: EmailConfirmationRequestModel = {
      email: this.email!,
      clientUri: environment.emailConfirmUri
    };

    this._authenticationService.resendConfirmationEmail(emailConfirmation).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('A new confirmation email has been successfully sent to your address.', 'Email Sent');
          this.showResendSuccess = true;
          this.showError = false;
        }
      }),
      error: (error) => {
        console.log(error);
        this._toastr.error('An error occurred while attempting to send the confirmation email. Please try again later.', 'Error Sending Email');
      }
    });
  }

  confirmEmail(email: string, token: string) {
    const confirmEmailRequest: ConfirmEmailRequestModel = {
      email: email,
      token: token
    }
    this._authenticationService.confirmEmail(confirmEmailRequest).subscribe({
      next: ((response) => {
        if (response) {
          this.showSuccess = true;
        }
      }),
      error: (error) => {
        console.log(error);
        this.showError = true;
      }
    });
  }
}
