import {
  Component,
  inject,
  OnInit,
} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import { PlayerModel} from "../../models/player.model";
import {FormControl} from "@angular/forms";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerEditModalComponent} from "../../modals/player-edit-modal/player-edit-modal.component";
import Swal from 'sweetalert2'
import {Router} from "@angular/router";
import {JwtTokenService} from "../../services/jwt-token.service";



@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrl: './player.component.css'
})
export class PlayerComponent implements OnInit {


  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _modalService : NgbModal = inject(NgbModal);
  private readonly _router: Router = inject(Router);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);

  private allianceId = this._tokenService.getAllianceId();

  public players: PlayerModel[] = [];
  public activePlayers: PlayerModel[] = [];
  public r1Players: PlayerModel[] = [];
  public r2Players: PlayerModel[] = [];
  public r3Players: PlayerModel[] = [];
  public r4Players: PlayerModel[] = [];
  public filteredPlayers: PlayerModel[] = [];
  public page: number = 1;
  public itemsPerPage: number = 10;
  public filter = new FormControl('', { nonNullable: true });


  ngOnInit() {
    this.filter.valueChanges.subscribe({
      next: ((value) => {
        const term = value.toLowerCase();
          this.filteredPlayers = this.activePlayers.filter(player => {
            return player.playerName.toLowerCase().includes(term);
          })
      })
    });

    this.getPlayers(this.allianceId!);
  }

  getPlayers(allianceId: string) {
    this.players = [];
    this.r1Players = [];
    this.r2Players = [];
    this.r3Players = [];
    this.r4Players = [];
    this.filteredPlayers = [];
    this.activePlayers = [];

    this._playerService.getAlliancePlayer(allianceId).subscribe({
      next: ((response: PlayerModel[]): void => {
        if (response) {
          response.sort((a, b) => {
            const numA = parseInt(a.rankName.substring(1));
            const numB = parseInt(b.rankName.substring(1));
            return numB - numA;
          })
          this.players = response;
          this.activePlayers = [...this.players];
          this.filteredPlayers = [...this.players];

          response.forEach((player: PlayerModel) => {
            if (player.rankName === "R1") {
              this.r1Players.push(player);
            } else if (player.rankName === "R2") {
              this.r2Players.push(player);
            } else if (player.rankName === "R3") {
              this.r3Players.push(player);
            } else if (player.rankName === "R4") {
              this.r4Players.push(player);
            }
          })
        }
      }),
      error: (error: Error) => {
        console.log(error);
      }
    });
  }

  onEditPlayer(player: PlayerModel) {
    const modalRef = this._modalService.open(PlayerEditModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.currentPlayer = player;
    modalRef.componentInstance.isUpdate = true;
    modalRef.closed.subscribe({
      next: ((response: PlayerModel) => {
        if (response) {
          this.filter.patchValue('');
          this.getPlayers(response.allianceId);
        }
      })
    })
  }

  onDeletePlayer(player: PlayerModel) {
    Swal.fire({
      title: "Delete Player ?",
      text: `Do you really want to delete the player ${player.playerName}`,
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
              }).then(_ => this.getPlayers(this.allianceId!));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onAddNewPlayer() {
    const player: PlayerModel = {
      id: '',
      notesCount: 0,
      admonitionsCount: 0,
      allianceId: this.allianceId!,
      playerName: '',
      level: 0,
      rankId: '',
      createdOn: new Date(),
      rankName: '',
      createdBy: ''
    }
    const modalRef = this._modalService.open(PlayerEditModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.currentPlayer = player;
    modalRef.componentInstance.isUpdate = false;
    modalRef.closed.subscribe({
      next: ((response: PlayerModel) => {
        if (response) {
          this.filter.patchValue('');
          this.getPlayers(response.allianceId);
        }
      })
    })
  }

  onGoToPlayerInformation(player: PlayerModel) {
    this._router.navigate(['player-information', player.id]).then();
  }

  onRankFilterChange(event: any) {
    const rank = event.target.value;
    this.filter.patchValue('');

    switch (rank) {
      case 'R1': {
        this.activePlayers = [...this.r1Players];
      } break;
      case 'R2': {
        this.activePlayers = [...this.r2Players];
        break;
      }
      case 'R3': {
        this.activePlayers = [...this.r3Players];
        break;
      }
      case 'R4': {
        this.activePlayers = [...this.r4Players];
      } break;
      default: {
        this.activePlayers = [...this.players];
        break;
      }
    }
    this.filteredPlayers = [...this.activePlayers];
  }
}
