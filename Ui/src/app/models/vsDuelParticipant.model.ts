export interface VsDuelParticipantModel {
  id: string;
  playerId: string;
  vsDuelId: string;
  weeklyPoints: number;
  playerName: string;
}

export interface VsDuelParticipantDetailModel {
  playerId: string;
  eventDate: Date;
  weeklyPoints: number;
}
