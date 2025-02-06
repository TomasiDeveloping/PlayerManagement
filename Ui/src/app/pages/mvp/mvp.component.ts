import {Component, inject, OnInit} from '@angular/core';
import {PlayerService} from "../../services/player.service";
import {JwtTokenService} from "../../services/jwt-token.service";
import {ToastrService} from "ngx-toastr";
import {PlayerMvpModel} from "../../models/player.model";


@Component({
  selector: 'app-mvp',
  templateUrl: './mvp.component.html',
  styleUrl: './mvp.component.css'
})
export class MvpComponent implements OnInit {

  private readonly _playerService: PlayerService = inject(PlayerService);
  private readonly _jwtTokenService: JwtTokenService = inject(JwtTokenService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  private readonly _allianceId: string = this._jwtTokenService.getAllianceId()!;


  public mvpPlayers: PlayerMvpModel[] = [];
  selectedFilter: string = 'players';
  showInfo: boolean = false;

  ngOnInit() {
    this.getMvpPlayerList(this.selectedFilter);
  }

  getMvpPlayerList(playerType: string) {
    this._playerService.getAllianceMvpPlayers(this._allianceId, playerType).subscribe({
      next: ((response) => {
        if (response) {
          this.mvpPlayers = response;
        } else {
          this.mvpPlayers = [];
        }
      }),
      error: (error) => {
        this.mvpPlayers = [];
        console.error(error);
        this._toastr.error('Error getting mvp players', 'Error');
      }
    });
  }

  onSelectChange() {
    this.getMvpPlayerList(this.selectedFilter);
  }

  toggleInfo() {
    this.showInfo = !this.showInfo;
  }
}
