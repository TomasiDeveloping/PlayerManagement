export interface PlayerCombatRecordOverviewModel {
  playerId: string;
  playerName: string;
  squad1?: number;
  squad2?: number;
  squad3?: number;
  squad1Power: number;
  squad2Power: number;
  squad3Power: number;
  totalHeroPower: number;
  kills: number;
  recordedAtUtc?: Date;
}
