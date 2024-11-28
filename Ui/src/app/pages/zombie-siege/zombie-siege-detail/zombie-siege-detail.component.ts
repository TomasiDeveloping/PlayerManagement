import {Component, inject, OnInit} from '@angular/core';
import {ZombieSiegeDetailModel} from "../../../models/zombieSiege.model";
import {ActivatedRoute} from "@angular/router";
import {ZombieSiegeService} from "../../../services/zombie-siege.service";

@Component({
  selector: 'app-zombie-siege-detail',
  templateUrl: './zombie-siege-detail.component.html',
  styleUrl: './zombie-siege-detail.component.css'
})
export class ZombieSiegeDetailComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _zombieSiegeService: ZombieSiegeService = inject(ZombieSiegeService);

  private zombieSiegeId!: string;

  public zombieSiegeDetail: ZombieSiegeDetailModel | undefined;

  ngOnInit() {
    this.zombieSiegeId = this._activatedRote.snapshot.params['id'];
    this.getZombieSiegeDetail(this.zombieSiegeId);
  }

  getZombieSiegeDetail(zombieSiegeId: string) {
    this._zombieSiegeService.getZombieSiegeDetail(zombieSiegeId).subscribe({
      next: ((response: ZombieSiegeDetailModel) => {
        if (response) {
          this.zombieSiegeDetail = response;
          this.zombieSiegeDetail.zombieSiegeParticipants.sort((a, b) => b.survivedWaves - a.survivedWaves);
        }
      })
    });
  }

}
