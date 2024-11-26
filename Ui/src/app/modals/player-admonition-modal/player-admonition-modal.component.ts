import {Component, inject, Input, OnInit} from '@angular/core';
import {PlayerModel} from "../../models/player.model";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {AdmonitionModel} from "../../models/admonition.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import {AdmonitionService} from "../../services/admonition.service";
import Swal from "sweetalert2";

@Component({
  selector: 'app-player-admonition-modal',
  templateUrl: './player-admonition-modal.component.html',
  styleUrl: './player-admonition-modal.component.css'
})
export class PlayerAdmonitionModalComponent implements OnInit {

  private readonly _admonitionService: AdmonitionService = inject(AdmonitionService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  public isCreateMode: boolean = false;
  public isEditMode: boolean = false;
  public playerAdmonitions: AdmonitionModel[] = [];
  public   admonitionForm: FormGroup = new FormGroup({
    id: new FormControl<string>(''),
    playerId: new FormControl<string>(''),
    reason: new FormControl<string>('', [Validators.required, Validators.maxLength(250)]),
  });



  @Input({required: true}) player!: PlayerModel;

  ngOnInit() {
    this.getPlayerAdmonitions(this.player.id);
  }

  getPlayerAdmonitions(playerId: string) {
    this._admonitionService.getPlayerAdmonitions(playerId).subscribe({
      next: ((response) => {
        if (response) {
          this.playerAdmonitions = response;
        } else {
          this.playerAdmonitions = [];
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load admonitions for player', 'Error loading admonitions');
      })
    });
  }


  onInsertNewAdmonition() {
    this.isCreateMode = true;
    this.admonitionForm.patchValue({
      reason: '',
      playerId: this.player.id,
    });
  }

  onEditAdmonition(admonition: AdmonitionModel) {
    this.isEditMode = true;
    this.admonitionForm.patchValue({
      id: admonition.id,
      reason: admonition.reason,
      playerId: admonition.playerId
    });
  }

  onDeleteAdmonition(admonition: AdmonitionModel) {
    Swal.fire({
      title: "Delete Admonition ?",
      text: 'Do you really want to delete this admonition ?',
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._admonitionService.deleteAdmonition(admonition.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Admonition has been deleted",
                icon: "success"
              }).then(_ => {
                this.getPlayerAdmonitions(admonition.playerId);
              });
            }
          }),
          error: (error: Error) => {
            console.log(error);
            this._toastr.error('Could not delete note', 'Error delete note');
          }
        });
      }
    });
  }

  onSubmit() {
    if (this.admonitionForm.invalid) {
      return;
    }
    const admonition: AdmonitionModel = this.admonitionForm.value as AdmonitionModel;

    if (this.isCreateMode) {
      this.creatNewAdmonition(admonition);
    }
    if (this.isEditMode) {
      this.updateAdmonition(admonition);
    }
  }

  onCancel() {
    this.isCreateMode = false;
    this.isEditMode = false;
    this.admonitionForm.reset();
  }

  private creatNewAdmonition(admonition: AdmonitionModel) {
    this._admonitionService.createAdmonition(admonition).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully created admonition', 'Created Admonition');
          this.getPlayerAdmonitions(admonition.playerId);
          this.onCancel();
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not create new Admonition', 'Error create admonition');
      })
    });
  }

  private updateAdmonition(admonition: AdmonitionModel) {
    this._admonitionService.updateAdmonition(admonition.id, admonition).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated admonition', 'Update admonition');
          this.getPlayerAdmonitions(response.playerId);
          this.onCancel();
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not update Admonition', 'Error update admonition');
      })
    });
  }
}
