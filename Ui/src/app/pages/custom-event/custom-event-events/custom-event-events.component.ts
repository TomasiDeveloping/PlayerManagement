import {Component, inject, OnInit} from '@angular/core';
import {CreateCustomEventModel, CustomEventDetailModel, CustomEventModel} from "../../../models/customEvent.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CustomEventParticipantModel} from "../../../models/customEventParticipant.model";
import {CustomEventCategoryModel} from "../../../models/customEventCategory.model";
import {JwtTokenService} from "../../../services/jwt-token.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {CustomEventService} from "../../../services/custom-event.service";
import {CustomEventParticipantService} from "../../../services/custom-event-participant.service";
import {CustomEventCategoryService} from "../../../services/custom-event-category.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {
  CustomEventParticipantsModelComponent
} from "../../../modals/custom-event-participants-model/custom-event-participants-model.component";
import Swal from "sweetalert2";
import {forkJoin, Observable} from "rxjs";

@Component({
  selector: 'app-custom-event-events',
  templateUrl: './custom-event-events.component.html',
  styleUrl: './custom-event-events.component.css'
})
export class CustomEventEventsComponent implements OnInit {

  customEvents: CustomEventModel[] = [];
  customEventDetail!: CustomEventDetailModel;
  isCreateCustomEvent: boolean = false;
  customEventForm!: FormGroup;
  playerSelected: boolean = false;
  playerParticipated: { playerId: string, playerName: string, participated: boolean, achievedPoints: number }[] = [];
  customEventParticipants: CustomEventParticipantModel[] = [];
  customEventCategories: CustomEventCategoryModel[] = [];
  isUpdate: boolean = false;
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _modalService: NgbModal = inject(NgbModal);
  private readonly _customEventService: CustomEventService = inject(CustomEventService);
  private readonly _customEventParticipantService: CustomEventParticipantService = inject(CustomEventParticipantService);
  private readonly _customEventCategoryService: CustomEventCategoryService = inject(CustomEventCategoryService);
  private readonly _router: Router = inject(Router);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private allianceId: string = this._tokenService.getAllianceId()!;

  public totalRecord: number = 0;
  public pageNumber: number = 1;
  public pageSize: number = 10

  get f() {
    return this.customEventForm.controls;
  }

  ngOnInit() {
    this.getCustomEvents();
    this.getCustomEventCategories();
  }

  getCustomEvents() {
    this.customEvents = [];
    this._customEventService.getAllianceCustomEvents(this.allianceId, this.pageNumber, this.pageSize).subscribe({
      next: ((response) => {
        if (response) {
          this.customEvents = response.data;
          this.totalRecord = response.totalRecords;
        }
      }),
      error: (error) => {
        console.error(error);
      }
    });
  }

  getCustomEventCategories() {
    this._customEventCategoryService.getAllianceCustomEventCategories(this.allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.customEventCategories = response;
        } else {
          this.customEventCategories = [];
        }
      }),
      error: (error) => {
        console.error(error);
      }
    });
  }


  onCreateEvent() {
    this.createCustomEventForm(false);
  }


  createCustomEventForm(isUpdate: boolean, customEventDetail: CustomEventDetailModel | null = null) {
    if (isUpdate) {
      this.playerSelected = true;
      this.playerParticipated = [...customEventDetail!.customEventParticipants];
    } else {
      this.playerSelected = false;
    }
    const d = isUpdate ? new Date(customEventDetail!.eventDate) : new Date();
    this.customEventForm = new FormGroup({
      id: new FormControl<string>(isUpdate ? customEventDetail!.id : ''),
      allianceId: new FormControl<string>(this.allianceId),
      customEventCategoryId: new FormControl<string | null>(null),
      name: new FormControl<string>(isUpdate ? customEventDetail!.name : '', [Validators.required, Validators.maxLength(150)]),
      description: new FormControl<string>(isUpdate ? customEventDetail!.description : '', [Validators.required, Validators.maxLength(500)]),
      isPointsEvent: new FormControl<boolean>(isUpdate ? customEventDetail!.isPointsEvent : false),
      isParticipationEvent: new FormControl(isUpdate ? customEventDetail!.isParticipationEvent : false),
      eventDate: new FormControl<string>(new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate())).toISOString().substring(0, 10)),
      isInProgress: new FormControl<boolean>(isUpdate ? customEventDetail!.isInProgress : true)
    });
    this.isCreateCustomEvent = true;
    if (isUpdate) {
      this.customEventForm.controls['customEventCategoryId'].disable();
    }
  }

  onAddParticipants() {
    const modalRef = this._modalService.open(CustomEventParticipantsModelComponent,
      {animation: true, backdrop: 'static', centered: true, size: 'lg', scrollable: true});
    if (this.playerSelected) {
      this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
      modalRef.componentInstance.players = [...this.playerParticipated];
    }
    modalRef.componentInstance.allianceId = this.allianceId;
    modalRef.componentInstance.isParticipationEvent = this.customEventForm.controls['isParticipationEvent'].value;
    modalRef.componentInstance.isPointEvent = this.customEventForm.controls['isPointsEvent'].value;
    modalRef.closed.subscribe({
      next: ((response: any[]) => {
        if (response) {
          this.playerSelected = true;
          this.playerParticipated = [...response];
          this.playerParticipated.sort((a, b) => a.playerName.localeCompare(b.playerName));
        }
      })
    })
  }

  onCancel() {
    this.isUpdate = false;
    this.isCreateCustomEvent = false;
  }

  onSubmit() {
    if (this.customEventForm.invalid) {
      return;
    }
    if (this.isUpdate) {
      this.updateCustomEvent();
      return;
    }
    const customEvent: CreateCustomEventModel = this.customEventForm.getRawValue() as CreateCustomEventModel;
    this._customEventService.createCustomEvent(customEvent).subscribe({
      next: ((response) => {
        if (response) {
          this.insertCustomEventParticipants(response.id);
        }
      }),
      error: ((error: any) => {
        console.error(error);
      })
    })
  }

  insertCustomEventParticipants(customEventId: string) {
    const customEventParticipants: CustomEventParticipantModel[] = [];
    this.playerParticipated.forEach((player) => {
      const participant: CustomEventParticipantModel = {
        id: '',
        customEventId: customEventId,
        participated: player.participated,
        playerId: player.playerId,
        achievedPoints: player.achievedPoints,
        playerName: player.playerName
      };
      customEventParticipants.push(participant);
    });
    this._customEventParticipantService.insertCustomEventParticipants(customEventParticipants).subscribe({
      next: (() => {
        this.playerParticipated = [];
        this.onCancel();
        this.resetAndGetCustomEvents();
      }),
      error: (error) => {
        console.log(error);
      }
    });
  }

  onGoToCustomEventDetail(customEvent: CustomEventModel) {
    this._router.navigate(['custom-event-detail', customEvent.id]).then();
  }

  onEditCustomEvent(customEvent: CustomEventModel) {
    this._customEventService.getCustomEventDetail(customEvent.id).subscribe({
      next: ((response) => {
        if (response) {
          this.customEventParticipants = structuredClone(response.customEventParticipants);
          this.isUpdate = true;
          this.createCustomEventForm(true, response);

          this.customEventDetail = response;
        }
      })
    });
  }

  onDeleteCustomEvent(customEvent: CustomEventModel) {
    Swal.fire({
      title: "Delete Custom event ?",
      text: `Do you really want to delete the custom event`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._customEventService.deleteCustomEvent(customEvent.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Custom event has been deleted",
                icon: "success"
              }).then(_ => this.resetAndGetCustomEvents());
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  private updateCustomEvent() {
    const participantsToUpdate: any[] = [];
    const customEvent: CustomEventModel = this.customEventForm.getRawValue() as CustomEventModel;
    this.customEventParticipants.forEach((participant) => {
      const player = this.playerParticipated.find(p => p.playerId === participant.playerId);
      if (player!.participated !== participant.participated || player!.achievedPoints !== participant.achievedPoints) {
        const playerToUpdate: CustomEventParticipantModel = {
          playerId: player!.playerId,
          achievedPoints: player!.achievedPoints,
          playerName: player!.playerName,
          participated: player!.participated,
          id: participant.id,
          customEventId: participant.customEventId,
        }
        participantsToUpdate.push(playerToUpdate);
      }
    });
    this._customEventService.updateCustomEvent(customEvent.id, customEvent).subscribe({
      next: ((response) => {
        if (response) {
          this.updateCustomEventParticipants(participantsToUpdate);
        }
      })
    });
  }

  private updateCustomEventParticipants(participantsToUpdate: any[]) {
    if (participantsToUpdate.length <= 0) {
      this._toastr.success('Successfully updated!', 'Successfully');
      this.onCancel();
      this.resetAndGetCustomEvents();
      return;
    }

    const requests: Observable<CustomEventParticipantModel>[] = [];
    participantsToUpdate.forEach(participant => {
      const request = this._customEventParticipantService.updateCustomEventParticipant(participant.id, participant);
      requests.push(request);
    })
    forkJoin(requests).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated!', 'Successfully');
          this.onCancel();
          this.resetAndGetCustomEvents();
        }
      })
    })
  }

  pageChanged(event: number) {
    this.pageNumber = event;
    this.getCustomEvents();
  }

  resetAndGetCustomEvents() {
    this.pageNumber = 1;
    this.getCustomEvents();
  }

  onCategoryChange() {
    const categoryId = this.customEventForm.get('customEventCategoryId')?.value;
    if (categoryId !== null) {
      const category = this.customEventCategories.find(e => e.id === categoryId);
      this.customEventForm.controls['isPointsEvent'].setValue(category?.isPointsEvent);
      this.customEventForm.controls['isPointsEvent'].disable();
      this.customEventForm.controls['isParticipationEvent'].setValue(category?.isParticipationEvent);
      this.customEventForm.controls['isParticipationEvent'].disable();
    } else {
      this.customEventForm.controls['isPointsEvent'].setValue(false);
      this.customEventForm.controls['isParticipationEvent'].setValue(false);
      this.customEventForm.controls['isPointsEvent'].enable();
      this.customEventForm.controls['isParticipationEvent'].enable();
    }
  }
}
