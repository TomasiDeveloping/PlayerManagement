import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {CustomEventService} from "../../../services/custom-event.service";
import {CustomEventDetailModel} from "../../../models/customEvent.model";

@Component({
  selector: 'app-custom-event-detail',
  templateUrl: './custom-event-detail.component.html',
  styleUrl: './custom-event-detail.component.css'
})
export class CustomEventDetailComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _customEventService: CustomEventService = inject(CustomEventService);

  private customEventId!: string;

  public customEventDetail: CustomEventDetailModel | undefined;
  public participatedPlayers: number = 0;

  ngOnInit() {
    this.customEventId = this._activatedRote.snapshot.params['id'];

    if (!this.customEventId) {
      // TODO
    }
    this.getCustomEventDetail(this.customEventId);
  }

  getCustomEventDetail(customEventId: string) {
    this._customEventService.getCustomEventDetail(customEventId).subscribe({
      next: ((response: CustomEventDetailModel) => {
        if (response) {
          this.participatedPlayers = 0;
          this.customEventDetail = response;
          if (this.customEventDetail.isParticipationEvent) {
            this.customEventDetail.customEventParticipants.forEach((player) => {
              if (player.participated) {
                this.participatedPlayers++;
              }
            })
          }
        }
      })
    });
  }

}
