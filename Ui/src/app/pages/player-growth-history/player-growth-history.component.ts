import {Component, Input, OnInit, OnChanges, SimpleChanges, ChangeDetectorRef, inject} from '@angular/core';
import {ChartConfiguration, ChartOptions} from "chart.js";
import {PlayerCombatRecordModel} from "../../models/playerCombatRecord.model";
import { TRANSLATIONS } from '../../helpers/translation';
import {PlayerCombatService} from "../../services/player-combat.service";


@Component({
  selector: 'app-player-growth-history',
  standalone: false,
  styleUrl: './player-growth-history.component.css',
  templateUrl: './player-growth-history.component.html',
})
export class PlayerGrowthHistoryComponent implements OnInit, OnChanges {

  @Input({required: true}) playerId!: string;
  @Input() currentLang: string = 'en';

  private readonly _playerCombatService: PlayerCombatService = inject(PlayerCombatService);

  translations = TRANSLATIONS;

  t(key: string): string {
    return this.translations[this.currentLang]?.[key] || this.translations['en'][key] || key;
  }

  isLoading: boolean = false;
  hasNoData: boolean = false;

  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: { legend: { display: false }, tooltip: { enabled: true } },
    scales: { x: { display: false }, y: { display: false } },
    elements: {
      line: { tension: 0.3 },
      point: { radius: 2, hitRadius: 10, hoverRadius: 5 }
    }
  };

  cardData = {
    squad1: { value: '0.00M', type: '', growth: '+0.00%', color: '#ffc107' },
    squad2: { value: '0.00M', type: '', growth: '+0.00%', color: '#0dcaf0' },
    squad3: { value: '0.00M', type: '', growth: '+0.00%', color: '#ffc107' },
    thp:    { value: '0',     growth: '+0.00%', color: '#0dcaf0' },
    kills:  { value: '0',     growth: '+0.00%', color: '#dc3545' }
  };

  public squad1ChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [{ data: [], borderColor: '#ffc107', backgroundColor: 'rgba(255, 193, 7, 0.1)', fill: true }] };
  public squad2ChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [{ data: [], borderColor: '#0dcaf0', backgroundColor: 'rgba(13, 202, 240, 0.1)', fill: true }] };
  public squad3ChartData: ChartConfiguration<'line'>['data'] = { labels: [], datasets: [{ data: [], borderColor: '#ffc107', backgroundColor: 'rgba(255, 193, 7, 0.1)', fill: true }] };
  public thpChartData: ChartConfiguration<'line'>['data']    = { labels: [], datasets: [{ data: [], borderColor: '#0dcaf0', backgroundColor: 'rgba(13, 202, 240, 0.1)', fill: true }] };
  public killsChartData: ChartConfiguration<'line'>['data']  = { labels: [], datasets: [{ data: [], borderColor: '#dc3545', backgroundColor: 'rgba(220, 53, 69, 0.1)', fill: true }] };

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    if (this.playerId) {
      this.loadPlayerHistory(this.playerId);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['playerId'] && !changes['playerId'].firstChange && this.playerId) {
      this.loadPlayerHistory(this.playerId);
    }

    if (changes['currentLang'] && !changes['currentLang'].firstChange) {
      this.cdr.detectChanges();
    }
  }

  loadPlayerHistory(playerId: string) {
    this.isLoading = true;
    this.hasNoData = false;

    this._playerCombatService.getPlayerRecords(playerId, 10).subscribe({
      next: (result: PlayerCombatRecordModel[]) => {
        if (!result || result.length === 0) {
          console.warn('Keine Daten gefunden für ID:', playerId);
          this.hasNoData = true;
        } else {
          this.processRecordsForCharts(result);
        }

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Fehler beim Laden der Spieler-Historie:', err);
        this.hasNoData = true;
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  private processRecordsForCharts(rawRecords: PlayerCombatRecordModel[]) {
    const records = [...rawRecords].reverse();

    const labels = records.map(r => new Date(r.recordedAtUtc!).toLocaleDateString(undefined, { month: 'short', day: 'numeric' }));


    const squad1Values = records.map(r => r.squad1Power);
    const squad2Values = records.map(r => r.squad2Power);
    const squad3Values = records.map(r => r.squad3Power);
    const thpValues    = records.map(r => r.totalHeroPower);
    const killsValues  = records.map(r => r.kills);


    const latest = records[records.length - 1];

    this.cardData.squad1.value = latest.squad1Power + 'M';
    this.cardData.squad2.value = latest.squad2Power + 'M';
    this.cardData.squad3.value = latest.squad3Power + 'M';
    this.cardData.thp.value    = latest.totalHeroPower + 'M';
    this.cardData.kills.value  = latest.kills + 'M';

    this.cardData.squad1.type  = latest.squad1.toString();
    this.cardData.squad2.type  = latest.squad2.toString();
    this.cardData.squad3.type  = latest.squad3.toString();

    if (records.length === 1) {
      this.cardData.squad1.growth = 'Initial';
      this.cardData.squad2.growth = 'Initial';
      this.cardData.squad3.growth = 'Initial';
      this.cardData.thp.growth    = 'Initial';
      this.cardData.kills.growth  = 'Initial';

      this.updateDataset(this.squad1ChartData, ['Start', labels[0]], [squad1Values[0], squad1Values[0]]);
      this.updateDataset(this.squad2ChartData, ['Start', labels[0]], [squad2Values[0], squad2Values[0]]);
      this.updateDataset(this.squad3ChartData, ['Start', labels[0]], [squad3Values[0], squad3Values[0]]);
      this.updateDataset(this.thpChartData, ['Start', labels[0]], [thpValues[0], thpValues[0]]);
      this.updateDataset(this.killsChartData, ['Start', labels[0]], [killsValues[0], killsValues[0]]);

    } else {
      const previous = records[records.length - 2];

      this.cardData.squad1.growth = this.calculateGrowthPercentage(previous.squad1Power, latest.squad1Power);
      this.cardData.squad2.growth = this.calculateGrowthPercentage(previous.squad2Power, latest.squad2Power);
      this.cardData.squad3.growth = this.calculateGrowthPercentage(previous.squad3Power, latest.squad3Power);
      this.cardData.thp.growth    = this.calculateGrowthPercentage(previous.totalHeroPower, latest.totalHeroPower);
      this.cardData.kills.growth  = this.calculateGrowthPercentage(previous.kills, latest.kills);

      this.updateDataset(this.squad1ChartData, labels, squad1Values);
      this.updateDataset(this.squad2ChartData, labels, squad2Values);
      this.updateDataset(this.squad3ChartData, labels, squad3Values);
      this.updateDataset(this.thpChartData, labels, thpValues);
      this.updateDataset(this.killsChartData, labels, killsValues);
    }
  }

  private calculateGrowthPercentage(oldVal: number, newVal: number): string {
    if (oldVal === 0) return '+0.00%';
    const diff = ((newVal - oldVal) / oldVal) * 100;
    const sign = diff > 0 ? '+' : '';
    return `${sign}${diff.toFixed(2)}%`;
  }

  private updateDataset(chartData: any, labels: string[], data: number[]) {
    chartData.labels = labels;
    chartData.datasets[0].data = data;
  }
}
