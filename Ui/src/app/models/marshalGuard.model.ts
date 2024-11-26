import {MarshalGuardParticipantModel} from "./marshalGuardParticipant.model";

export interface MarshalGuardModel {
  id: string;
  allianceId: string;
  participants: number;
  level: number;
  rewardPhase: number;
  allianceSize: number;
  eventDate: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
}

export interface MarshalGuardDetailModel extends MarshalGuardModel {
  marshalGuardParticipants: MarshalGuardParticipantModel[];
}

export interface CreateMarshalGuardModel {
  allianceId: string;
  rewardPhase: number;
  level: number;
  allianceSize: number;
  eventDate: string;
}

export interface UpdateMarshalGuardModel {
  id: string;
  allianceId: string;
  rewardPhase: number;
  level: number;
  eventDate: string;
  participants: number;
}
