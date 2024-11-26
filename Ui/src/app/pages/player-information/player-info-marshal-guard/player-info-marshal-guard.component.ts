import {Component, inject, Input} from '@angular/core';
import {MarshalGuardParticipantService} from "../../../services/marshal-guard-participant.service";
import {ToastrService} from "ngx-toastr";
import {MarshalGuardParticipantModel} from "../../../models/marshalGuardParticipant.model";

@Component({
  selector: 'app-player-info-marshal-guard',
  templateUrl: './player-info-marshal-guard.component.html',
  styleUrl: './player-info-marshal-guard.component.css'
})
export class PlayerInfoMarshalGuardComponent {

  public playerGuardsLoaded: boolean = false;
  public marshalType: string = 'success';
  public playerGuards: number = 0;
  public totalMarshalGuards: number = 0;
  public numberOfLoadMarshalGuards: number = 10;
  public progressValue: number = 0;
  @Input({required: true}) playerId!: string;
  private readonly _marshalGuardParticipantService: MarshalGuardParticipantService = inject(MarshalGuardParticipantService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public onReloadMarshalGuards(): void {
    this.getNumberOfParticipants(this.playerId, this.numberOfLoadMarshalGuards);
  }

  private getNumberOfParticipants(playerId: string, last: number): void {
    this.totalMarshalGuards = 0;
    this.playerGuards = 0;
    this._marshalGuardParticipantService.getPlayerMarshalGuardParticipants(playerId, last).subscribe({
      next: ((response: MarshalGuardParticipantModel[]) => {
        if (response) {
          this.playerGuardsLoaded = true;
          this.totalMarshalGuards = response.length;
          if (this.totalMarshalGuards < last) {
            this._toastr.info('Fewer Marshal Guards were held than wanted to be loaded');
            this.numberOfLoadMarshalGuards = response.length;
          }
          response.forEach((marshalGuardParticipant: MarshalGuardParticipantModel) => {
            if (marshalGuardParticipant.participated) {
              this.playerGuards++;
            }
          });
          this.getMarshalValue();
        }
      })
    });
  }

  private getMarshalValue(): void {
    const percent: number = (this.playerGuards / this.totalMarshalGuards) * 100;
    this.setMarshalColor(percent);
    this.progressValue = percent;
  }

  private setMarshalColor(percent: number): void {
    if (percent <= 20) {
      this.marshalType = 'danger';
    } else if (percent > 20 && percent <= 50) {
      this.marshalType = 'warning';
    } else if (percent > 50 && percent <= 70) {
      this.marshalType = 'primary';
    } else {
      this.marshalType = 'success';
    }
  }
}
