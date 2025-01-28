import {ZombieSiegeParticipantModel} from "./zombieSiegeParticipant.model";

export interface ZombieSiegeModel {
  id: string;
  allianceId: string;
  eventDate: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
  allianceSize: number;
  level: number;
  totalLevel20Players: number;
  totalWavesSurvived: number;
}

export interface ZombieSiegeDetailModel extends ZombieSiegeModel{
  zombieSiegeParticipants: ZombieSiegeParticipantModel[];
}

export interface CreateZombieSiegeModel {
  allianceId: string;
  allianceSize: number;
  level: number;
  eventDate: string;
}
