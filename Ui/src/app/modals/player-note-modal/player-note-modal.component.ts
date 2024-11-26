import {Component, inject, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {NoteModel} from "../../models/note.model";
import {PlayerModel} from "../../models/player.model";
import {NotesService} from "../../services/notes.service";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import Swal from "sweetalert2";

@Component({
  selector: 'app-player-note-modal',
  templateUrl: './player-note-modal.component.html',
  styleUrl: './player-note-modal.component.css'
})
export class PlayerNoteModalComponent implements OnInit {

  private readonly _noteService: NotesService = inject(NotesService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  public playerNotes: NoteModel[] = [];
  public isCreateMode: boolean = false;
  public isEditMode: boolean = false;

  public noteForm: FormGroup = new FormGroup({
    id: new FormControl<string>(''),
    playerNote: new FormControl<string>('', [Validators.required, Validators.maxLength(500)]),
    playerId: new FormControl<string>(''),
  });

  @Input({required: true}) player!: PlayerModel;

  ngOnInit() {
    this.getPlayerNotes(this.player.id);
  }

  getPlayerNotes(playerId: string) {
    this._noteService.getPlayerNotes(playerId).subscribe({
      next: ((response) => {
        if (response) {
          this.playerNotes = response;
        } else {
          this.playerNotes = [];
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load notes for player', 'Error loading notes');
      })
    });
  }

  onInsertNewNote() {
    this.isCreateMode = true;
    this.noteForm.patchValue({
      playerNote: '',
      playerId: this.player.id,
    });
  }

  onSubmit() {
    if (this.noteForm.invalid) {
      return;
    }
    const note: NoteModel = this.noteForm.value as NoteModel;

    if (this.isCreateMode) {
      this.createNewNote(note);
    }
    if (this.isEditMode) {
      this.updateNote(note);
    }
  }

  onCancel() {
    this.isCreateMode = false;
    this.isEditMode = false;
    this.noteForm.reset();
  }

  createNewNote(note: NoteModel) {
    this._noteService.createNote(note).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully created note', 'Created note');
          this.getPlayerNotes(note.playerId);
          this.onCancel();
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not create new Note', 'Error create note');
      })
    });
  }

  updateNote(note: NoteModel) {
    this._noteService.updateNote(note.id, note).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated note', 'Update note');
          this.getPlayerNotes(note.playerId);
          this.onCancel();
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not update Note', 'Error update note');
      })
    });
  }

  onEditNote(note: NoteModel) {
    this.isEditMode = true;
    this.noteForm.patchValue({
      id: note.id,
      playerNote: note.playerNote,
      playerId: note.playerId
    });
  }

  deleteNote(note: NoteModel) {
    Swal.fire({
      title: "Delete Note ?",
      text: 'Do you really want to delete this note ?',
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._noteService.deleteNote(note.id).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "Note has been deleted",
                icon: "success"
              }).then(_ => {
                this.getPlayerNotes(note.playerId);
              });
            }
          }),
          error: (error: Error) => {
            console.log(error);
            this._toastr.error('Could not delete note', 'Error delete note');
          }
        });
      }
    });
  }
}
