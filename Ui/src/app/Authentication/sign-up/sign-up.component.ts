import {Component, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {PasswordValidators} from "../../helpers/passwordValidators";
import {SignUpRequestModel} from "../../models/signUp.model";
import {AuthenticationService} from "../../services/authentication.service";
import {ToastrService} from "ngx-toastr";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {

  private readonly _router: Router = inject(Router);
  private readonly _authenticationService = inject(AuthenticationService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public isSignUpSuccess: boolean = false;
  public playerName: string = '';
  public allianceName: string = '';

  isInputText: boolean = false;

  public signUpForm: FormGroup = new FormGroup({
    email: new FormControl<string>('',  [Validators.required, Validators.email]),
    playerName: new FormControl<string>('', [Validators.required]),
    allianceServer: new FormControl<number | null>(null, [Validators.required]),
    allianceName: new FormControl<string>('', [Validators.required]),
    allianceAbbreviation: new FormControl<string>('', [Validators.required, Validators.maxLength(5)]),
    confirmPassword: new FormControl('', [Validators.required]),
    password: new FormControl<string>('', Validators.compose([
      Validators.required,
      PasswordValidators.patternValidator(new RegExp("(?=.*[0-9])"), {hasNumber: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[A-Z])"), {hasCapitalCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[a-z])"), {hasSmallCase: true}),
      PasswordValidators.patternValidator(new RegExp("(?=.*[$@^!%*?&+])"), {hasSpecialCharacters: true}),
      Validators.minLength(8)
    ]))
  }, {
    validators: PasswordValidators.passwordMatch('password', 'confirmPassword')
  });


  get f() {
    return this.signUpForm.controls;
  }


  onCancel() {
    this._router.navigate(['login']).then();
  }

  onSignUp() {
    if (this.signUpForm.invalid) {
      return;
    }

    const register: SignUpRequestModel = this.signUpForm.value as SignUpRequestModel;
    register.emailConfirmUri = environment.emailConfirmUri;

    this._authenticationService.signUp(register).subscribe({
      next: ((response) => {
        if (response) {
          this.playerName = register.playerName;
          this.allianceName = register.allianceName;
          this._toastr.success('Successfully signed up', 'Sign Up');
          this.isSignUpSuccess = true;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error(error.error.name ?? 'Sign up failed', 'Sign up');
      })
    });

  }
}
