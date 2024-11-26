import {Component, inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PlayerService} from "../../services/player.service";
import {JwtTokenService} from "../../services/jwt-token.service";
import {PlayerModel} from "../../models/player.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {MarshalGuardModalComponent} from "../../modals/marshal-guard-modal/marshal-guard-modal.component";
import {
  CreateMarshalGuardModel,
  MarshalGuardDetailModel,
  MarshalGuardModel,
  UpdateMarshalGuardModel
} from "../../models/marshalGuard.model";
import {MarshalGuardService} from "../../services/marshal-guard.service";
import {MarshalGuardParticipantService} from "../../services/marshal-guard-participant.service";
import {
  CreateMarshalGuardParticipantModel, MarshalGuardParticipantModel
} from "../../models/marshalGuardParticipant.model";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";
import {Router} from "@angular/router";
import {forkJoin, Observable} from "rxjs";

@Component({
  selector: 'app-marshal-guard',
  templateUrl: './marshal-guard.component.html',
  styleUrl: './marshal-guard.component.css'
})
export class MarshalGuardComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _marshalGuardService: MarshalGuardService = inject(MarshalGuardService);
  private readonly _marshalGuardParticipantService: MarshalGuardParticipantService = inject(MarshalGuardParticipantService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);

  public marshalGuardForm!: FormGroup;
  isCreateMarshalGuard: boolean = false
  isUpdate: boolean = false;
  public alliancePlayers: PlayerModel[] = [];
  public playerParticipated: { playerId: string, playerName: string, participated: boolean }[] = [];

  private marshalGuardDetail!: MarshalGuardDetailModel;

  public participantToUpdate: MarshalGuardParticipantModel[] = [];
  public participatedPlayer: number = 0;
  public notParticipatedPlayer: number = 0;
  public playerSelected: boolean = false;
  public marshalGuards: MarshalGuardModel[] = [];

  private allianceId: string = this._tokenService.getAllianceId()!;

  get f() {
    return this.marshalGuardForm.controls;
  }

  ngOnInit() {
    this.getMarshalGuards(10);
  }

  public getMarshalGuards(take: number) {
    this._marshalGuardService.getAllianceMarshalGuards(this.allianceId, take).subscribe({
      next: ((response) => {
        if (response) {
          this.marshalGuards = response;
        } else {
          this.marshalGuards = [];
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load marshalGuards', 'Error loading marshalGuards');
      })
    });
  }

  public getAlliancePlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.alliancePlayers = response;
          this.createMarshalGuardForm(false);
        }
      })
    });
  }

  public createMarshalGuardForm(isUpdate: boolean, marshalGuard: MarshalGuardDetailModel | null = null) {
    if (isUpdate) {
      this.playerSelected = true;
      this.playerParticipated = [...marshalGuard!.marshalGuardParticipants];
      this.participatedPlayer = marshalGuard!.participants;
      this.notParticipatedPlayer = marshalGuard!.allianceSize - marshalGuard!.participants;
    } else {
      this.playerSelected = false;
    }
    const d = isUpdate ? new Date(marshalGuard!.eventDate) : new Date();
    this.marshalGuardForm = new FormGroup({
      id: new FormControl<string>(isUpdate ? marshalGuard!.id : ''),
      allianceId: new FormControl<string>(this.allianceId),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      level: new FormControl<number | null>(isUpdate ? marshalGuard!.level : null, [Validators.required]),
      rewardPhase: new FormControl<number>(isUpdate ? marshalGuard!.rewardPhase : 1),
      allianceSize: new FormControl<number>(isUpdate ? marshalGuard!.allianceSize : this.alliancePlayers.length),
    });
    this.marshalGuardForm.get('allianceSize')?.disable();
    this.isCreateMarshalGuard = true;
  }

  onCreateEvent() {
    this.getAlliancePlayers();
  }

  onCancel() {
    this.isCreateMarshalGuard = false;
  }

  onAddParticipants() {
    const modalRef = this._modalService.open(MarshalGuardModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg', scrollable: true});
    if (this.playerSelected) {
      modalRef.componentInstance.players = [...this.playerParticipated];
    }
    modalRef.componentInstance.allianceId = this.allianceId;
    modalRef.closed.subscribe({
      next: ((response: any) => {
        if (response) {
          this.participatedPlayer = 0;
          this.notParticipatedPlayer = 0;
          this.playerSelected = true;
          response.forEach((player: { playerId: string, playerName: string, participated: boolean }) => {
            if (player.participated) {
              this.participatedPlayer++;
            } else {
              this.notParticipatedPlayer++;
            }
          })
          this.playerParticipated = response;
        }
      })
    })
  }


  onSubmit() {
    if (this.isUpdate) {
      this.updateMarshalGuard();
      return;
    }
    const createMarshalGuard: CreateMarshalGuardModel = {
      allianceId: this.marshalGuardForm.controls['allianceId'].value,
      level: this.marshalGuardForm.controls['level'].value,
      rewardPhase: this.marshalGuardForm.controls['rewardPhase'].value,
      allianceSize: this.marshalGuardForm.controls['allianceSize'].value,
      eventDate: this.marshalGuardForm.controls['eventDate'].value,
    }

    this._marshalGuardService.insertMarshalGuards(createMarshalGuard).subscribe({
      next: ((response) => {
        if (response) {
          if (this.playerParticipated.length > 0) {
            this.insertMarshalGuardParticipants(response.id);
            this._toastr.success('Successfully created marshalGuard', 'Successfully created marshalGuard');
          }
        }
      })
    });
  }

  private insertMarshalGuardParticipants(marshalGuardId: string) {
    const marshalGuardParticipants: CreateMarshalGuardParticipantModel[] = [];

    this.playerParticipated.forEach((player) => {
      const createMarshalGuardParticipant: CreateMarshalGuardParticipantModel = {
        marshalGuardId: marshalGuardId,
        participated: player.participated,
        playerId: player.playerId
      };
      marshalGuardParticipants.push(createMarshalGuardParticipant);
    });

    this._marshalGuardParticipantService.insertMarshalGuardOParticipants(marshalGuardParticipants).subscribe({
      next: (() => {
        this.onCancel();
        this.getMarshalGuards(10);
        this.playerParticipated = [];
      })
    })

  }

  private updateMarshalGuard() {
    let participatedPlayers: number = this.marshalGuardDetail.participants;
    this.marshalGuardDetail.marshalGuardParticipants.forEach((participant) => {
      const playerToUpdate = this.playerParticipated.find(p => p.playerId === participant.playerId);
      if (playerToUpdate) {
        if (playerToUpdate.participated !== participant.participated) {
          if (playerToUpdate.participated) {
            participatedPlayers++;
          } else {
            participatedPlayers--;
          }
          this.participantToUpdate.push({
            playerId: participant.playerId,
            marshalGuardId: participant.marshalGuardId,
            participated: playerToUpdate.participated,
            id: participant.id,
            playerName: participant.playerName
          });
        }
      }
    });
    const updateMarshalGuard: UpdateMarshalGuardModel = {
      id: this.marshalGuardForm.controls['id'].value,
      allianceId: this.marshalGuardForm.controls['allianceId'].value,
      level: this.marshalGuardForm.controls['level'].value,
      rewardPhase: this.marshalGuardForm.controls['rewardPhase'].value,
      eventDate: this.marshalGuardForm.controls['eventDate'].value,
      participants: participatedPlayers
    }

    this._marshalGuardService.updateMarshalGuard(updateMarshalGuard.id, updateMarshalGuard).subscribe({
      next: ((response) => {
          if (response) {
            this.updateMarshalGuardParticipants();
        }
      })
    });
  }

  private updateMarshalGuardParticipants() {
    if (this.participantToUpdate.length <= 0) {
      this.onCancel();
      this.getMarshalGuards(10);
      this.playerParticipated = [];
      this.participantToUpdate = [];
      this._toastr.success('Successfully updated marshalGuard', 'Update marshalGuard')
      return;
    }
    const requests: Observable<MarshalGuardParticipantModel>[] = [];

    this.participantToUpdate.forEach((participant) => {
      const request = this._marshalGuardParticipantService.updateMarshalGuardParticipant(participant.id, participant);
      requests.push(request);
      })
    forkJoin(requests).subscribe({
      next: ((response) => {
        if (response) {
          this.onCancel();
          this.getMarshalGuards(10);
          this.playerParticipated = [];
          this.participantToUpdate = [];
          this._toastr.success('Successfully updated marshalGuard', 'Update marshalGuard');
        }
      })
    })
  }

  onEditMarshalGuard(marshalGuard: MarshalGuardModel) {
    this._marshalGuardService.getMarshalGuardDetail(marshalGuard.id).subscribe({
      next: ((response) => {
        if (response) {
          this.isUpdate = true;
          this.marshalGuardDetail = structuredClone(response);
          this.participantToUpdate = [];
          this.createMarshalGuardForm(true, response);
        }
      })
    });
  }

  onDeleteMarshalGuard(marshalGuard: MarshalGuardModel) {
    Swal.fire({
      title: "Delete Marshal guard ?",
      text: `Do you really want to delete the marshal guard`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._marshalGuardService.deleteMarshalGuard(marshalGuard.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Marshal guards has been deleted",
                icon: "success"
              }).then(_ => this.getMarshalGuards(10));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  onGoToMarshalGuardDetail(marshalGuard: MarshalGuardModel) {
    this._router.navigate(['marshal-guard-detail', marshalGuard.id]).then();
  }
}
