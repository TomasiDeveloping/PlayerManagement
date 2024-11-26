import {Component, inject, OnInit} from '@angular/core';
import {JwtTokenService} from "../../services/jwt-token.service";
import {UserModel} from "../../models/user.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserService} from "../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {HttpErrorResponse} from "@angular/common/http";
import Swal from "sweetalert2";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit{

  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  public userForm: FormGroup | undefined;
  public isEditMode: boolean = false;
  public currentUser!: UserModel;

  get f() {
    return this.userForm!.controls;
  }

  ngOnInit() {
    const userId = this._tokenService.getUserId();

    if (!userId) {
      return;
    }

    this._userService.getUser(userId).subscribe({
      next: (response: UserModel) => {
        if (response) {
          this.createUserForm(response);
          this.currentUser = response;
        }
      },
      error: (error: HttpErrorResponse) => {
        console.log(error);
        this._toastr.error('Error load user', 'Load users');
      }
    });
  }

  createUserForm(user: UserModel) {
    this.userForm = new FormGroup({
      id: new FormControl<string>(user.id),
      email: new FormControl({value: user.email, disabled: true}),
      role: new FormControl({value: user.role, disabled: true}),
      playerName: new FormControl(user.playerName, [Validators.required]),
    });
    this.userForm.disable();
  }

  onEdit() {
    this.userForm!.controls['playerName'].enable();
    this.isEditMode = true;
  }

  onCancel() {
    this.userForm!.reset();
    this.createUserForm(this.currentUser);
    this.isEditMode = false;
    this.userForm!.disable();
  }

  onSubmit() {
    if (this.userForm!.invalid) {
      return;
    }

    const user = this.userForm!.getRawValue();
    this._userService.updateUser(user.id, user).subscribe({
      next: ((response: UserModel) => {
        if (response) {
          this._toastr.success("User updated successfully.", 'Update user');
          Swal.fire('Auto logged out', 'You have to log in again and will be logged out automatically', 'warning')
            .then(() => this._authenticationService.logout());
        }
      }),
      error: ((error: HttpErrorResponse) => {
        console.log(error);
        this._toastr.error("Error updating user", 'Update user');
      })
    })
  }
}
