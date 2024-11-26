import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {VsDuelService} from "../../../services/vs-duel.service";
import {VsDuelDetailModel} from "../../../models/vsDuel.model";


@Component({
  selector: 'app-vs-duel-detail',
  templateUrl: './vs-duel-detail.component.html',
  styleUrl: './vs-duel-detail.component.css'
})
export class VsDuelDetailComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _vsDuelService: VsDuelService = inject(VsDuelService);

  public vsDuelId!: string;
  public vsDuelDetail: VsDuelDetailModel | undefined;

  ngOnInit() {
    this.vsDuelId = this._activatedRote.snapshot.params['id'];

    this.getVsDuelDetail(this.vsDuelId);
  }

  getVsDuelDetail(vsDuelId: string) {
    this._vsDuelService.getVsDuelDetail(vsDuelId).subscribe({
      next: ((response: VsDuelDetailModel) => {
        this.vsDuelDetail = response;
        this.vsDuelDetail.vsDuelParticipants = this.vsDuelDetail.vsDuelParticipants.sort((a, b) => b.weeklyPoints - a.weeklyPoints);
      })
    });
  }
}
