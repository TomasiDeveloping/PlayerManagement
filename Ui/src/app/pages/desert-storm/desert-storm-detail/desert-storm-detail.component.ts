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
  filterType: 'ALL' | 'PARTICIPATED' | 'START_NOT_PARTICIPATED' | 'SUB_NOT_PARTICIPATED' = 'ALL';

  get filteredPlayers() {
    if (this.desertStormDetail?.desertStormParticipants) {
      return this.desertStormDetail.desertStormParticipants.filter(player => {
        if (!player.registered) return false;

        switch (this.filterType) {
          case 'PARTICIPATED':
            return player.participated;

          case 'START_NOT_PARTICIPATED':
            return player.startPlayer && !player.participated;

          case 'SUB_NOT_PARTICIPATED':
            return !player.startPlayer && !player.participated;

          default:
            return true;
        }
      });
    }
    return [];

  }


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
