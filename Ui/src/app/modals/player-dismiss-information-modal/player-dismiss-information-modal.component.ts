import {Component, inject, Input, OnInit} from '@angular/core';
import { DismissPlayerInformationModel} from "../../models/player.model";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {PlayerService} from "../../services/player.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-player-dismiss-information-modal',
  templateUrl: './player-dismiss-information-modal.component.html',
  styleUrl: './player-dismiss-information-modal.component.css'
})
export class PlayerDismissInformationModalComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  public dismissPlayerInformation: DismissPlayerInformationModel | undefined;
  public activeModal: NgbActiveModal = inject(NgbActiveModal);

  @Input({required: true}) playerId!: string;

  ngOnInit() {
    this.getDismissPlayerInformation();
  }

  getDismissPlayerInformation() {
    this._playerService.getDismissPlayerInformation(this.playerId).subscribe({
      next: ((response) => {
        if (response) {
          this.dismissPlayerInformation = response;
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not get dismissPlayerInformation', 'Get Player Information');
      })
    });
  }
}
