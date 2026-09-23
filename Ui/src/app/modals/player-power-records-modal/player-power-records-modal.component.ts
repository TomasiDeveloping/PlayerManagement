import {ChangeDetectionStrategy, Component, inject, Input, OnInit} from '@angular/core';
import {PlayerCombatRecordModel} from "../../models/playerCombatRecord.model";
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerCombatService} from "../../services/player-combat.service";
import {PowerTrackingEditDialogComponent} from "../power-tracking-edit-dialog/power-tracking-edit-dialog.component";
import Swal from "sweetalert2";

@Component({
  selector: 'app-player-power-records-modal',
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './player-power-records-modal.component.css',
  templateUrl: './player-power-records-modal.component.html',
})
export class PlayerPowerRecordsModalComponent implements OnInit {

  @Input() playerId!: string;
  @Input() playerName!: string;

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private modalService: NgbModal = inject(NgbModal);
  private combatService: PlayerCombatService = inject(PlayerCombatService);

  records: PlayerCombatRecordModel[] = [];
  loading: boolean = false;
  hasChanges: boolean = false;

  ngOnInit() {
    this.loadRecords();
  }

  loadRecords(): void {
    this.loading = true;
    this.combatService.getPlayerRecords(this.playerId).subscribe({
      next: data => {
        this.records = data;
        this.loading = false;
      },
      error: err => {
        console.log(err);
        this.loading = false;
      }
    });
  }

  editRecord(record: PlayerCombatRecordModel) {
    const modalRef = this.modalService.open(PowerTrackingEditDialogComponent, { size: 'md', centered: true });
    modalRef.componentInstance.isUpdate = true;
    modalRef.componentInstance.editData = record;

    modalRef.result.then((result) => {
      if (result === true) {
        this.hasChanges = true;
        this.loadRecords();
      }
    }).catch(() => {});
  }

  deleteRecord(recordId: string) {
    Swal.fire({
      title: "Delete Record ?",
      text: `Do you really want to delete the record ?`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.combatService.deleteRecord(recordId).subscribe({
          next: ((response) => {
            if (response) {
              this.hasChanges = true;
              Swal.fire({
                title: "Deleted!",
                text: "Record has been deleted",
                icon: "success"
              }).then(_ => this.loadRecords());
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

}
