import {Component, inject, Input, OnInit} from '@angular/core';
import Swal from "sweetalert2";
import {ApiKeyModel, CreateApiKeyModel, UpdateApiKeyModel} from "../../../models/apiKey.model";
import {ApiKeyService} from "../../../services/api-key.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-alliance-api-key',
  templateUrl: './alliance-api-key.component.html',
  styleUrl: './alliance-api-key.component.css'
})
export class AllianceApiKeyComponent implements OnInit {

  private readonly _apiKeyService: ApiKeyService = inject(ApiKeyService);
  private readonly _toastr: ToastrService = inject(ToastrService);

  @Input('allianceId') allianceId!: string;

  public isKeyVisible: any;
  public copied = false;
  public isIdVisible: boolean = false;
  public apiKey: ApiKeyModel | undefined;


  ngOnInit() {
    this.getAllianceApiKey(this.allianceId);
  }

  toggleKeyVisibility() {
    this.isKeyVisible = !this.isKeyVisible;
  }

  getAllianceApiKey(allianceId: string) {
    this._apiKeyService.getAllianceApiKey(allianceId).subscribe({
      next: ((response) => {
        if (response) {
          this.apiKey = response;
        }
      }),
      error: (error) => {
        this.apiKey = undefined;
        console.log(error);
      }
    })
  }

  regenerateApiKey(apiKeyId: string) {
    Swal.fire({
      title: "Regenerate API Key?",
      text: "Are you sure you want to generate a new API Key? Your current key will be invalidated, and you will need to use the new key for API access.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, regenerate it!"
    }).then((result) => {
      if (result.isConfirmed) {
        const updateApiKey: UpdateApiKeyModel = {
          allianceId: this.allianceId!,
          id: apiKeyId
        };
        this._apiKeyService.updateApiKey(apiKeyId, updateApiKey).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "API Key Regenerated!",
                text: "A new API Key has been successfully generated. Please update your applications with the new key.",
                icon: "success",
              }).then(_ => this.getAllianceApiKey(this.allianceId!));
            }
          }),
          error: (error: Error) => {
            console.log(error);
            Swal.fire({
              title: "Error!",
              text: "Something went wrong while regenerating the API Key. Please try again later.",
              icon: "error",
            }).then();
          }
        });
      }
    });
  }

  deleteApiKey(apiKeyId: string) {
    Swal.fire({
      title: "Delete API Key ?",
      text: "Are you sure you want to delete this API Key? Once deleted, you will no longer be able to access the API without a new key.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, I understand – delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this._apiKeyService.deleteApiKey(apiKeyId).subscribe({
          next: ((response) => {
            if (response) {
              Swal.fire({
                title: "Deleted!",
                text: "The API Key has been successfully deleted. You will need to generate a new key to access the API.",
                icon: "success",
              }).then(_ => this.getAllianceApiKey(this.allianceId!));
            }
          }),
          error: (error: Error) => {
            console.log(error);
          }
        });
      }
    });
  }

  generateApiKey() {
    const createApiKeyModel: CreateApiKeyModel = {
      allianceId: this.allianceId!,
    }
    this._apiKeyService.createApiKey(createApiKeyModel).subscribe({
      next: ((response) => {
        if (response) {
          this._toastr.success('Successfully generated api key!');
          this.apiKey = response;
        }
      }),
      error: (error: Error) => {
        console.log(error);
        this._toastr.error('Failed to generate api key', 'Generate api key');
      }
    })
  }

  toggleIdVisibility() {
    this.isIdVisible = !this.isIdVisible;
  }

  copyAllianceId() {
    navigator.clipboard.writeText(this.allianceId!)
      .then(() => {
        this.copied = true;
        this._toastr.info('Alliance id copied to clipboard!');
        setTimeout(() => this.copied = false, 2000);
      })
      .catch(err => {
        console.error(err);
        this._toastr.error('Failed to copy Alliance id.');
      });
  }

  copyApiKey(): void {
    navigator.clipboard.writeText(this.apiKey!.key)
      .then(() => {
        this.copied = true;
        this._toastr.info('API key copied to clipboard!');
        setTimeout(() => this.copied = false, 2000);
      })
      .catch(err => {
        console.error(err);
        this._toastr.error('Failed to copy API key.');
      });
  }
}
