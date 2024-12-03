import {CustomEventParticipantModel} from "./customEventParticipant.model";

export interface CustomEventModel {
  id: string;
  allianceId: string;
  name: string;
  description: string;
  isPointsEvent?: boolean;
  isParticipationEvent?: boolean;
  eventDate: Date;
  isInProgress: boolean;
}

export interface CustomEventDetailModel extends CustomEventModel {
  customEventParticipants: CustomEventParticipantModel[];
}
