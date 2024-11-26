import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MarshalGuardService} from "../../../services/marshal-guard.service";
import {MarshalGuardDetailModel} from "../../../models/marshalGuard.model";

@Component({
  selector: 'app-marshal-guard-detail',
  templateUrl: './marshal-guard-detail.component.html',
  styleUrl: './marshal-guard-detail.component.css'
})
export class MarshalGuardDetailComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _marshalGuardService: MarshalGuardService = inject(MarshalGuardService);

  public marshalGuardId!: string;
  public marshalGuardDetail: MarshalGuardDetailModel | undefined;


  ngOnInit() {
    this.marshalGuardId = this._activatedRote.snapshot.params['id'];

    this.getMarshalGuardDetail(this.marshalGuardId);
  }

  getMarshalGuardDetail(marshalGuardId: string) {
    this._marshalGuardService.getMarshalGuardDetail(marshalGuardId).subscribe({
      next: ((response) => {
        if (response) {
          this.marshalGuardDetail = response;
        }
      })
    });
  }
}
