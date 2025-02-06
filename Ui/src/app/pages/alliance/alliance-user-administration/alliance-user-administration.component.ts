import {Component, inject, Input, OnInit} from '@angular/core';
import {UserModel} from "../../../models/user.model";
import {UserEditModalComponent} from "../../../modals/user-edit-modal/user-edit-modal.component";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {UserService} from "../../../services/user.service";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";
import {InviteUserModalComponent} from "../../../modals/invite-user-modal/invite-user-modal.component";
import {JwtTokenService} from "../../../services/jwt-token.service";

@Component({
  selector: 'app-alliance-user-administration',
  templateUrl: './alliance-user-administration.component.html',
  styleUrl: './alliance-user-administration.component.css'
})
export class AllianceUserAdministrationComponent implements OnInit {

  public users: UserModel[] = [];
  public page: number = 1;
  @Input('allianceId') allianceId!: string;
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _userService: UserService = inject(UserService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);

  ngOnInit() {
    this.getAllianceUsers(this.allianceId);
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
}
