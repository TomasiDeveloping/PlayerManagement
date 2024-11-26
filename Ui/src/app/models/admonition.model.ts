export interface AdmonitionModel {
  id: string;
  reason: string;
  playerId: string;
  createdOn: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
}
