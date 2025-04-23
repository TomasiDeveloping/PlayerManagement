import {Component, inject, OnInit} from '@angular/core';
import {CustomEventCategoryModel} from "../../../models/customEventCategory.model";
import {CustomEventCategoryService} from "../../../services/custom-event-category.service";
import {ToastrService} from "ngx-toastr";
import {JwtTokenService} from "../../../services/jwt-token.service";
import {CustomEventLeaderboardService} from "../../../services/custom-event-leaderboard.service";
import {
  LeaderboardParticipationEventModel, LeaderboardPointAndParticipationEventModel,
  LeaderboardPointEventModel
} from "../../../models/customEventLeaderboard.model";

@Component({
  selector: 'app-custom-event-leaderboard',
  templateUrl: './custom-event-leaderboard.component.html',
  styleUrl: './custom-event-leaderboard.component.css'
})
export class CustomEventLeaderboardComponent implements OnInit {

  private readonly _customEventCategoryService: CustomEventCategoryService = inject(CustomEventCategoryService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _customLeaderboardService: CustomEventLeaderboardService = inject(CustomEventLeaderboardService);

  private allianceId: string = this._tokenService.getAllianceId()!;

  customEventCategories: CustomEventCategoryModel[] = [];
  selectedCustomEventCategory: CustomEventCategoryModel | undefined;
  pointEventLeaderboard: LeaderboardPointEventModel[] | undefined;
  participationEventLeaderboard: LeaderboardParticipationEventModel[] | undefined;
  pointAndParticipationEventLeaderboard: LeaderboardPointAndParticipationEventModel[] | undefined;
  pointPageNumber: number = 1;
  participationPageNumber: number = 1;
  pointAndParticipationPageNumber: number = 1;
  showInfo: boolean = false;
  activeTab: number = 1;

  ngOnInit() {
    this.getCustomEventCategories();
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

  onCategoryChange(event: any) {
    this.pointEventLeaderboard = undefined;
    this.participationEventLeaderboard = undefined;
    this.pointAndParticipationEventLeaderboard = undefined;
    const categoryId = event.target.value;
    if (categoryId !== 'null') {
      this.selectedCustomEventCategory = this.customEventCategories.find(e => e.id === categoryId);
      if (this.selectedCustomEventCategory!.isPointsEvent && this.selectedCustomEventCategory!.isParticipationEvent) {
        this.getPointAndParticipationEventLeaderboard(this.selectedCustomEventCategory!.id);
        return;
      }
      if (this.selectedCustomEventCategory!.isParticipationEvent) {
        this.getParticipationEventLeaderboard(this.selectedCustomEventCategory!.id);
        return;
      }
      if (this.selectedCustomEventCategory!.isPointsEvent) {
        this.getPointEventLeaderboard(this.selectedCustomEventCategory!.id);
      }
    } else {
      this.selectedCustomEventCategory = undefined;
    }
  }

  getPointEventLeaderboard(customEventCategoryId: string) {
    this._customLeaderboardService.getPointEvent(customEventCategoryId).subscribe({
      next: ((response) => {
        if (response) {
          this.pointEventLeaderboard = response;
        } else {
          this.pointEventLeaderboard = [];
        }
      })
    })
  }

  getParticipationEventLeaderboard(customEventCategoryId: string) {
    this._customLeaderboardService.getParticipationEvent(customEventCategoryId).subscribe({
      next: ((response) => {
        if (response) {
          this.participationEventLeaderboard = response;
        } else {
          this.participationEventLeaderboard = [];
        }
      })
    })
  }

  getPointAndParticipationEventLeaderboard(customEventCategoryId: string) {
    this._customLeaderboardService.getPointAndParticipationEvent(customEventCategoryId).subscribe({
      next: ((response) => {
        if (response) {
          this.pointAndParticipationEventLeaderboard = response;
        } else {
          this.pointAndParticipationEventLeaderboard = [];
        }
      })
    })
  }

  pointPageChanged(event: number) {
    this.pointPageNumber = event;
  }

  participationPageChanged(event: number) {
    this.participationPageNumber = event;
  }

  pointAndParticipationPageChanged(event: number) {
    this.pointAndParticipationPageNumber = event;
  }

  toggleInfo() {
    this.showInfo = !this.showInfo;
  }
}
