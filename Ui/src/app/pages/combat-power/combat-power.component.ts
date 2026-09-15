import {Component, inject, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {PlayerModel} from "../../models/player.model";

@Component({
  selector: 'app-combat-power',
  standalone: false,
  templateUrl: './combat-power.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './combat-power.component.css',
})
export class CombatPowerComponent implements OnInit {

  private readonly _route: ActivatedRoute = inject(ActivatedRoute);

  token: string | null = null;
  allianceName: string = '';
  players: PlayerModel[] = [];
  troopTypes: string[] = ['Tank', 'Air', 'Missile'];

  // Formular-Daten
  selectedPlayerId: string = '';
  squad1Type: string = 'Tank';
  squad1Power: number = 0;
  squad2Type: string = 'Tank';
  squad2Power: number = 0;
  squad3Type: string = 'Tank';
  squad3Power: number = 0;
  totalHeroPower: number = 0;
  kills: number = 0;

  // UI States
  isLoading: boolean = true;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isSubmitting: boolean = false;

  ngOnInit() {
    this.token = this._route.snapshot.queryParamMap.get('token');

    if (!this.token) {
      this.isLoading = false;
      this.errorMessage = 'Access denied. No security token provided in the URL.';
      return;
    }
  }

  onSubmit(): void {
    if (!this.token || !this.selectedPlayerId) {
      this.errorMessage = 'Please select a player before submitting.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;
    this.successMessage = null;

    const payload = {
      token: this.token,
      playerId: this.selectedPlayerId,
      squad1: this.squad1Type,
      squad1Power: this.squad1Power,
      squad2: this.squad2Type,
      squad2Power: this.squad2Power,
      squad3: this.squad3Type,
      squad3Power: this.squad3Power,
      totalHeroPower: this.totalHeroPower,
      kills: this.kills
    }
  }

}
