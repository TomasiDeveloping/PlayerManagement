export interface NoteModel {
  id: string;
  playerId: string;
  playerNote: string;
  createdOn: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?:string;
}
