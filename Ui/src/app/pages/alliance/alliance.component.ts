import {Component, inject, OnInit} from '@angular/core';
import {AllianceService} from "../../services/alliance.service";
import {AllianceModel} from "../../models/alliance.model";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {JwtTokenService} from "../../services/jwt-token.service";
import {UserModel} from "../../models/user.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {InviteUserModalComponent} from "../../modals/invite-user-modal/invite-user-modal.component";
import {UserService} from "../../services/user.service";
import {UserEditModalComponent} from "../../modals/user-edit-modal/user-edit-modal.component";
import Swal from "sweetalert2";

@Component({
  selector: 'app-alliance',
  templateUrl: './alliance.component.html',
  styleUrl: './alliance.component.css'
})
export class AllianceComponent implements OnInit {


  private readonly _allianceService: AllianceService = inject(AllianceService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _modalService : NgbModal = inject(NgbModal);
  private readonly _userService: UserService = inject(UserService);

  private allianceId = this._tokenService.getAllianceId();

  public allianceForm: FormGroup | undefined;
  public currentAlliance: AllianceModel | undefined;
  active: number = 1;
  public users: UserModel[] = [];
  page: number = 1;

  get f() {
    return this.allianceForm!.controls;
  }

  ngOnInit() {
    this.getAlliance(this.allianceId!);
    this.getAllianceUsers(this.allianceId!);
  }

  getAlliance(allianceId: string) {
    this._allianceService.getAlliance(allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentAlliance = response;
          this.createAllianceForm(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load alliance', 'Error load alliance');
      })
    });
  }

  getAllianceUsers(allianceId: string) {
    this._userService.getAllianceUsers(allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.users = response;
        } else {
          this.users = [];
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load alliance users', 'Error load users');
      })
    })
  }

  createAllianceForm(alliance: AllianceModel) {
    this.allianceForm = new FormGroup({
      id: new FormControl<string>(alliance.id),
      server: new FormControl<number>(alliance.server, [Validators.required]),
      name: new FormControl<string>(alliance.name, [Validators.required, Validators.maxLength(200)]),
      abbreviation: new FormControl<string>(alliance.abbreviation, [Validators.required, Validators.maxLength(4)]),
    });
  }

  onSubmit() {
    if (this.allianceForm?.invalid) {
      return;
    }

    const alliance: AllianceModel = this.allianceForm?.value;

    this._allianceService.updateAlliance(alliance.id, alliance).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated alliance', 'Update');
          this.currentAlliance = response;
          this.createAllianceForm(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Failed to update alliance', 'Update failed');
        this.getAlliance(alliance.id);
      })
    });
  }

  onEditUser(user: UserModel) {
    const modalRef = this._modalService.open(UserEditModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.currentUser = user;
    modalRef.closed.subscribe({
      next: ((response: UserModel) => {
        if (response) {
          this.getAllianceUsers(this.allianceId!);
        }
      })
    })
  }

  onDeleteUser(user: UserModel) {
    Swal.fire({
      title: "Delete User ?",
      text: `Do you really want to delete the user ${user.playerName}`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._userService.deleteUser(user.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "User has been deleted",
                icon: "success"
              }).then(_ => this.getAllianceUsers(this.allianceId!));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onInviteUser() {
    const modalRef = this._modalService.open(InviteUserModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.userId = this._tokenService.getUserId();
    modalRef.componentInstance.allianceId = this.allianceId;
    modalRef.closed.subscribe({
      next: ((response) => {
        if (response) {
        }
      })
    })
  }
}
