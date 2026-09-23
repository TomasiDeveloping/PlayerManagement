export interface PlayerCombatRecordModel {
  id: string;
  playerId: string;
  squad1: string;
  squad1Power: number;
  squad2Power: number;
  squad2: string;
  squad3Power: number;
  squad3: string;
  totalHeroPower: number;
  kills: number;
  recordedAtUtc?: string;
}
