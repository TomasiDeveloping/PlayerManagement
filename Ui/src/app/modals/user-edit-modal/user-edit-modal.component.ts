import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {UpdateUserModel, UserModel} from "../../models/user.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-user-edit-modal',
  templateUrl: './user-edit-modal.component.html',
  styleUrl: './user-edit-modal.component.css'
})
export class UserEditModalComponent implements OnInit {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toasterService: ToastrService = inject(ToastrService);

  @Input({required: true}) currentUser!: UserModel;

  userForm!: FormGroup;
  public roles: string[] = ['Administrator', 'User', 'Guest'];

  get f() {
    return this.userForm.controls;
  }

  ngOnInit() {
    this.userForm = new FormGroup({
      id: new FormControl<string>(this.currentUser.id, [Validators.required]),
      email: new FormControl<string>({ value: this.currentUser.email, disabled: true }, Validators.required),
      role: new FormControl<string>(this.currentUser.role, [Validators.required]),
      playerName: new FormControl<string>(this.currentUser.playerName, [Validators.required]),
    });
  }

  onSubmit() {
    if (this.userForm.invalid) {
      return;
    }

    const updateUserModel: UpdateUserModel = this.userForm.value as UpdateUserModel;
    this._userService.updateUser(updateUserModel.id, updateUserModel).subscribe({
      next: ((response) => {
        if (response) {
          this._toasterService.success('Update successfully', 'Update User');
          this.activeModal.close(response)
        }
      }),
      error: (error) => {
        console.log(error);
        this._toasterService.error('Update Failed', 'Update User');
      }
    });
  }
}
