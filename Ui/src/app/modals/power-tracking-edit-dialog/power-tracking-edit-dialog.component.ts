import {ChangeDetectionStrategy, Component, HostListener, inject, Input, OnInit} from '@angular/core';
import {PlayerCombatRecordModel} from "../../models/playerCombatRecord.model";
import {PlayerModel} from "../../models/player.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerService} from "../../services/player.service";
import {JwtTokenService} from "../../services/jwt-token.service";
import {PlayerCombatService} from "../../services/player-combat.service";

@Component({
  selector: 'app-power-tracking-edit-dialog',
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './power-tracking-edit-dialog.component.css',
  templateUrl: './power-tracking-edit-dialog.component.html',
})
export class PowerTrackingEditDialogComponent implements OnInit {

  private fb: FormBuilder = inject(FormBuilder);
  private playerService: PlayerService = inject(PlayerService);
  private combatService: PlayerCombatService = inject(PlayerCombatService);
  private tokenService: JwtTokenService = inject(JwtTokenService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);

  @Input() isUpdate: boolean = false;
  @Input() editData?: PlayerCombatRecordModel;

  recordForm!: FormGroup;
  players: PlayerModel[] = [];
  isLoading: boolean = false;
  isSubmitting: boolean = false;

  isDropdownOpen: boolean = false;
  playerSearchQuery: string = '';
  selectedPlayerName: string = '-- Select Commander --';

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.custom-dropdown-container')) {
      this.isDropdownOpen = false;
    }
  }

  get filteredPlayers() {
    if (!this.playerSearchQuery) return this.players;
    const query = this.playerSearchQuery.toLowerCase();
    return this.players.filter(p => p.playerName.toLowerCase().includes(query));
  }

  selectPlayer(player: PlayerModel) {
    this.recordForm.patchValue({ playerId: player.id });
    this.selectedPlayerName = player.playerName;
    this.isDropdownOpen = false;
    this.playerSearchQuery = '';
    this.recordForm.get('playerId')?.markAsTouched();
  }

  toggleDropdown(event: Event) {
    event.stopPropagation();
    if (this.isUpdate) return;
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  ngOnInit(): void {
    this.initForm();

    const todayStr = new Date().toISOString().substring(0, 10);

    if (this.isUpdate && this.editData) {
      let recordDate = todayStr;
      if (this.editData.recordedAtUtc) {
        recordDate = new Date(this.editData.recordedAtUtc).toISOString().substring(0, 10);
      }
      this.recordForm.patchValue({
        id: this.editData.id,
        playerId: this.editData.playerId,
        squad1: this.editData.squad1 !== null && this.editData.squad1 !== undefined ? this.editData.squad1.toString() : '0',
        squad1Power: this.editData.squad1Power || 0,
        squad2: this.editData.squad2 !== null && this.editData.squad2 !== undefined ? this.editData.squad2.toString() : '0',
        squad2Power: this.editData.squad2Power || 0,
        squad3: this.editData.squad3 !== null && this.editData.squad3 !== undefined ? this.editData.squad3.toString() : '0',
        squad3Power: this.editData.squad3Power || 0,
        totalHeroPower: this.editData.totalHeroPower || 0,
        kills: this.editData.kills || 0,
        recordedAtUtc: recordDate
      });
    } else {
      this.loadAlliancePlayers();
    }
  }

  private initForm(): void {
    this.recordForm = this.fb.group({
      id: [{ value: '', disabled: this.isUpdate }],
      playerId: [{ value: '', disabled: this.isUpdate }, Validators.required],
      squad1: ['0', Validators.required],
      squad1Power: [0, [Validators.required, Validators.min(0)]],
      squad2: ['0', Validators.required],
      squad2Power: [0, [Validators.required, Validators.min(0)]],
      squad3: ['0', Validators.required],
      squad3Power: [0, [Validators.required, Validators.min(0)]],
      totalHeroPower: [0, [Validators.required, Validators.min(0)]],
      kills: [0, [Validators.required, Validators.min(0)]],
      recordedAtUtc: [new Date().toISOString().substring(0, 10), Validators.required]
    });
  }

  loadAlliancePlayers(): void {
    const allianceId = this.tokenService.getAllianceId();
    if (!allianceId) {
      return;
    }
    this.isLoading = true;
    this.playerService.getAlliancePlayer(allianceId).subscribe({
      next: data => {
        this.players = data;
        this.isLoading = false;
      },
      error: error => {
        this.isLoading = false;
        console.log(error);
      }
    })
  }

  onSubmit(): void {
    if (this.recordForm.invalid) {
      this.recordForm.markAsTouched();
      return;
    }

    this.isSubmitting = true;
    const formValue: PlayerCombatRecordModel = this.recordForm.getRawValue() as PlayerCombatRecordModel;
    formValue.recordedAtUtc = new Date(formValue.recordedAtUtc!).toISOString();

    const request$ = this.isUpdate
      ? this.combatService.updateRecord(this.editData!.id, formValue)
      : this.combatService.addRecord(formValue);

    request$.subscribe({
      next: () => {
        this.isSubmitting = false;
        this.activeModal.close(true);
      },
      error: error => {
        this.isSubmitting = false;
        console.log(error);
      }
    });

  }



}
