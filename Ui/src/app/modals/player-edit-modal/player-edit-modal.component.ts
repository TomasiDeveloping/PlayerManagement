import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {CreatePlayerModel, PlayerModel, UpdatePlayerModel} from "../../models/player.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {RankService} from "../../services/rank.service";
import {RankModel} from "../../models/rank.model";
import {PlayerService} from "../../services/player.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-player-edit-modal',
  templateUrl: './player-edit-modal.component.html',
  styleUrl: './player-edit-modal.component.css'
})
export class PlayerEditModalComponent implements OnInit {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private readonly _rankService: RankService = inject(RankService);
  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _toasterService: ToastrService = inject(ToastrService);

  @Input({required: true}) currentPlayer!: PlayerModel;
  @Input({required: true}) isUpdate!: boolean;

  playerForm!: FormGroup;
  ranks: RankModel[] = [];

  get f() {
    return this.playerForm.controls;
  }

  ngOnInit() {
    this.playerForm = new FormGroup({
      id: new FormControl<string>(this.currentPlayer.id),
      playerName: new FormControl<string>(this.currentPlayer.playerName, [Validators.required, Validators.maxLength(250)]),
      rankId: new FormControl<string>(this.currentPlayer.rankId, [Validators.required]),
      allianceId: new FormControl<string>(this.currentPlayer.allianceId, [Validators.required]),
      level: new FormControl<number>(this.currentPlayer.level, [Validators.required])
    });
    this.getRanks();
  }

  getRanks() {
    this._rankService.getRanks().subscribe({
      next: ((response: RankModel[]): void => {
        if (response) {
          this.ranks = response;
        }
      })
    })
  }

  onSubmit() {
    if (this.playerForm.invalid) {
      return;
    }
    if (this.isUpdate) {
      const player: UpdatePlayerModel = this.playerForm.value as UpdatePlayerModel;
      this.updatePlayer(player.id, player);
    } else {
      const player: CreatePlayerModel = this.playerForm.value as CreatePlayerModel;
      this.insertPlayer(player);
    }
  }

  updatePlayer(playerId: string, player: UpdatePlayerModel) {
    this._playerService.updatePlayer(playerId, player).subscribe({
      next: ((response) => {
        if (response) {
          this._toasterService.success('Player successfully updated!', 'Update Player');
          this.activeModal.close(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toasterService.error('Player could not be updated', 'Error update Player')
      })
    });
  }

  insertPlayer(player: CreatePlayerModel) {
    this._playerService.insertPlayer(player).subscribe({
      next: ((response) => {
        if (response) {
          this._toasterService.success('Player successfully created!', 'Create Player');
          this.activeModal.close(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toasterService.error('Player could not be created', 'Error create Player')
      })
    })
  }
}
