import {ChangeDetectionStrategy, Component, inject, Input} from '@angular/core';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-power-tracking-records-modal',
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './power-tracking-records-modal.component.css',
  templateUrl: './power-tracking-records-modal.component.html',
})
export class PowerTrackingRecordsModalComponent {

  @Input() playerId!: string;
  @Input() playerName!: string;

  public activeModal: NgbActiveModal = inject(NgbActiveModal);

}
