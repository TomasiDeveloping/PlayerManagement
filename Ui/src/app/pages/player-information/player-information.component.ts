import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {PlayerModel} from "../../models/player.model";
import {PlayerService} from "../../services/player.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerNoteModalComponent} from "../../modals/player-note-modal/player-note-modal.component";
import {PlayerAdmonitionModalComponent} from "../../modals/player-admonition-modal/player-admonition-modal.component";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-player-information',
  templateUrl: './player-information.component.html',
  styleUrl: './player-information.component.css'
})
export class PlayerInformationComponent implements OnInit {

  public currentPlayer: PlayerModel | undefined;
  public playerId: string = '';
  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _toastr: ToastrService = inject(ToastrService);

  ngOnInit() {
    this.playerId = this._activatedRote.snapshot.params['id'];

    this.getPlayer(this.playerId);
  }

  getPlayer(playerId: string) {
    this._playerService.getPlayer(playerId).subscribe({
      next: ((response: PlayerModel) => {
        if (response) {
          this.currentPlayer = response;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load player', 'Error loading player');
      })
    });
  }


  openPlayerNotes(player: PlayerModel) {
    const modalRef = this._modalService.open(PlayerNoteModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.player = player
    modalRef.dismissed.subscribe({
      next: (() => {
        this.getPlayer(player.id);
      })
    });
  }

  openPlayerAdmonitions(currentPlayer: PlayerModel) {
    const modalRef = this._modalService.open(PlayerAdmonitionModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.player = currentPlayer
    modalRef.dismissed.subscribe({
      next: (() => {
        this.getPlayer(currentPlayer
          .id);
      })
    });
  }

}
