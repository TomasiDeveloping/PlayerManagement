import {Component, inject, Input} from '@angular/core';
import { AgChartOptions } from "ag-charts-community";
import {DatePipe} from "@angular/common";
import {VsDuelParticipantService} from "../../../services/vs-duel-participant.service";
import {ToastrService} from "ngx-toastr";


@Component({
  selector: 'app-player-info-vs-duel',
  templateUrl: './player-info-vs-duel.component.html',
  styleUrl: './player-info-vs-duel.component.css',
  providers: [DatePipe]
})
export class PlayerInfoVsDuelComponent {

  @Input({required: true}) playerId!: string;

  private readonly _vsDuelParticipantsService: VsDuelParticipantService = inject(VsDuelParticipantService);
  private readonly _datePipe: DatePipe = inject(DatePipe);
  private readonly _toastr: ToastrService = inject(ToastrService);

  numberOfLoadVsDuels: number = 10;
  vsDuelsLoaded: boolean = false;


  options: AgChartOptions = {
    title: {
      text: 'Weekly Points'
    },
    data: [],
    series: [
      {
        type: 'bar',
        xKey: 'date',
        xName: 'Date',
        yKey: 'points',
        yName: 'Weekly Points',
        stacked: false,
        fill: 'blue'
      }
    ],
    axes: [
      {
        type: 'number',
        position: 'left',
        label: {
          formatter: (params: any) => {
            return params.value.toLocaleString('en-US')
          }
        }
      },
      {
        type: 'category',
        position: 'bottom',
      }
    ]
  }

  getData(take: number) {
    const chartData: {date: string, points: number}[] = [];
    this._vsDuelParticipantsService.getVsDuelParticipantsDetail(this.playerId, take).subscribe({
      next: ((response) => {
        if (response) {
          this.vsDuelsLoaded = true;
          if (response.length < take) {
            this._toastr.info('Fewer vs duels were held than wanted to be loaded');
            this.numberOfLoadVsDuels = response.length;
          }
          response.forEach(player => {
            const data: {date: string, points: number} = {
              date: this._datePipe.transform(player.eventDate, 'dd.MM.yyyy')!,
              points: player.weeklyPoints
            };
            chartData.push(data)
          })
          this.options = {
            ...this.options,
            data: chartData.reverse()
          }
        }
      }),
      error: error => {
        console.log(error);
      }
    })
  }


  onReloadVsDuels() {
    this.getData(this.numberOfLoadVsDuels);
  }
}
