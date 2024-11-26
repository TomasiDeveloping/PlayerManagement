import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'week'
})
export class WeekPipe implements PipeTransform {

  transform(value: Date, ...args: unknown[]): unknown {
    const d = new Date(value);
    let yearStart = +new Date(d.getFullYear(), 0, 1);
    let today = +new Date(d.getFullYear(),d.getMonth(),d.getDate());
    let dayOfYear = ((today - yearStart + 1) / 86400000);
    return Math.ceil(dayOfYear / 7);
  }

}
