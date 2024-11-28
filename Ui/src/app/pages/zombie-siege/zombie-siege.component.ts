import {Component, inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CreateZombieSiegeModel, ZombieSiegeDetailModel, ZombieSiegeModel} from "../../models/zombieSiege.model";
import {JwtTokenService} from "../../services/jwt-token.service";
import {PlayerService} from "../../services/player.service";
import {PlayerModel} from "../../models/player.model";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {
  ZombieSiegeParticipantsModalComponent
} from "../../modals/zombie-siege-participants-modal/zombie-siege-participants-modal.component";
import {ZombieSiegeService} from "../../services/zombie-siege.service";
import {ToastrService} from "ngx-toastr";
import {
  CreateZombieSiegeParticipantModel,
  ZombieSiegeParticipantModel
} from "../../models/zombieSiegeParticipant.model";
import {ZombieSiegeParticipantService} from "../../services/zombie-siege-participant.service";
import Swal from "sweetalert2";
import {Router} from "@angular/router";
import {forkJoin, Observable} from "rxjs";

@Component({
  selector: 'app-zombie-siege',
  templateUrl: './zombie-siege.component.html',
  styleUrl: './zombie-siege.component.css'
})
export class ZombieSiegeComponent implements OnInit {

  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _zombieSiegeService: ZombieSiegeService = inject(ZombieSiegeService);
  private readonly _zombieSiegeParticipantService: ZombieSiegeParticipantService = inject(ZombieSiegeParticipantService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _router: Router = inject(Router);

  private allianceId: string = this._tokenService.getAllianceId()!;

  public isCreateZombieSiege: boolean = false;
  public playerSelected: boolean = false;
  public zombieSiegeForm!: FormGroup;
  public playerParticipated: { playerId: string, playerName: string, survivedWaves: number; }[] = [];
  public isUpdate: boolean = false;
  public zombieSieges: ZombieSiegeModel[] = [];
  public alliancePlayers: PlayerModel[] = [];
  public zombieSiegeDetailModel: ZombieSiegeDetailModel | undefined;
  private originalZombieSiegeParticipants: ZombieSiegeParticipantModel[] = [];

  get f() {
    return this.zombieSiegeForm.controls;
  }

  ngOnInit() {
    this.getZombieSieges(10);
  }

  getZombieSieges(limit: number) {
    this._zombieSiegeService.getAllianceZombieSieges(this.allianceId, limit).subscribe({
      next: ((response: any) => {
        if (response) {
          this.zombieSieges = response;
        } else {
          this.zombieSieges = [];
        }
      })
      ,error: error => {
        console.log(error);
        this._toastr.error('Could not load zombie Sieges', 'Load');
      }
    })
  }

  onCreateEvent() {
    this.getPlayers();
  }

  onAddParticipants() {
    const modalRef = this._modalService.open(ZombieSiegeParticipantsModalComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg', scrollable: true});
    if (this.playerSelected) {
      this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
      modalRef.componentInstance.players = [...this.playerParticipated];
    }
    modalRef.componentInstance.allianceId = this.allianceId;
    modalRef.closed.subscribe({
      next: ((response: any) => {
        if (response) {
          this.playerSelected = true;
          this.playerParticipated = [...response];
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
      })
    })
  }

  onCancel() {
    this.isCreateZombieSiege = false;
    this.isUpdate = false;
    this.playerParticipated = [];
  }

  onSubmit() {
    if (this.isUpdate) {
      this.updateZombieSiege();
      return;
    }
    const createZombieSiege: CreateZombieSiegeModel = this.zombieSiegeForm.getRawValue() as CreateZombieSiegeModel;

    this._zombieSiegeService.createZombieSiege(createZombieSiege).subscribe({
      next: ((response) => {
        if (response) {
          this.insertZombieSiegeParticipants(response.id);
          this._toastr.success('Successfully created Zombie Sieges', 'Create Zombie Siege');
        }
      }),
      error: error => {
        console.log(error);
        this._toastr.error('Could not create zombie Siege', 'Create Zombie Siege');
      }
    });
  }

  onGoToZombieSiegeDetail(zombieSiege: ZombieSiegeModel) {
    this._router.navigate(['zombie-siege-detail', zombieSiege.id]).then();
  }

  onEditZombieSiege(zombieSiege: ZombieSiegeModel) {
    this._zombieSiegeService.getZombieSiegeDetail(zombieSiege.id).subscribe({
      next: ((response) => {
        if (response) {
          this.zombieSiegeDetailModel = response;
          const originalParticipants = structuredClone(response.zombieSiegeParticipants);

          this.playerParticipated = structuredClone(response.zombieSiegeParticipants);
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
          this.playerSelected = true;
          this.isUpdate = true;
          this.createZombieSiegeForm(true, response);

          this.originalZombieSiegeParticipants = originalParticipants;

        }
      })
    });
  }

  onDeleteZombieSiege(zombieSiege: ZombieSiegeModel) {
    Swal.fire({
      title: "Delete Zombie Siege ?",
      text: `Do you really want to delete the zombie siege?`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._zombieSiegeService.deleteZombieSiege(zombieSiege.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Zombie Siege has been deleted",
                icon: "success"
              }).then(_ => this.getZombieSieges(10));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  private getPlayers() {
    this._playerService.getAlliancePlayer(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.alliancePlayers = response;
          this.createZombieSiegeForm(false);
        }
      })
    });
  }

  private createZombieSiegeForm(isUpdate: boolean, zombieSiegeModel: ZombieSiegeDetailModel | null = null) {
    if (isUpdate) {
      this.playerSelected = true;
      this.playerParticipated = [...zombieSiegeModel!.zombieSiegeParticipants];
    } else {
      this.playerSelected = false;
    }
    const d = zombieSiegeModel ? new Date(zombieSiegeModel!.eventDate) : new Date();
    this.zombieSiegeForm = new FormGroup({
      id: new FormControl<string>(zombieSiegeModel ? zombieSiegeModel!.id : ''),
      allianceId: new FormControl<string>(this.allianceId),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      level: new FormControl<number | null>(zombieSiegeModel ? zombieSiegeModel!.level : null, [Validators.required]),
      allianceSize: new FormControl<number>(zombieSiegeModel ? zombieSiegeModel!.allianceSize : this.alliancePlayers.length),
    });
    this.zombieSiegeForm.get('allianceSize')?.disable();
    this.isCreateZombieSiege = true;
  }

  private insertZombieSiegeParticipants(zombieSiegeId: string) {
    const createZombieSiegeParticipants: CreateZombieSiegeParticipantModel[] = [];

    this.playerParticipated.forEach((player) => {
      const createZombieSiegeParticipant: CreateZombieSiegeParticipantModel = {
        zombieSiegeId: zombieSiegeId,
        playerId: player.playerId,
        survivedWaves: player.survivedWaves,
      };
      createZombieSiegeParticipants.push(createZombieSiegeParticipant);
    });

    this._zombieSiegeParticipantService.insertZombieSiegeParticipants(createZombieSiegeParticipants).subscribe({
      next: (() => {
        this.onCancel();
        this.getZombieSieges(10);
        this.playerParticipated = [];
      })
    });
  }

  private updateZombieSiege() {
    const toUpdate: any[] = [];
    const updateZombieSiege: ZombieSiegeModel = this.zombieSiegeForm.getRawValue() as  ZombieSiegeModel;
    this.playerParticipated.forEach((p) => {
      const player = this.originalZombieSiegeParticipants.find(d => d.playerId == p.playerId)!;
      if (player.survivedWaves !== p.survivedWaves) {
        toUpdate.push(p);
      }
    });
    this._zombieSiegeService.updateZombieSiege(updateZombieSiege.id, updateZombieSiege).subscribe({
      next: ((response) => {
        if (response) {
          this.updateZombieSiegeParticipants(toUpdate);
        }
      })
    })
  }

  private updateZombieSiegeParticipants(zombieSiegeParticipants: ZombieSiegeParticipantModel[]) {
    if (zombieSiegeParticipants.length <= 0) {
      this._toastr.success('Successfully updated!', 'Successfully');
      this.onCancel();
      this.getZombieSieges(10);
      return;
    }
    const requests: Observable<ZombieSiegeParticipantModel>[] = [];

    zombieSiegeParticipants.forEach((participant) => {
      const request = this._zombieSiegeParticipantService.updateZombieSiegeParticipant(participant.id, participant);
      requests.push(request);
    })
    forkJoin(requests).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated!', 'Successfully');
          this.onCancel();
          this.getZombieSieges(10);
        }
      })
    })
  }
}
