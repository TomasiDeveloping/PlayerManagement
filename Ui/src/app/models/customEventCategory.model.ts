export interface CustomEventCategoryModel {
  id: string;
  allianceId: string;
  name: string;
  isPointsEvent: boolean;
  isParticipationEvent: boolean;
}

export interface CreateCustomEventCategoryModel {
  allianceId: string;
  name: string;
  isPointsEvent: boolean;
  isParticipationEvent: boolean;
}

export interface UpdateCustomEventCategoryModel {
  id: string;
  name: string;
  isPointsEvent: boolean;
  isParticipationEvent: boolean;
}
