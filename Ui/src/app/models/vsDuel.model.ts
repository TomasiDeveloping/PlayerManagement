import {VsDuelParticipantModel} from "./vsDuelParticipant.model";

export interface VsDuelModel {
  id: string;
  allianceId: string;
  eventDate: Date;
  won: boolean;
  opponentName: string;
  opponentServer: number;
  opponentPower: number;
  opponentSize: number;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
}

export interface VsDuelDetailModel extends VsDuelModel {
  vsDuelParticipants: VsDuelParticipantModel[];
}
