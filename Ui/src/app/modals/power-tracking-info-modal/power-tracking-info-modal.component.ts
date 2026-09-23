import {Component, inject} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-power-tracking-info-modal',
  standalone: false,
  styleUrl: './power-tracking-info-modal.component.css',
  templateUrl: './power-tracking-info-modal.component.html',
})
export class PowerTrackingInfoModalComponent {

  public activeModal: NgbActiveModal = inject(NgbActiveModal);
}
