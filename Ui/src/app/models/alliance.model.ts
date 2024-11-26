export interface AllianceModel {
  id: string;
  server: number;
  name: string;
  abbreviation: string;
  createdOn: Date;
  modifiedOn?: Date;
  modifiedBy?: string;
}
