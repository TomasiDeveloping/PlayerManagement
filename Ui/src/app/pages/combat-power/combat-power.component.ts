import {Component, inject, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import { TRANSLATIONS } from '../../helpers/translation';
import {PlayerCombatService} from "../../services/player-combat.service";
import {AllianceAccessTokenService} from "../../services/alliance-access-token.service";
import {PlayerCombatRecordModel} from "../../models/playerCombatRecord.model";

@Component({
  selector: 'app-combat-power',
  standalone: false,
  templateUrl: './combat-power.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './combat-power.component.css',
})
export class CombatPowerComponent implements OnInit {

  private readonly _route: ActivatedRoute = inject(ActivatedRoute);
  private readonly _playerCombatService: PlayerCombatService = inject(PlayerCombatService);
  private readonly _accessTokenService: AllianceAccessTokenService = inject(AllianceAccessTokenService);

  token: string | null = null;
  allianceName: string = '';
  players: {id: string, name: string}[] = [];
  troopTypes: string[] = ['Tank', 'Air', 'Missile'];
  selectedPlayerName: string = '';
  activeTab: string = 'form';

  playerSearchQuery: string = '';
  isPlayerDropdownOpen: boolean = false;

  currentLang: string = 'en';
  translations = TRANSLATIONS;
  isLangDropdownOpen: boolean = false;


  selectedPlayerId: string = '';
  squad1Type: string = 'Tank';
  squad1Power: number | null = null;
  squad2Type: string = 'Tank';
  squad2Power: number | null = null;
  squad3Type: string = 'Tank';
  squad3Power: number | null = null;
  totalHeroPower: number | null = null;
  kills: number| null = null;

  isLoading: boolean = true;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isSubmitting: boolean = false;
  submitErrorMessage: string = '';

  activeHelpTitle: string = '';
  activeHelpText: string = '';
  activeHelpImage: string | null = null;
  isHelpModalOpen: boolean = false;

  t(key: string): string {
    return this.translations[this.currentLang]?.[key] || this.translations['en'][key] || key;
  }


  get filteredPlayers() {
    if (!this.playerSearchQuery) {
      return this.players;
    }
    const query = this.playerSearchQuery.toLowerCase();
    return this.players.filter(p => p.name.toLowerCase().includes(query));
  }

  changeLanguage(lang: string) {
    this.currentLang = lang;
    this.isLangDropdownOpen = false;
  }

  ngOnInit() {
    this.token = this._route.snapshot.queryParamMap.get('token');

    if (!this.token) {
      this.isLoading = false;
      this.errorMessage = 'Access denied. No security token provided in the URL.';
      return;
    }
    this.validateToken(this.token);
  }

  validateToken(token: string) {
    this.isLoading = true;
    this._accessTokenService.validate(token).subscribe({
      next: result => {
        if (result) {
          this.loadData(result.allianceId);
        } else {
          this.errorMessage = 'The provided token is invalid, expired, or has been revoked.';
          this.isLoading = false;
        }
      },
      error: _ => {
        this.errorMessage = 'The provided token is invalid, expired, or has been revoked.';
        this.isLoading = false;
      }
    });
  }

  loadData(allianceId: string) {
    this._playerCombatService.getPlayers(allianceId).subscribe({
      next: result => {
        if (result) {
          this.allianceName = result.allianceName;
          this.players = result.players;
          this.isLoading = false;
        } else {
          this.errorMessage = 'Something went wrong.';
          this.isLoading = false;
        }
      },
      error: _ => {
        this.errorMessage = 'Something went wrong.';
        this.isLoading = false;
      }
    });
  };

  selectPlayerFromSearch(player: any) {
    this.selectedPlayerId = player.id;
    this.playerSearchQuery = player.name;
    this.isPlayerDropdownOpen = false;
    this.onPlayerSelected();
  }

  onPlayerSelected() {
    const found = this.players.find(p => p.id === this.selectedPlayerId);
    this.selectedPlayerName = found ? found.name : '';
  }


  resetPlayerSelection() {
    this.selectedPlayerId = '';
    this.selectedPlayerName = '';
  }

  setTab(tabName: string) {
    this.activeTab = tabName;
  }


  onSubmit(): void {
    if (!this.token || !this.selectedPlayerId) {
      this.errorMessage = 'Please select a player before submitting.';
      return;
    }

    this.isSubmitting = true;
    this.submitErrorMessage = '';
    this.successMessage = '';

    const payload: PlayerCombatRecordModel = {
      playerId: this.selectedPlayerId,
      id: '',
      squad1: this.squad1Type,
      recordedAtUtc: undefined,
      squad1Power: this.squad1Power as number,
      squad2: this.squad2Type,
      squad2Power: this.squad2Power as number,
      squad3: this.squad3Type,
      squad3Power: this.squad3Power as number,
      totalHeroPower: this.totalHeroPower as number,
      kills: this.kills as number
    };
    this._playerCombatService.addRecord(payload).subscribe({
      next: result => {
        if (result) {
          this.isSubmitting = false;
          this.successMessage = 'Your records have been successfully updated!';
        } else {
          this.isSubmitting = false;
          this.submitErrorMessage = 'Failed to submit records. Please try again.';
        }
      },
      error: _ => {
        this.isSubmitting = false;
        this.submitErrorMessage = 'Failed to submit records. Please try again.';
      }
    })
  }


  openHelpModal(title: string, text: string, imageUrl: string | null = null): void {
    this.activeHelpTitle = title;
    this.activeHelpText = text;
    this.activeHelpImage = imageUrl;
    this.isHelpModalOpen = true;
  }

  closeHelpModal(): void {
    this.isHelpModalOpen = false;
    this.activeHelpImage = null;
  }

}
