import {Component, inject, OnInit} from '@angular/core';
import {CreateDesertStormModel, DesertStormDetailModel, DesertStormModel} from "../../models/desertStorm.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {JwtTokenService} from "../../services/jwt-token.service";
import {DesertStormService} from "../../services/desert-storm.service";
import {WeekPipe} from "../../helpers/week.pipe";
import {Router} from "@angular/router";
import Swal from "sweetalert2";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {
  DesertStormParticipantsModalComponent
} from "../../modals/desert-storm-participants-modal/desert-storm-participants-modal.component";
import {DesertStormParticipantService} from "../../services/desert-storm-participant.service";
import {
  CreateDesertStormParticipantModel,
  DesertStormParticipantModel
} from "../../models/desertStormParticipant.model";
import {ToastrService} from "ngx-toastr";
import {forkJoin, Observable} from "rxjs";

@Component({
  selector: 'app-desert-storm',
  templateUrl: './desert-storm.component.html',
  styleUrl: './desert-storm.component.css'
})
export class DesertStormComponent implements OnInit {

  private readonly _weekPipe: WeekPipe = new WeekPipe();
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _desertStormService: DesertStormService = inject(DesertStormService);
  private readonly _desertStormParticipantService: DesertStormParticipantService = inject(DesertStormParticipantService);
  private readonly _router: Router = inject(Router);
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _toastr: ToastrService = inject(ToastrService);

  isCreateDessertStorm: boolean = false;
  public desertStorms: DesertStormModel[] = [];
  currentDate: Date = new Date();
  currentWeekDuelExists: boolean = false;
  desertStormPlayers: {playerId: string, playerName: string, participated: boolean, registered: boolean, startPlayer: boolean}[] = [];
  selectedPlayers: number = 0;
  desertStormDetailModel: DesertStormDetailModel | undefined;

  desertStormForm!: FormGroup;
  isUpdate: boolean = false;

  get f() {
    return this.desertStormForm.controls;
  }

  ngOnInit() {
    this.getDesertStorms(10);
  }

  getDesertStorms(take: number) {
    this._desertStormService.getAllianceDesertStorms(this._tokenService.getAllianceId()!, take).subscribe({
      next: (response) => {
          if (response) {
            response.forEach((desertStorm: DesertStormModel) => {
              if (this._weekPipe.transform(desertStorm.eventDate) === this._weekPipe.transform(new Date())) {
                this.currentWeekDuelExists = true;
              }
            })
            this.desertStorms = response;
        } else {
          this.desertStorms = [];
          this.currentWeekDuelExists = false;
        }
    }});
  }

  onCreateEvent() {
    this.createDesertStormForm();
    this.isCreateDessertStorm = true;
  }

  createDesertStormForm(desertStormModel: DesertStormModel | null = null): void {
    const d = desertStormModel ? new Date(desertStormModel.eventDate) : new Date();
    this.desertStormForm = new FormGroup({
      id: new FormControl<string>(desertStormModel ? desertStormModel.id : ''),
      allianceId: new FormControl<string>(desertStormModel ? desertStormModel.allianceId : this._tokenService.getAllianceId()!, [Validators.required]),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      won: new FormControl<boolean>(desertStormModel ? desertStormModel.won : false),
      opponentName: new FormControl<string>(desertStormModel ? desertStormModel.opponentName : ''),
      opponentServer: new FormControl<number | null>(desertStormModel ? desertStormModel.opponentServer : null),
      OpposingParticipants: new FormControl<number | null>(desertStormModel ? desertStormModel.opposingParticipants : null),
    })
  }

  onSubmit() {
    if (this.desertStormForm.invalid) {
      return;
    }

    if (this.isUpdate) {
      this.updateDesertStorm();
      return;
    }

    const createEvent: CreateDesertStormModel = this.desertStormForm.value as CreateDesertStormModel;
    this._desertStormService.createDesertStorm(createEvent).subscribe({
      next: (response) => {
        if (response) {
          this.insertDesertStormParticipants(response.id);
        }
      }
    });
  }

  onDesertStormDetail(desertStorm: DesertStormModel) {
    this._router.navigate(['desert-storm-detail', desertStorm.id]).then();
  }

  onDeleteDesertStorm(desertStorm: DesertStormModel) {
    Swal.fire({
      title: "Delete Desert storm ?",
      text: `Do you really want to delete the Desert storm`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._desertStormService.deleteDesertStorm(desertStorm.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Desert storm has been deleted",
                icon: "success"
              }).then(_ => this.getDesertStorms(10));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onAddParticipants() {
    const modalRef = this._modalService.open(DesertStormParticipantsModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg', scrollable: true});
    if (this.desertStormPlayers.length > 0) {
      modalRef.componentInstance.players = [...this.desertStormPlayers];
    }
    modalRef.componentInstance.allianceId = this._tokenService.getAllianceId();
    modalRef.closed.subscribe({
      next: ((response: {playerId: string, playerName: string, participated: boolean, registered: boolean, startPlayer: boolean}[]) => {
        if (response) {
          this.selectedPlayers = 0;
          this.desertStormPlayers = response;
          response.forEach((d => {
            if (d.registered) {
              this.selectedPlayers++;
            }
          }))
        }
      })
    })
  }

  onCancel() {
    this.isCreateDessertStorm = false;
    this.selectedPlayers = 0;
    this.desertStormPlayers = [];
  }

  private insertDesertStormParticipants(desertStormId: string) {
    const desertStormParticipants: CreateDesertStormParticipantModel[] = [];

    this.desertStormPlayers.forEach(player => {
      const desertStormParticipant: CreateDesertStormParticipantModel = {
        desertStormId: desertStormId,
        participated: player.participated,
        playerId: player.playerId,
        startPlayer: player.startPlayer,
        registered: player.registered,
      }
      desertStormParticipants.push(desertStormParticipant);
    });

    this._desertStormParticipantService.insertDesertStormOParticipants(desertStormParticipants).subscribe({
      next: (() => {
        this._toastr.success('Successfully created!', 'Successfully');
        this.onCancel();
        this.getDesertStorms(10);
      })
    })
  }

  onEditDesertStorm(desertStorm: DesertStormModel) {
    this._desertStormService.getDesertStormDetail(desertStorm.id).subscribe({
      next: (response) => {
        if (response) {
          this.desertStormPlayers = structuredClone(response.desertStormParticipants);
          this.createDesertStormForm(response);
          response.desertStormParticipants.forEach((d) => {
            if (d.registered) {
              this.selectedPlayers++;
            }
          })
          this.isCreateDessertStorm = true;
          this.isUpdate = true;
          this.desertStormDetailModel = response;
        }
      }
    })
  }

  updateDesertStorm() {
    const toUpdate: any[] = [];
    const desertStorm: DesertStormModel = this.desertStormForm.value as DesertStormModel;
    this.desertStormPlayers.forEach(p => {
      const player = this.desertStormDetailModel!.desertStormParticipants.find(d => d.playerId === p.playerId)!;
      if (player.participated !== p.participated || player.registered !== p.registered || player.startPlayer !== p.startPlayer) {
        toUpdate.push(p);
      }
    });
    this._desertStormService.updateDesertStorm(desertStorm.id, desertStorm).subscribe({
      next: ((response) => {
        if (response) {
          this.updateDesertStormParticipants(toUpdate);
        }
      })
    });
  }

  private updateDesertStormParticipants(desertStormParticipants: DesertStormParticipantModel[]) {
    if (desertStormParticipants.length <= 0) {
      this._toastr.success('Successfully updated!', 'Successfully');
      this.onCancel();
      this.getDesertStorms(10);
      return;
    }
    const requests: Observable<DesertStormParticipantModel>[] = [];

    desertStormParticipants.forEach((participant) => {
      const request = this._desertStormParticipantService.updateDesertStormParticipant(participant.id, participant);
      requests.push(request);
    })
    forkJoin(requests).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated!', 'Successfully');
          this.onCancel();
          this.getDesertStorms(10);
        }
      })
    })
  }
}
