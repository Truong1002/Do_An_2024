import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { OrdersService } from '@proxy/orders';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-home-detail',
  templateUrl: './home-detail.component.html',
  styleUrls: ['./home-detail.component.scss']
})
export class HomeDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  public chartData: any;
  public chartOptions: any;
  public selectedYear: number;
  public years: number[] = [];

  constructor(
    private orderService: OrdersService,
    public config: DynamicDialogConfig,
    public ref: DynamicDialogRef
  ) {}

  ngOnInit(): void {
    this.populateYears();
    this.selectedYear = this.config.data.year || new Date().getFullYear();
    this.loadMonthlyRevenueData();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  populateYears() {
    const currentYear = new Date().getFullYear();
    for (let year = 2020; year <= currentYear; year++) {
      this.years.push(year);
    }
  }

  loadMonthlyRevenueData() {
    this.orderService
      .getMonthlyRevenueStatisticsForYear(this.selectedYear)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          this.prepareMonthlyRevenueChartData(response);
        },
        error: () => {
          // Handle error
        },
      });
  }

  prepareMonthlyRevenueChartData(data: any) {
    const labels = data.map(item => item.month);
    const revenues = data.map(item => item.totalRevenue);

    this.chartData = {
      labels: labels,
      datasets: [
        {
          label: 'Tổng doanh thu',
          data: revenues,
          backgroundColor: '#42A5F5',
          type: 'bar'
        }
      ]
    };

    this.chartOptions = {
      responsive: true,
      scales: {
        x: {
          scaleLabel: {
            display: true,
            labelString: 'Tháng'
          }
        },
        y: {
          scaleLabel: {
            display: true,
            labelString: 'Tổng doanh thu ($)'
          },
          ticks: {
            beginAtZero: true
          }
        }
      }
    };
  }

  onYearChange(event: any) {
    this.loadMonthlyRevenueData();
  }
}
