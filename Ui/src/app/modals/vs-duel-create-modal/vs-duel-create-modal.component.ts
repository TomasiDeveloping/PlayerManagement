import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {VsDuelModel} from "../../models/vsDuel.model";
import {VsDuelService} from "../../services/vs-duel.service";
import {ToastrService} from "ngx-toastr";
import {VsDuelLeagueService} from "../../services/vs-duel-league.service";
import {VsDuelLeagueModel} from "../../models/vsDuelLeague.model";

@Component({
  selector: 'app-vs-duel-edit-modal',
  templateUrl: './vs-duel-create-modal.component.html',
  styleUrl: './vs-duel-create-modal.component.css'
})
export class VsDuelCreateModalComponent implements OnInit {

  private readonly _vsDuelService: VsDuelService = inject(VsDuelService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _vsDuelLeagueService: VsDuelLeagueService = inject(VsDuelLeagueService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  public vsDuelForm!: FormGroup;
  public vsDuelLeagues: VsDuelLeagueModel[] = [];

  @Input({required: true}) isUpdate!: boolean;
  @Input({required: true}) vsDuelModel!: VsDuelModel;

  get f() {
    return this.vsDuelForm.controls;
  }

  ngOnInit() {
    this.getVsDuelLeagues();

    const d = new Date(this.vsDuelModel.eventDate);
    this.vsDuelForm = new FormGroup({
      id: new FormControl<string>(this.vsDuelModel.id),
      vsDuelLeagueId: new FormControl<string>(this.vsDuelModel.vsDuelLeagueId, [Validators.required]),
      allianceId: new FormControl<string>(this.vsDuelModel.allianceId),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      won: new FormControl<boolean>(this.vsDuelModel.won),
      isInProgress: new FormControl<boolean>(this.vsDuelModel.isInProgress),
      opponentName: new FormControl<string>(this.vsDuelModel.opponentName, [Validators.required]),
      opponentServer: new FormControl<number>(this.vsDuelModel.opponentServer, [Validators.required]),
      opponentPower: new FormControl<number>(this.vsDuelModel.opponentPower, [Validators.required]),
      opponentSize: new FormControl<number>(this.vsDuelModel.opponentSize, [Validators.required]),
    });
  }

  getVsDuelLeagues() {
    this._vsDuelLeagueService.getVsDuelLeagues().subscribe({
      next: ((response) => {
        if (response) {
          this.vsDuelLeagues = response;
        }
      }),
      error: (error) => {
        console.log(error);
        this._toastr.error('Could not load vsDuelLeagues', 'Load VsDuelLeagues');
      }
    });
  }

  onSubmit() {
    if (this.vsDuelForm.invalid) {
      return;
    }

    const vsDuel: VsDuelModel = this.vsDuelForm.value as VsDuelModel;

    this.isUpdate ? this.updateVsDuel(vsDuel) : this.createVsDuel(vsDuel);
  }

  private createVsDuel(vsDuel: VsDuelModel) {
    this._vsDuelService.createVsDuel(vsDuel).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully created VS-Duel', 'Successfully created');
          this.activeModal.close(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Failed to create VS-Duel', 'Failed to create');
      })
    });
  }

  private updateVsDuel(vsDuel: VsDuelModel) {
    this._vsDuelService.updateVsDuel(vsDuel.id, vsDuel).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated VS-Duel', 'Successfully updated');
          this.activeModal.close(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Failed to update VS-Duel', 'Update failed');
      })
    });
  }

}
