import {Component, inject, OnInit} from '@angular/core';
import {VsDuelModel} from "../../models/vsDuel.model";
import {JwtTokenService} from "../../services/jwt-token.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {VsDuelCreateModalComponent} from "../../modals/vs-duel-create-modal/vs-duel-create-modal.component";
import {VsDuelService} from "../../services/vs-duel.service";
import {WeekPipe} from "../../helpers/week.pipe";
import Swal from "sweetalert2";
import {Router} from "@angular/router";

@Component({
  selector: 'app-vs-duel',
  templateUrl: './vs-duel.component.html',
  styleUrl: './vs-duel.component.css',
  providers: [WeekPipe]
})
export class VsDuelComponent implements OnInit {

  private readonly _weekPipe: WeekPipe = new WeekPipe();
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _modalService : NgbModal = inject(NgbModal);
  private readonly _vsDuelService: VsDuelService = inject(VsDuelService);
  public readonly _router: Router = inject(Router);

  public currentDate: Date = new Date();
  public vsDuels: VsDuelModel[] = [];
  public page: number = 1;
  public currentWeekDuelExists: boolean = false;

  ngOnInit() {
    this.getVsDuels(this._tokenService.getAllianceId()!, 10);
  }

  onCreateEvent() {
    const vsDuel: VsDuelModel = {
      eventDate: new Date(),
      opponentName: '',
      allianceId: this._tokenService.getAllianceId()!,
      id: '',
      won: false,
      opponentPower: 0,
      opponentServer: 0,
      opponentSize: 0,
      createdBy: ''
    };
    this.openVsDuelEditModal(vsDuel, false);
  }

  openVsDuelEditModal(vsDuelModel: VsDuelModel, isUpdate: boolean) {
    const modalRef = this._modalService.open(VsDuelCreateModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.vsDuelModel = vsDuelModel;
    modalRef.componentInstance.isUpdate = isUpdate;
    modalRef.closed.subscribe({
      next: ((response: VsDuelModel) => {
        if (response) {
          this.getVsDuels(response.allianceId, 10);
        }
      })
    })
  }

  onGoToVsDuelInformation(vsDuel: VsDuelModel) {
    this._router.navigate(['vs-duel-detail', vsDuel.id]).then();
  }

  onEditVsDuel(vsDuel: VsDuelModel) {
    this._router.navigate(['vs-duel-edit', vsDuel.id]).then();
  }

  onDeleteVsDuel(vsDuel: VsDuelModel) {
    Swal.fire({
      title: "Delete VS-Duel ?",
      text: `Do you really want to delete the VS-Duel`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._vsDuelService.deleteVsDuel(vsDuel.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "VS-Duel has been deleted",
                icon: "success"
              }).then(_ => this.getVsDuels(vsDuel.allianceId, 10));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  private getVsDuels(allianceId: string, take: number) {
    this.vsDuels = []
    this.currentWeekDuelExists = false;
    this._vsDuelService.getAllianceVsDuels(allianceId, take).subscribe({
      next: ((response) => {
        if (response) {
          response.forEach((vsDuel: VsDuelModel) => {
            if (this._weekPipe.transform(vsDuel.eventDate) === this._weekPipe.transform(new Date())) {
              this.currentWeekDuelExists = true;
            }
          })
          this.vsDuels = response;
        }
      })
    });
  }
}
