import {Component, inject, OnInit} from '@angular/core';
import {AllianceService} from "../../services/alliance.service";
import {AllianceModel} from "../../models/alliance.model";
import {ToastrService} from "ngx-toastr";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {JwtTokenService} from "../../services/jwt-token.service";


@Component({
  selector: 'app-alliance',
  templateUrl: './alliance.component.html',
  styleUrl: './alliance.component.css'
})
export class AllianceComponent implements OnInit {


  private readonly _allianceService: AllianceService = inject(AllianceService);
  private readonly _toastr: ToastrService = inject(ToastrService);
  private readonly _tokenService: JwtTokenService = inject(JwtTokenService);

  public allianceId = this._tokenService.getAllianceId();

  public allianceForm: FormGroup | undefined;
  public currentAlliance: AllianceModel | undefined;
  public activeTab: number = 1;


  get f() {
    return this.allianceForm!.controls;
  }

  ngOnInit() {
    this.getAlliance(this.allianceId!);
  }

  getAlliance(allianceId: string) {
    this._allianceService.getAlliance(allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.currentAlliance = response;
          this.createAllianceForm(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Could not load alliance', 'Error load alliance');
      })
    });
  }

  createAllianceForm(alliance: AllianceModel) {
    this.allianceForm = new FormGroup({
      id: new FormControl<string>(alliance.id),
      server: new FormControl<number>(alliance.server, [Validators.required]),
      name: new FormControl<string>(alliance.name, [Validators.required, Validators.maxLength(200)]),
      abbreviation: new FormControl<string>(alliance.abbreviation, [Validators.required, Validators.maxLength(4)]),
    });
  }

  onSubmit() {
    if (this.allianceForm?.invalid) {
      return;
    }

    const alliance: AllianceModel = this.allianceForm?.value;

    this._allianceService.updateAlliance(alliance.id, alliance).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully updated alliance', 'Update');
          this.currentAlliance = response;
          this.createAllianceForm(response);
        }
      }),
      error: ((error) => {
        console.log(error);
        this._toastr.error('Failed to update alliance', 'Update failed');
        this.getAlliance(alliance.id);
      })
    });
  }

}
