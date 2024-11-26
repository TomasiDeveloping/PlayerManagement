import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerService} from "../../services/player.service";
import {PlayerModel} from "../../models/player.model";

@Component({
  selector: 'app-marshal-guard-modal',
  templateUrl: './marshal-guard-modal.component.html',
  styleUrl: './marshal-guard-modal.component.css'
})
export class MarshalGuardModalComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  public playerParticipated: {playerId: string, playerName: string, participated: boolean}[] = [];

  @Input({required: true}) allianceId!: string;
  @Input() players: { playerId: string; playerName: string; participated: boolean; }[] | undefined;

  ngOnInit() {
    if (this.players) {
      this.playerParticipated  = [...this.players];
    } else {
      this.getPlayers();
    }
  }

  getPlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((player: PlayerModel) => {
            this.playerParticipated.push({playerId: player.id, playerName: player.playerName, participated: false})
          });
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
      })
    });
  }

}
