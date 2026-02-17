import {Pipe, PipeTransform} from '@angular/core';
import moment from "moment";

@Pipe({
    name: 'week',
    standalone: false
})
export class WeekPipe implements PipeTransform {

  transform(value: Date, ...args: unknown[]): unknown {
    return moment(value).isoWeek();
  }
}
