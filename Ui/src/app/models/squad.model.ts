export interface SquadModel extends CreateSquadModel{
  id: string;
  typeName: string;
  lastUpdateAt: Date;
}

export interface CreateSquadModel {
  squadTypeId: string;
  playerId: string;
  power: number;
  position: number;
}

export interface UpdateSquadModel extends CreateSquadModel {
  id: string;
}
