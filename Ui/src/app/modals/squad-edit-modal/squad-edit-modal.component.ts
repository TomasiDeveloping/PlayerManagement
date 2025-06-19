import {Component, inject, Input, OnInit} from '@angular/core';
import {SquadTypeService} from "../../services/squad-type.service";
import {SquadService} from "../../services/squad.service";
import {SquadTypeModel} from "../../models/squadType.model";
import {CreateSquadModel, SquadModel, UpdateSquadModel} from "../../models/squad.model";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-squad-edit-modal',
  templateUrl: './squad-edit-modal.component.html',
  styleUrl: './squad-edit-modal.component.css'
})
export class SquadEditModalComponent implements OnInit {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  @Input({required: true}) playerId!: string;
  @Input({required: true}) isUpdate!: boolean;
  @Input({required: true}) currentSquad!: SquadModel;
  public squadTypes: SquadTypeModel[] = [];
  public positions: number[] = [1, 2, 3, 4];
  public squadForm!: FormGroup;
  private readonly _squadTypeService: SquadTypeService = inject(SquadTypeService);
  private readonly _squadService: SquadService = inject(SquadService);
  private readonly _toaster: ToastrService = inject(ToastrService);

  get f() {
    return this.squadForm.controls;
  }

  ngOnInit() {
    this.getSquadTypes();
    this.squadForm = new FormGroup({
      id: new FormControl<string>(this.currentSquad.id),
      playerId: new FormControl<string>(this.currentSquad.playerId),
      power: new FormControl<number | null>(this.isUpdate ? this.currentSquad.power : null, [Validators.required]),
      position: new FormControl<number | null>(this.isUpdate ? this.currentSquad.position : null, [Validators.required]),
      squadTypeId: new FormControl<string>(this.currentSquad.squadTypeId, [Validators.required]),
    })
  }

  getSquadTypes() {
    this._squadTypeService.getSquadTypes().subscribe({
      next: (response: SquadTypeModel[]) => {
        this.squadTypes = response;
      },
      error: (error: Error) => {
        console.error(error);
        this._toaster.error('Could not get squad types', 'Get squad types');
      }
    });
  }


  onSubmit() {
    if (this.squadForm.invalid) {
      return;
    }
    if (this.isUpdate) {
      const squad: UpdateSquadModel = this.squadForm.value as UpdateSquadModel;
      this.updateSquad(squad);
    } else {
      const squad: CreateSquadModel = this.squadForm.value as CreateSquadModel;
      this.addSquad(squad);
    }
  }

  private updateSquad(squad: UpdateSquadModel) {
    this._squadService.updateSquad(squad.id, squad).subscribe({
      next: (_: SquadModel) => {
        this._toaster.success('Squad updated successfully', 'Squad update');
        this.activeModal.close();
      },
      error: (error: Error) => {
        console.error(error);
        this._toaster.error('Could not update squad types', 'Squad update');
      }
    });
  }

  private addSquad(squad: CreateSquadModel) {
    this._squadService.createSquad(squad).subscribe({
      next: (_: SquadModel) => {
        this._toaster.success('Squad created successfully', 'Squad create');
        this.activeModal.close();
      },
      error: (error: Error) => {
        console.error(error);
        this._toaster.error('Could not create squad', 'Squad create');
      }
    })
  }
}
