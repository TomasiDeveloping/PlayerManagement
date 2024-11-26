import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {InviteUserModel} from "../../models/inviteUser.model";
import {AuthenticationService} from "../../services/authentication.service";
import {environment} from "../../../environments/environment";


@Component({
  selector: 'app-invite-user-modal',
  templateUrl: './invite-user-modal.component.html',
  styleUrl: './invite-user-modal.component.css'
})
export class InviteUserModalComponent implements OnInit {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private readonly _toasterService: ToastrService = inject(ToastrService);
  private readonly _authenticationService: AuthenticationService = inject(AuthenticationService);

  @Input({required: true}) userId!: string;
  @Input({required: true}) allianceId!: string;

  inviteUserForm!: FormGroup;
  public roles: string[] = ['Administrator', 'User', 'Guest'];

  get f() {
    return this.inviteUserForm.controls;
  }

    ngOnInit() {
    this.inviteUserForm = new FormGroup({
      userId: new FormControl<string>(this.userId, [Validators.required]),
      allianceId: new FormControl<string>(this.allianceId, [Validators.required]),
      email: new FormControl<string>('' , [Validators.required, Validators.email]),
      role: new FormControl('', [Validators.required]),
    })
  }

  onSubmit() {
    if (this.inviteUserForm.invalid) {
      return;
    }
    const inviteUserModel = this.inviteUserForm.value as InviteUserModel;
    inviteUserModel.invitingUserId = this.userId;
    inviteUserModel.registerUserUri = environment.registerUserUri;

    this._authenticationService.inviteUser(inviteUserModel).subscribe({
      next: (result) => {
        if (result) {
          this._toasterService.success('User successfully invite', 'Invite User');
          this.activeModal.close();
        }
      },
      error: error => {
        console.log(error);
        this._toasterService.error('Error in invite user', 'Invite User');
      }
    });
  }
}
