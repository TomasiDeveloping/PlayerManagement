import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {
  CreateAccessTokenModalComponent
} from "../../modals/create-access-token-modal/create-access-token-modal.component";
import {JwtTokenService} from "../../services/jwt-token.service";
import {AllianceAccessTokenService} from "../../services/alliance-access-token.service";
import {AllianceAccessTokenModel} from "../../models/allianceAccessToken.model";
import {PlayerCombatService} from "../../services/player-combat.service";
import {PlayerCombatRecordOverviewModel} from "../../models/playerCombatRecordOverview.model";
import {
  PowerTrackingEditDialogComponent
} from "../../modals/power-tracking-edit-dialog/power-tracking-edit-dialog.component";
import {
  PowerTrackingRecordsModalComponent
} from "../../modals/power-tracking-records-modal/power-tracking-records-modal.component";
import {
  PlayerPowerRecordsModalComponent
} from "../../modals/player-power-records-modal/player-power-records-modal.component";
import {
  PowerTrackingInfoModalComponent
} from "../../modals/power-tracking-info-modal/power-tracking-info-modal.component";
import Swal from "sweetalert2";

@Component({
  selector: 'app-power-tracking',
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './power-tracking.component.css',
  templateUrl: './power-tracking.component.html',
})
export class PowerTrackingComponent implements OnInit {

  private modalService: NgbModal = inject(NgbModal);
  private tokenService: JwtTokenService = inject(JwtTokenService);
  private accessTokenService: AllianceAccessTokenService = inject(AllianceAccessTokenService);
  private playerCombatService: PlayerCombatService = inject(PlayerCombatService);

  activeTab: 'roster' | 'tokens' = 'roster';
  allianceId: string | null = null;

  currentSortColumn: keyof PlayerCombatRecordOverviewModel | '' = 'squad1Power';
  isAscending: boolean = false;

  players: PlayerCombatRecordOverviewModel[] = [];
  filteredPlayers: PlayerCombatRecordOverviewModel[] = [];
  tokens: AllianceAccessTokenModel[] = [];

  pageSize: number = 10;

  searchText: string = '';
  filterOutdatedOrEmpty: boolean = false;

  pageNumberPlayer: number = 1;
  totalPlayers: number = 0;

  pageNumberToken: number = 1;
  totalTokens: number = 0;

  isLoading: boolean = false;

  ngOnInit(): void {
    this.allianceId = this.tokenService.getAllianceId();
    if (this.allianceId == null) {
      return;
    }
    this.loadTokens();
    this.loadData();
  }

  loadTokens(): void {
    this.accessTokenService.getTokens(this.allianceId!).subscribe({
      next: data => {
        this.tokens = data;
      }
    })
  }

  loadData(): void {
    this.isLoading = true;
    this.playerCombatService.getAllianzOverview(this.allianceId!).subscribe({
      next: data => {
        this.players = data;
        this.applyFilters();
        this.totalPlayers = data.length;
        this.isLoading = false;
      },
      error: err => {
        this.isLoading = false;
        console.log(err);
      }
    });
  }

  onFilterChanged(): void {
    this.pageNumberPlayer = 1;
    this.applyFilters();
  }

  sortPlayers(column: keyof PlayerCombatRecordOverviewModel) {
    if (this.currentSortColumn === column) {
      this.isAscending = !this.isAscending;
    } else {
      this.currentSortColumn = column;
      this.isAscending = false;
    }

    const sortFn = (list: PlayerCombatRecordOverviewModel[]) => {
      list.sort((a, b) => {
        let valueA = a[column] ?? 0;
        let valueB = b[column] ?? 0;

        if (typeof valueA === 'string' && typeof valueB === 'string') {
          return this.isAscending ? valueA.localeCompare(valueB) : valueB.localeCompare(valueA);
        }

        return this.isAscending
          ? (valueA > valueB ? 1 : valueA < valueB ? -1 : 0)
          : (valueA < valueB ? 1 : valueA > valueB ? -1 : 0);
      });
    };

    sortFn(this.players);
    sortFn(this.filteredPlayers);
  }

  switchTab(tab: 'roster' | 'tokens'): void {
    this.activeTab = tab;
  }

  pageChangedPlayer(event: number): void {
    this.pageNumberPlayer = event;
  }

  openPlayerHistory(playerCombatOverview: PlayerCombatRecordOverviewModel): void {
    const modalRef = this.modalService.open(PowerTrackingRecordsModalComponent, {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.playerName = playerCombatOverview.playerName;
    modalRef.componentInstance.playerId = playerCombatOverview.playerId;
  }

  openEditPlayer(playerCombatOverview: PlayerCombatRecordOverviewModel): void {
    const modalRef = this.modalService.open(PlayerPowerRecordsModalComponent, {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.playerId = playerCombatOverview.playerId;
    modalRef.componentInstance.playerName = playerCombatOverview.playerName;

    modalRef.result.then((result) => {
      if (result) {
        this.loadData();
      }
    }).catch(() => {})
  }

  openCreateTokenModal(): void {
    const modalRef = this.modalService.open(CreateAccessTokenModalComponent, {animation: true, backdrop: 'static', centered: true, size: 'lg'});
    modalRef.componentInstance.allianceId = this.allianceId;

    modalRef.result.then((result) => {
      if (result) {
        this.loadTokens();
      }
    }, () => {
    });
  }

  revokeToken(tokenId: string): void {
    Swal.fire({
      title: "Revoke Token?",
      text: "Do you really want to revoke this token?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, revoke it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.accessTokenService.revokeToken(tokenId).subscribe({
          next: () => {
            Swal.fire({
              title: "Revoked!",
              text: "The token has been revoked.",
              icon: "success"
            }).then(_ => this.loadTokens());
          },
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  copyTokenLink(fullShareUrl: string): void {
    navigator.clipboard.writeText(fullShareUrl).then(() => {
      console.log('Copied to clipboard:', fullShareUrl);
    }).catch(err => {
      console.error('Failed to copy', err);
    });
  }

  pageChangedToken(event: number): void {
    this.pageNumberToken = event;
  }

  openNewRecordModal() {
    const modalRef = this.modalService.open(PowerTrackingEditDialogComponent, {animation: true, backdrop: 'static', centered: true, size: 'lg'});

    modalRef.componentInstance.isUpdate = false;

    modalRef.result.then((result) => {
      if (result === true) {
        this.loadData();
      }
    }).catch(() => {})
  }

  private applyFilters(): void {
    let result = [...this.players];

    if (this.searchText && this.searchText.trim() !== '') {
      const query = this.searchText.toLowerCase().trim();
      result = result.filter(p => p.playerName.toLowerCase().includes(query));
    }

    if (this.filterOutdatedOrEmpty) {
      const oneMonthAgo = new Date();
      oneMonthAgo.setMonth(oneMonthAgo.getMonth() - 1);

      result = result.filter(p => {
        if (!p.recordedAtUtc) {
          return true;
        }
        const recordDate = new Date(p.recordedAtUtc);
        return recordDate < oneMonthAgo;
      });
    }

    this.filteredPlayers = result;
    this.totalPlayers = result.length;
  }

  exportAllPlayers(scope: 'latest' | 'all', format: 'excel' | 'csv') {
    this.playerCombatService.exportAllianceData(this.allianceId!, scope, format).subscribe({
      next: (response: Blob)=> {
        const extension = format === 'excel' ? 'xlsx' : 'csv';
        const filename = `alliance-power-${scope}-${new Date().toISOString().slice(0, 10)}.${extension}`;

        const blob = new Blob([response], {
          type: format === 'excel'
            ? 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            : 'text/csv'
        });

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();

        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.log(err);
      }
    });
  }

  openInfoModal() {
    this.modalService.open(PowerTrackingInfoModalComponent, {centered: true, size: 'md'});
  }
}
