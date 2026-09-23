import {ChangeDetectionStrategy, Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {AllianceAccessTokenService} from "../../services/alliance-access-token.service";

@Component({
  selector: 'app-create-access-token-modal',
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './create-access-token-modal.component.css',
  templateUrl: './create-access-token-modal.component.html',
})
export class CreateAccessTokenModalComponent implements OnInit {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
  private fb: FormBuilder = inject(FormBuilder);
  private accessTokenService: AllianceAccessTokenService = inject(AllianceAccessTokenService);

  tokenForm!: FormGroup;
  allianceId!: string;

  ngOnInit(): void {
    this.tokenForm = this.fb.group({
      expiresAt: [null]
    })
  }

  onSubmit(): void {
    if (this.tokenForm.valid) {
      const formValue = this.tokenForm.value;

      const createDto = {
        allianceId: this.allianceId,
        expiresAt: formValue.expiresAt ? new Date(formValue.expiresAt).toISOString() : null
      };

      this.accessTokenService.create(createDto.allianceId, createDto.expiresAt).subscribe({
        next: result => {
          if (result) {
            this.activeModal.close(result);
          }
        }
      });
    }
  }

  onCancel(): void {
    this.activeModal.dismiss();
  }
}
