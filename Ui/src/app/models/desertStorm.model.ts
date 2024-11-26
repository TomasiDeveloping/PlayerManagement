import {DesertStormParticipantModel} from "./desertStormParticipant.model";

export interface DesertStormModel {
  id: string;
  allianceId: string;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
  won: boolean;
  opposingParticipants: number;
  opponentServer: number;
  eventDate: Date;
  opponentName: string;
  participants: number;
}

export interface DesertStormDetailModel extends DesertStormModel {
  desertStormParticipants: DesertStormParticipantModel[];
}

export interface CreateDesertStormModel {
  allianceId: string;
  won: boolean;
  opposingParticipants: number;
  opponentServer: number;
  eventDate: string;
  opponentName: string;
}
