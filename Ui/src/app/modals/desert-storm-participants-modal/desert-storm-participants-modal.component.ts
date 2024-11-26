import {Component, inject, Input, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerModel} from "../../models/player.model";

@Component({
  selector: 'app-desert-storm-participants-modal',
  templateUrl: './desert-storm-participants-modal.component.html',
  styleUrl: './desert-storm-participants-modal.component.css'
})
export class DesertStormParticipantsModalComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);

  public playerParticipated: {playerId: string, playerName: string, participated: boolean, registered: boolean, startPlayer: boolean}[] = [];

  @Input({required: true}) allianceId!: string;
  @Input() players: {playerId: string, playerName: string, participated: boolean, registered: boolean, startPlayer: boolean}[] | undefined;

  ngOnInit() {
    if (this.players) {
      this.playerParticipated = [...this.players];
    } else {
      this.getPlayers();
    }
  }

  getPlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((player: PlayerModel) => {
            this.playerParticipated.push({playerId: player.id, playerName: player.playerName, participated: false, registered: false, startPlayer: false});
          });
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
      })
    });
  }

  onRegisterChange(player: {
    playerId: string;
    playerName: string;
    participated: boolean;
    registered: boolean;
    startPlayer: boolean
  }) {
    player.registered = !player.registered;
    if (!player.registered) {
      player.participated = false;
      player.startPlayer = false;
    }
  }
}
