import {Component, inject} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastrService} from "ngx-toastr";
import {AuthenticationService} from "../../services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ForgotPasswordModel} from "../../models/forgotPassword.model";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private readonly _toasterService: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  public showSendMessage: boolean = false;

  forgotPasswordForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
  });

  get f() {
    return this.forgotPasswordForm.controls;
  }

  onSubmit() {
    if (this.forgotPasswordForm.invalid) {
      return;
    }
    const forgotPasswordModel: ForgotPasswordModel = {
      email: this.forgotPasswordForm.get('email')?.value,
      resetPasswordUri: environment.resetPasswordUrl
    };

    this._authenticationService.forgotPassword(forgotPasswordModel).subscribe({
      next: (response) => {
        if (response) {
          this.showSendMessage = true;
        }
      },
      error: (error) => {
        console.log(error);
        this._toasterService.error('An error occurred. Please try again later.', 'Error');
      }
    });
  }

}
