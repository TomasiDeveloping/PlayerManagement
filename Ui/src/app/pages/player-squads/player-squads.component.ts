import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {SquadEditModalComponent} from "../../modals/squad-edit-modal/squad-edit-modal.component";
import {SquadModel} from "../../models/squad.model";
import {SquadService} from "../../services/squad.service";
import Swal from "sweetalert2";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-player-squads',
  templateUrl: './player-squads.component.html',
  styleUrl: './player-squads.component.css'
})
export class PlayerSquadsComponent implements OnInit {

  @Input({required: true}) playerId!: string;
  public squads: SquadModel[] = [];
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _squadService: SquadService = inject(SquadService);
  private readonly _toaster: ToastrService = inject(ToastrService);

  get totalPower(): number {
    return this.squads?.reduce((sum, squad) => sum + squad.power, 0) || 0;
  }

  ngOnInit(): void {
    this.getPlayerSquads();
  }

  getPlayerSquads(): void {
    this._squadService.getPlayerSquads(this.playerId).subscribe({
      next: (result) => {
        if (result) {
          this.squads = result;
        }
      },
      error: err => {
        console.log(err);
        this._toaster.error('Error getting player squads', 'Getting player squads');
      }
    })
  }

  editSquad(squad: SquadModel) {
    this.onOpenModal(this.playerId, squad, true);
  }

  deleteSquad(squad: SquadModel) {
    Swal.fire({
      title: "Delete Squad ?",
      text: `Do you really want to delete the ${squad.typeName} squad`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._squadService.deleteSquad(squad.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Squad has been deleted",
                icon: "success"
              }).then(_ => this.getPlayerSquads());
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  addSquad() {
    const squad: SquadModel = {
      id: '',
      playerId: this.playerId,
      squadTypeId: '',
      power: 0,
      position: 0,
      lastUpdateAt: new Date(),
      typeName: ''
    };
    this.onOpenModal(this.playerId, squad, false);
  }

  onOpenModal(playerId: string, squad: SquadModel, isUpdate: boolean) {
    const modalRef = this._modalService.open(SquadEditModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.playerId = playerId;
    modalRef.componentInstance.currentSquad = squad;
    modalRef.componentInstance.isUpdate = isUpdate;
    modalRef.closed.subscribe({
      next: (() => {
        this.getPlayerSquads();
      })
    });
  }

}
