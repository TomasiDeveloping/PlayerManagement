import {Component, inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthenticationService} from "../../services/authentication.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {LoginRequestModel} from "../../models/login.model";
import {environment} from "../../../environments/environment";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ForgotPasswordComponent} from "../forgot-password/forgot-password.component";
import {StatService} from "../../services/stat.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  public isPasswordType: boolean = true;
  public currentYear: number = new Date().getFullYear();
  public version: string = environment.version;

  public loginForm: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);
  private readonly _statService: StatService = inject(StatService);
  private readonly _router: Router = inject(Router);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _modalService : NgbModal = inject(NgbModal);
  allianceCount: number | null = null;

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  ngOnInit() {
    this.fetchAllianceCount();
  }

  fetchAllianceCount() {
    this._statService.getAllianceUseCount().subscribe({
      next: ((response => {
        this.allianceCount = response;
      }))
    })
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const loginRequest: LoginRequestModel = this.loginForm.value as LoginRequestModel;

    this._authenticationService.login(loginRequest).subscribe({
      next: ((response) => {
        if (response) {
          this._router.navigate(['/']).then();
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error(error.error.name ?? 'An error occurred while trying to log in', 'Login');
      })
    });
  }

  onForgotPassword(): void {
    this._modalService.open(ForgotPasswordComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
  }

  onSignUp() {
    this._router.navigate(['sign-up']).then();
  }

  onVersion() {
    window.open('https://github.com/TomasiDeveloping/PlayerManagement', '_blank');
  }

  onCompany() {
    window.open('https://tomasi-developing.ch', '_blank');
  }
}
