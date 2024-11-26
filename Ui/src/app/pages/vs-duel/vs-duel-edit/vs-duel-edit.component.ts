import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {VsDuelService} from "../../../services/vs-duel.service";
import {VsDuelDetailModel, VsDuelModel} from "../../../models/vsDuel.model";
import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {VsDuelParticipantService} from "../../../services/vs-duel-participant.service";
import {VsDuelParticipantModel} from "../../../models/vsDuelParticipant.model";
import {forkJoin, Observable} from "rxjs";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-vs-duel-edit',
  templateUrl: './vs-duel-edit.component.html',
  styleUrl: './vs-duel-edit.component.css'
})
export class VsDuelEditComponent implements OnInit {

  private readonly _activatedRote: ActivatedRoute = inject(ActivatedRoute);
  private readonly _vsDuelService: VsDuelService = inject(VsDuelService);
  private readonly _vsDuelParticipantService: VsDuelParticipantService = inject(VsDuelParticipantService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  private vsDuelId!: string;
  private vsDuelDetail!: VsDuelDetailModel;

  public vsDuelParticipantsForm: FormGroup = new FormGroup({});
  public vsDuelForm: FormGroup = new FormGroup({});


  get vsDuelParticipants(): FormArray {
    return this.vsDuelParticipantsForm.get('vsDuelParticipants') as FormArray;
  }

  get df() {
    return this.vsDuelForm.controls;
  }

  ngOnInit() {
    this.vsDuelId = this._activatedRote.snapshot.params['id'];
    this.getVsDuelDetail(this.vsDuelId);
  }

  getVsDuelDetail(vsDuelId: string) {
    this._vsDuelService.getVsDuelDetail(vsDuelId).subscribe({
      next: ((response: VsDuelDetailModel) => {
        this.vsDuelDetail = response;
        this.vsDuelDetail.vsDuelParticipants = this.vsDuelDetail.vsDuelParticipants.sort((a, b) => a.playerName.localeCompare(b.playerName));
          this.createForm(response);
      })
    });
  }

  createForm(vsDuelDetail: VsDuelDetailModel) {
    const d = new Date(vsDuelDetail.eventDate);
    this.vsDuelForm = new FormGroup({
      id: new FormControl<string>(vsDuelDetail.id),
      allianceId: new FormControl<string>(vsDuelDetail.allianceId),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      won: new FormControl<boolean>(vsDuelDetail.won),
      opponentName: new FormControl<string>(vsDuelDetail.opponentName, [Validators.required, Validators.maxLength(150)]),
      opponentServer: new FormControl<number>(vsDuelDetail.opponentServer, [Validators.required]),
      opponentPower: new FormControl<number>(vsDuelDetail.opponentPower, [Validators.required]),
      opponentSize: new FormControl<number>(vsDuelDetail.opponentSize, [Validators.required]),
    });

    this.vsDuelParticipantsForm = new FormGroup({
      vsDuelParticipants: new FormArray([])
    });

    vsDuelDetail.vsDuelParticipants.forEach((vsDuelParticipant) => {
      this.vsDuelParticipants.push(new FormGroup({
        id: new FormControl<string>(vsDuelParticipant.id),
        playerId: new FormControl<string>(vsDuelParticipant.playerId),
        vsDuelId: new FormControl<string>(vsDuelParticipant.vsDuelId),
        weeklyPoints: new FormControl<number>(vsDuelParticipant.weeklyPoints, [Validators.required, Validators.pattern('(0|[1-9]\\d*)')]),
        playerName: new FormControl<string>(vsDuelParticipant.playerName),
      }));
    })
  }

  onUpdatePlayers() {
    const requests: Observable<VsDuelParticipantModel>[] = [];

    this.vsDuelParticipants.controls.forEach((control) => {
      if (control.dirty) {
        const vsDuelParticipant: VsDuelParticipantModel = control.value as VsDuelParticipantModel;
        const request = this._vsDuelParticipantService.updateVsDuelParticipant(vsDuelParticipant.id, vsDuelParticipant);
        requests.push(request);
      }
    })
    if (requests.length > 0) {
      forkJoin(requests).subscribe({
        next: ((response) => {
          if (response.length > 0) {
            this._toastr.success(`Successfully updated ${response.length} players`);
            this.getVsDuelDetail(this.vsDuelId);
          }
        }),
        error: ((error) => {
          console.log(error);
          this._toastr.error('Unable to update players', 'Update');
        })
      })
    }
  }

  onUpdateEvent() {
    if (this.vsDuelForm.invalid) {
      return;
    }

    const vsDuel: VsDuelModel = this.vsDuelForm.value as VsDuelModel;
    this._vsDuelService.updateVsDuel(vsDuel.id, vsDuel).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated event', 'Update');
          this.getVsDuelDetail(response.id);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Unable to update event', 'Update');
      })
    });
  }
}
