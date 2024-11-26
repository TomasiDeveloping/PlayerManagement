export interface DesertStormParticipantModel {
  id: string;
  desertStormId: string;
  playerId: string;
  playerName: string;
  registered: boolean;
  participated: boolean;
  startPlayer: boolean;
}

export interface CreateDesertStormParticipantModel {
  desertStormId: string;
  playerId: string;
  registered: boolean;
  participated: boolean;
  startPlayer: boolean;
}
