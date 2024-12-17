export interface CustomEventParticipantModel {
  id: string;
  playerId: string;
  customEventId: string;
  participated: boolean;
  achievedPoints: number;
  playerName: string;
}
