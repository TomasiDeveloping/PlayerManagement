import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {DesertStormService} from "../../../services/desert-storm.service";
import {DesertStormDetailModel} from "../../../models/desertStorm.model";

@Component({
  selector: 'app-desert-storm-detail',
  templateUrl: './desert-storm-detail.component.html',
  styleUrl: './desert-storm-detail.component.css'
})
export class DesertStormDetailComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _desertStormService: DesertStormService = inject(DesertStormService);

  public desertStormId!: string;
  public desertStormDetail: DesertStormDetailModel | undefined;
  public registeredPlayers: number = 0;
  public participatedPlayers: number = 0;
  public startedPlayers: number = 0;

  ngOnInit() {

    this.desertStormId = this._activatedRote.snapshot.params['id'];

    this.getDesertStormDetail(this.desertStormId);
  }

  getDesertStormDetail(desertStormId: string) {
    this._desertStormService.getDesertStormDetail(desertStormId).subscribe({
      next: (desertStormDetail: DesertStormDetailModel) => {
        if (desertStormDetail) {
          this.desertStormDetail = desertStormDetail;
          desertStormDetail.desertStormParticipants.forEach((d) => {
            if (d.registered) {
              this.registeredPlayers++;
            }
            if (d.participated) {
              this.participatedPlayers++;
            }
            if (d.startPlayer) {
              this.startedPlayers++;
            }
          })
        }
      }
    });
  }
}
