import {Component, ChangeDetectionStrategy} from '@angular/core';


@Component({
    selector: 'app-custom-event',
    templateUrl: './custom-event.component.html',
    styleUrl: './custom-event.component.css',
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class CustomEventComponent  {

  public activeTab: number = 1;
}
