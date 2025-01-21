import {Component, inject} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastrService} from "ngx-toastr";
import Swal from "sweetalert2";
import {JwtTokenService} from "../../services/jwt-token.service";
import {PlayerService} from "../../services/player.service";

@Component({
  selector: 'app-player-excel-import-modal',
  templateUrl: './player-excel-import-modal.component.html',
  styleUrl: './player-excel-import-modal.component.css'
})
export class PlayerExcelImportModalComponent {

  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _playerService: PlayerService = inject(PlayerService);

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  selectedFile: File | null = null;

  onFileUpload() {
    if (!this.selectedFile) {
      Swal.fire('No file uploaded', 'Please select a file first', 'error').then();
      return;
    }
    this._playerService.uploadPlayerFromExcel(this._tokenService.getAllianceId()!, this.selectedFile).subscribe({
      next: ((response) => {
        if (response) {
          Swal.fire('Uploaded successfully', `${response.addSum} player added, ${response.skipSum} player skipped`, 'success').then(() =>
          this.activeModal.close(response.addSum > 0),);
        }
      }),
      error: (error) => {
        console.log(error);
        if (error.error.name) {
          Swal.fire('Error', error.error.name, 'error').then(() => {this.selectedFile = null;});
        } else {
          this._toastr.error('Could not import players from Excel', 'error');
        }
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (
      file &&
      (file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' ||
        file.type === 'application/vnd.ms-excel')
    ) {
      this.selectedFile = file;
    } else {
      Swal.fire('Incorrect file type', 'Please upload only .xlsx or .xls files.', 'warning').then();
      this.selectedFile = null;
    }
  }

  removeFile() {
    this.selectedFile = null;
  }
}
