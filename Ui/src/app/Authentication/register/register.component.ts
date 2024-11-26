import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {AuthenticationService} from "../../services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PasswordValidators} from "../../helpers/passwordValidators";
import {RegisterUserModel} from "../../models/registerUser.model";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {

  private readonly _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  public showError: boolean = false;
  public showSuccess: boolean = false;
  public registerForm: FormGroup | undefined;
  isInputText: boolean = false;
  playerName: string = "";

  ngOnInit() {

    const role = this._activatedRoute.snapshot.queryParams['role'];
    const allianceId = this._activatedRoute.snapshot.queryParams['allianceId'];
    const email = this._activatedRoute.snapshot.queryParams['email'];

    if (!role || !email || !allianceId) {
      this.showError = true;
      return;
    }
    this.createRegisterForm(role, email, allianceId);
  }

  get f() {
    return this.registerForm!.controls;
  }

  createRegisterForm(role: string, email: string, allianceId: string) {
    this.registerForm = new FormGroup({
      email: new FormControl<string>(email, [Validators.required, Validators.email]),
      allianceId: new FormControl<string>(allianceId, [Validators.required]),
      roleId: new FormControl(role, [Validators.required]),
      playerName: new FormControl<string>('', [Validators.required]),
      confirmPassword: new FormControl<string>('', [Validators.required]),
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
  }

  onSubmit() {
    this.playerName = this.registerForm?.value.playerName;

    const registerUserModel: RegisterUserModel = this.registerForm!.value;
    registerUserModel.emailConfirmUri = environment.emailConfirmUri;

    this._authenticationService.registerUser(registerUserModel).subscribe({
      next: ((response) => {
        if (response) {
          this.playerName = registerUserModel.playerName;
          this._toastr.success('Registration successful', 'Register');
          this.showSuccess = true;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error(error.error.name ?? 'Registration failed', 'Register');
      })
    });
  }
}
