import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerService} from "../../services/player.service";
import {PlayerModel} from "../../models/player.model";

@Component({
  selector: 'app-zombie-siege-participants-modal',
  templateUrl: './zombie-siege-participants-modal.component.html',
  styleUrl: './zombie-siege-participants-modal.component.css'
})
export class ZombieSiegeParticipantsModalComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  public playerParticipated: { playerId: string, playerName: string, survivedWaves: number; }[] = [];
  public waves: number[] = [0,1,2,3,4,5,6,7,8,9,10, 11,12,13,14,15,16,17,18,19,20];
  public isUpdate: boolean = false;

  @Input() players: { playerId: string, playerName: string, survivedWaves: number; }[] | undefined;
  @Input({required: true}) allianceId!: string;

  ngOnInit() {
    if (this.players) {
      this.playerParticipated  = [...this.players];
      this.isUpdate = true;
    } else {
      this.getPlayers();
      this.isUpdate = false;
    }
  }

  getPlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((player: PlayerModel) => {
            this.playerParticipated.push({playerId: player.id, playerName: player.playerName, survivedWaves: 0})
          });
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
      })
    });
  }
}
