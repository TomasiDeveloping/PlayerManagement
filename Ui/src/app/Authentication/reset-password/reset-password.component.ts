import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {AuthenticationService} from "../../services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PasswordValidators} from "../../helpers/passwordValidators";
import {ResetPasswordModel} from "../../models/resetPassword.model";

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {

  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  private token: string | undefined;
  private email: string | undefined;

  public showError: boolean = false;
  public isSuccess: boolean = false;
  public resetPasswordForm: FormGroup | undefined;
  isInputText: boolean = false;

  get password() {
    return this.resetPasswordForm!.get('password');
  }

  get confirmPassword() {
    return this.resetPasswordForm!.get('confirmPassword');
  }

  ngOnInit() {
    this.token = this._activatedRoute.snapshot.queryParams['token'];
    this.email = this._activatedRoute.snapshot.queryParams['email'];

    if (!this.token || !this.email) {
      this.showError = true;
      return;
    }
    this.createResetPasswordForm();
  }

  createResetPasswordForm() {
    this.resetPasswordForm = new FormGroup({
      confirmPassword: new FormControl<string>('', [Validators.required]),
      password: new FormControl<string>('', Validators.compose([
        Validators.required,
        PasswordValidators.patternValidator(new RegExp("(?=.*[0-9])"), {hasNumber: true}),
        PasswordValidators.patternValidator(new RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
        PasswordValidators.patternValidator(new RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
        PasswordValidators.patternValidator(new RegExp("(?=.*[$@^!%*?&+#])"), {hasSpecialCharacters: true}),
        Validators.minLength(8)
      ]))
    }, {
      validators: PasswordValidators.passwordMatch('password', 'confirmPassword')
    });
  }

  onSubmit() {
    if (this.resetPasswordForm?.invalid) {
      return;
    }

    const resetPasswordModel: ResetPasswordModel = {
      confirmPassword: this.confirmPassword?.value,
      password: this.password?.value,
      email: this.email!,
      token: this.token!,
    }

    this._authenticationService.resetPassword(resetPasswordModel).subscribe({
      next: ((response) => {
        if (response) {
          this.isSuccess = true;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('An error occurred. Please try again later.', 'Error');
      })
    });
  }
}
