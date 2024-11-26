import {inject, Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {NoteModel} from "../models/note.model";

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  private readonly _serviceUrl = environment.apiBaseUrl + 'Notes/';
  private readonly _httpClient: HttpClient = inject(HttpClient);

  public getNote(notedId: string): Observable<NoteModel> {
    return this._httpClient.get<NoteModel>(this._serviceUrl + notedId);
  }

  public getPlayerNotes(playerId: string): Observable<NoteModel[]> {
    return this._httpClient.get<NoteModel[]>(this._serviceUrl + 'Player/' + playerId);
  }

  public createNote(note: NoteModel): Observable<NoteModel> {
    return this._httpClient.post<NoteModel>(this._serviceUrl, note);
  }

  public updateNote(noteId: string, note: NoteModel): Observable<NoteModel> {
    return this._httpClient.put<NoteModel>(this._serviceUrl + noteId, note);
  }

  public deleteNote(noteId: string): Observable<boolean> {
    return this._httpClient.delete<boolean>(this._serviceUrl + noteId);
  }
}
