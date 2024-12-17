import {Component, inject, Input, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerModel} from "../../models/player.model";
import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-custom-event-participants-model',
  templateUrl: './custom-event-participants-model.component.html',
  styleUrl: './custom-event-participants-model.component.css'
})
export class CustomEventParticipantsModelComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);

  public playerParticipated: {playerId: string, playerName: string, participated: boolean, achievedPoints: number}[] = [];
  public participantsForm!: FormGroup;
  public isUpdate: boolean = false;

  @Input({required: true}) allianceId!: string;
  @Input({required: true}) isPointEvent!: boolean;
  @Input({required: true}) isParticipationEvent!: boolean;
  @Input() players: {playerId: string, playerName: string, participated: boolean, achievedPoints: number}[] | undefined;

  get customEventParticipants(): FormArray {
    return this.participantsForm.get('customEventParticipants') as FormArray;
  }


  ngOnInit() {
    if (this.players) {
      this.isUpdate = true;
      this.playerParticipated = [...this.players];
      this.createParticipantForm();
    } else {
      this.getPlayers();
    }
  }

  getPlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((player: PlayerModel) => {
            this.playerParticipated.push({playerId: player.id, playerName: player.playerName, participated: false, achievedPoints: 0});
          });
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
        this.createParticipantForm();
      })
    });
  }

  createParticipantForm() {
    this.participantsForm = new FormGroup({
      customEventParticipants: new FormArray([])
    });
    this.playerParticipated.forEach(player => {
      this.customEventParticipants.push(new FormGroup({
        id: new FormControl<string>(''),
        playerId: new FormControl(player.playerId),
        playerName: new FormControl<string>(player.playerName),
        participated: new FormControl<boolean>(player.participated),
        achievedPoints: new FormControl<number>(player.achievedPoints, [Validators.required, Validators.pattern('(0|[1-9]\\d*)')])
      }))
    });
  }

  onSubmit() {
    const participants = this.participantsForm.value.customEventParticipants;
    this.activeModal.close(participants);
  }
}
