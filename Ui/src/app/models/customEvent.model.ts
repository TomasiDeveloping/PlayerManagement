import {CustomEventParticipantModel} from "./customEventParticipant.model";

export interface CustomEventModel {
  id: string;
  allianceId: string;
  name: string;
  description: string;
  isPointsEvent: boolean;
  isParticipationEvent: boolean;
  eventDate: Date;
  isInProgress: boolean;
  createdBy: string;
  modifiedBy?: string;
  modifiedOn?: Date;
}

export interface CustomEventDetailModel extends CustomEventModel {
  customEventParticipants: CustomEventParticipantModel[];
}

export interface CreateCustomEventModel {
  name: string;
  description: string;
  isPointsEvent: boolean;
  isParticipationEvent: boolean;
  eventDate: string;
  allianceId: string;
  isInProgress: boolean;
}
