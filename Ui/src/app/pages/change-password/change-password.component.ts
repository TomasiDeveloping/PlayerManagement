import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PasswordValidators} from "../../helpers/passwordValidators";
import {AuthenticationService} from "../../services/authentication.service";
import {UserService} from "../../services/user.service";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";
import {HttpErrorResponse} from "@angular/common/http";
import {JwtTokenService} from "../../services/jwt-token.service";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {

  public isPasswordType: boolean = true;
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);

  public changePasswordForm: FormGroup = new FormGroup({
    userId: new FormControl<string>(this._tokenService.getUserId()!),
    currentPassword: new FormControl<string>('', [Validators.required]),
    confirmPassword: new FormControl<string>('', [Validators.required]),
    newPassword: new FormControl<string>('', Validators.compose([
      Validators.required,
      PasswordValidators.patternValidator(RegExp("(?=.*[0-9])"), {hasNumber: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
      PasswordValidators.patternValidator(RegExp("(?=.*[$@^!%*?&+#])"), {hasSpecialCharacters: true}),
      Validators.minLength(8)
    ]))
  }, {
    validators: PasswordValidators.passwordMatch('newPassword', 'confirmPassword')
  });

  get f() {
    return this.changePasswordForm.controls;
  }

  onSubmit() {
    if (this.changePasswordForm.invalid) {
      return;
    }

    const changePasswordModel = this.changePasswordForm.value;

    this._userService.changeUserPassword(changePasswordModel).subscribe({
      next: ((response: boolean) => {
        if (response) {
          Swal.fire('Change password', 'Password changed successfully. You will be logged out automatically', 'success')
            .then(() => this._authenticationService.logout());
        }
      }),
      error: (error: HttpErrorResponse) => {
        console.log(error);
        this._toastr.error(error.error.name ?? 'Something went wrong');
      }
    });
  }
}
