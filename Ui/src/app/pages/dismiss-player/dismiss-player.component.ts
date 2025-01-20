import {Component, inject, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {ToastrService} from "ngx-toastr";
import {PlayerModel} from "../../models/player.model";
import {JwtTokenService} from "../../services/jwt-token.service";
import Swal from "sweetalert2";
import {Router} from "@angular/router";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {
  PlayerDismissInformationModalComponent
} from "../../modals/player-dismiss-information-modal/player-dismiss-information-modal.component";

@Component({
  selector: 'app-dismiss-player',
  templateUrl: './dismiss-player.component.html',
  styleUrl: './dismiss-player.component.css'
})
export class DismissPlayerComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _router: Router = inject(Router);
  private readonly _modalService: NgbModal = inject(NgbModal);

  private allianceId: string = this._tokenService.getAllianceId()!;

  public dismissedPlayers: PlayerModel[] = [];
  itemsPerPage: string | number = 10;
  page: string | number = 1;



  ngOnInit() {
    this.getDismissedPlayers();
  }

  getDismissedPlayers() {
    this._playerService.getDismissedPlayers(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.dismissedPlayers = response;
        } else {
          this.dismissedPlayers = [];
        }
      }),
      error: (error) => {
        console.log(error);
        this._toastr.error('Could not get dismissedPlayers', 'Get DismissedPlayers');
      }
    })
  }

  onDeletePlayer(player: PlayerModel) {
    Swal.fire({
      title: `Delete player ${player.playerName}?`,
      text: `All data is permanently deleted and cannot be restored`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._playerService.deletePlayer(player.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Player has been deleted",
                icon: "success"
              }).then(_ => this.getDismissedPlayers());
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onReactivePlayer(player: any) {
    Swal.fire({
      title: `Reactive player ${player.playerName}?`,
      text: `The player is accepted back into the alliance`,
      icon: "info",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes reactive player"
    }).then((result) => {
      if (result.isConfirmed) {
        this._playerService.reactivePlayer(player.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Reactivated!",
                text: "Player has been reactivated",
                icon: "success"
              }).then(_ => this._router.navigate(['/players']));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onPlayerInformation(player: PlayerModel) {
    const modalRef = this._modalService.open(PlayerDismissInformationModalComponent,
      {animation: true, backdrop: true, centered: true, size: 'lg', scrollable: true});
    modalRef.componentInstance.playerId = player.id;
  }
}
