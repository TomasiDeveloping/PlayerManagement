export interface MarshalGuardParticipantModel {
  id: string;
  playerId: string;
  marshalGuardId: string;
  participated: boolean;
  playerName: string;
}

export interface CreateMarshalGuardParticipantModel {
  playerId: string;
  marshalGuardId: string;
  participated: boolean;
}
