export interface ApiKeyModel {
  id: string;
  allianceId: string;
  key: string;
  createdOn: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
}

export interface CreateApiKeyModel {
  allianceId: string;
}

export interface UpdateApiKeyModel {
  id: string;
  allianceId: string;
}
