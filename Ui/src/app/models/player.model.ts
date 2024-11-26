export interface PlayerModel {
  id: string;
  playerName: string;
  level: number;
  rankName: string;
  rankId: string;
  allianceId: string;
  createdOn: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
  notesCount: number;
  admonitionsCount: number;
}

export interface CreatePlayerModel {
  playerName: string;
  rankId: string;
  allianceId: string;
  level: number;
}

export interface UpdatePlayerModel {
  id: string;
  playerName: string;
  rankId: string;
  level: number;
}
