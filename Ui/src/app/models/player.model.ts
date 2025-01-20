import {NoteModel} from "./note.model";
import {AdmonitionModel} from "./admonition.model";
import {DesertStormParticipantModel} from "./desertStormParticipant.model";
import {MarshalGuardParticipantModel} from "./marshalGuardParticipant.model";
import {VsDuelParticipantModel} from "./vsDuelParticipant.model";

export interface PlayerModel {
  id: string;
  playerName: string;
  level: number;
  rankName: string;
  rankId: string;
  allianceId: string;
  createdOn: Date;
  createdBy: string;
  modifiedOn?: Date;
  modifiedBy?: string;
  notesCount: number;
  admonitionsCount: number;
  isDismissed: boolean;
  dismissedAt?: Date;
  dismissalReason?: string;
}

export interface CreatePlayerModel {
  playerName: string;
  rankId: string;
  allianceId: string;
  level: number;
}

export interface UpdatePlayerModel {
  id: string;
  playerName: string;
  rankId: string;
  level: number;
}

export interface DismissPlayerInformationModel {
  id: string;
  playerName: string;
  dismissedAt: Date;
  dismissalReason: string;
  notes: NoteModel[],
  admonitions: AdmonitionModel[],
  desertStormParticipants: {eventDate: Date, participated: boolean}[],
  marshalGuardParticipants: {eventDate: Date, participated: boolean}[],
  vsDuelParticipants: {eventDate: Date, weeklyPoints: number}[],
}
