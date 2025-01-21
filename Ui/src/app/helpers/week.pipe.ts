import {Pipe, PipeTransform} from '@angular/core';
import moment from "moment";

@Pipe({
  name: 'week'
})
export class WeekPipe implements PipeTransform {

  transform(value: Date, ...args: unknown[]): unknown {
    return moment(value).isoWeek();
  }
}
