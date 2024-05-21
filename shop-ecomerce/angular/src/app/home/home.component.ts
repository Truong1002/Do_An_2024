import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService, PagedResultDto } from '@abp/ng.core';
import { OrdersService, ProductSalesDto, ProductSalesTimeDto } from '@proxy/orders';
import { Subject, takeUntil } from 'rxjs';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ProductsService } from '@proxy/catalog/products';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { formatDate } from '@angular/common';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { HomeDetailComponent } from './home-detail.component';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {

  private ngUnsubscribe = new Subject<void>();
  public items: ProductSalesTimeDto[] = [];
  public blockedPanel: boolean = false; // Để hiển thị trạng thái loading

  // Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;
  // Filter
  keyword: string = '';

  // Date range filter
  startDate: string;
  endDate: string;

  // Image
  public thumbnailImage;
  public thumbnailImageUrl: SafeUrl;

  public chartData: any;
  public chartOptions: any;

  totalQuantitySold: number = 0;
  totalRevenue: number = 0;

  // Year selection
  selectedYear: number = new Date().getFullYear();
  years: number[] = [];

  ref: DynamicDialogRef;


  constructor(
    private authService: AuthService,
    private productService: ProductsService,
    private orderService: OrdersService, // Inject OrdersService
    private sanitizer: DomSanitizer,
    private dialogService: DialogService,
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    if (this.ref) {
      this.ref.close();
    }
  }

  ngOnInit(): void {
    this.setCurrentMonthDates();
    this.loadData();
    this.populateYears();
  }

  populateYears() {
    const currentYear = new Date().getFullYear();
    for (let year = 2020; year <= currentYear; year++) {
      this.years.push(year);
    }
  }

  prepareChartData() {
    const labels = this.items.map(item => item.productName);
    const revenues = this.items.map(item => item.totalRevenue);
    const quantities = this.items.map(item => item.quantitySold);
  
    this.chartData = {
      labels: labels,
      datasets: [
        {
          label: 'Tổng tiền',
          data: revenues,
          backgroundColor: '#42A5F5',
          yAxisID: 'y-axis-revenue',
          type: 'bar'
        },
        {
          label: 'Số lượng bán được',
          data: quantities,
          borderColor: '#FFA726',
          backgroundColor: '#FFA726',
          yAxisID: 'y-axis-quantity',
          type: 'line',
          fill: false
        }
      ]
    };
  
    this.chartOptions = {
      responsive: true,
      scales: {
        x: { // Cập nhật từ 'xAxes' sang 'x'
          scaleLabel: {
            display: true,
            labelString: 'Products'
          }
        },
        y: { // Cập nhật từ 'yAxes' sang 'y'
          'y-axis-revenue': {
            type: 'linear',
            position: 'left',
            scaleLabel: {
              display: true,
              labelString: 'Total Revenue ($)'
            },
            ticks: {
              beginAtZero: true
            }
          },
          'y-axis-quantity': {
            type: 'linear',
            position: 'right',
            scaleLabel: {
              display: true,
              labelString: 'Quantity Sold'
            },
            ticks: {
              beginAtZero: true
            }
          }
        }
      }
    };
  }

  login() {
    this.authService.navigateToLogin(); // Chuyển hướng người dùng đến trang đăng nhập
  }

  loadData() {
    this.toggleBlockUI(true);
    this.orderService
      .getProductSalesStatisticsByTime(
        {
          keyword: this.keyword,
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
        },
        this.startDate,
        this.endDate
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PagedResultDto<ProductSalesTimeDto>) => {
          this.items = response.items;
          this.totalCount = response.totalCount;
          this.items.forEach(product => {
            if (product.thumbnailPicture) {
                this.loadThumbnail(product);
            }
          });
          this.calculateTotals();
          this.prepareChartData();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  private toggleBlockUI(enabled: boolean) {
    this.blockedPanel = enabled;
    if (!enabled) {
      setTimeout(() => this.blockedPanel = false, 1000); // Đảm bảo rằng UI sẽ được mở khóa sau một khoảng thời gian
    }
  }

  pageChanged(event: any): void {
    this.skipCount = event.first;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  loadThumbnail(product: ProductSalesDto) {
    if (!product.thumbnailPicture) {
      console.log('No thumbnail available for this product.');
      return;
    }
  
    this.productService.getThumbnailImage(product.thumbnailPicture)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: string) => {
          const fileExt = product.thumbnailPicture.split('.').pop();
          product.safeThumbnailUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
            `data:image/${fileExt};base64,${response}`
          );
        },
        error: () => {
          console.error(`Failed to load thumbnail for ${product.thumbnailPicture}`);
          product.safeThumbnailUrl = undefined; // Optionally set a default image in case of error
        }
      });
  }

  resetForm() {
    // Reset search/filter fields
    this.keyword = '';
    this.maxResultCount = 10;  // Giả sử đây là số lượng mặc định bạn muốn tải ban đầu
    this.skipCount = 0;        // Reset về trang đầu tiên
    this.startDate = undefined;
    this.endDate = undefined;

    this.loadData();
  }

  calculateTotals() {
    this.totalQuantitySold = this.items.reduce((acc, item) => acc + item.quantitySold, 0);
    this.totalRevenue = this.items.reduce((acc, item) => acc + item.totalRevenue, 0);
  }

  exportToExcel() {
    // Tạo dữ liệu từ các mục và thêm tổng doanh thu và tổng số lượng bán
    const data = this.items.map(item => ({
      'Tên sản phẩm': item.productName,
      'Tên nhà sản xuất': item.manufacturerName,
      'Số lượng bán': item.quantitySold,
      'Số tiền Discount': item.discount,
      'Tổng tiền chưa giảm': item.total,
      'Tổng tiền': item.totalRevenue
    }));
    const totalDiscount = this.items.reduce((acc, item) => acc + item.discount, 0);
    const totalT = this.items.reduce((acc, item) => acc + item.total, 0);
    // Thêm hàng tổng vào dữ liệu
    data.push({
      'Tên sản phẩm': 'Tổng',
      'Tên nhà sản xuất': '',
      'Số lượng bán': this.totalQuantitySold,
      'Số tiền Discount': totalDiscount,
      'Tổng tiền chưa giảm': totalT,
      'Tổng tiền': this.totalRevenue
    });
  
    // Chuyển đổi dữ liệu thành bảng tính
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
  
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'SalesData');
  
    // Tạo tệp Excel
    const wbout: Blob = new Blob([XLSX.write(wb, { bookType: 'xlsx', type: 'array' })], {
      type: 'application/octet-stream'
    });
  
    // Tải tệp xuống
    saveAs(wbout, 'SalesData.xlsx');
  }
  private setCurrentMonthDates(): void {
    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
    const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0); // Set day to 0 to get last day of current month
  
    // Format dates to 'YYYY-MM-DD' if your backend requires this format
    this.startDate = formatDate(startOfMonth, 'yyyy-MM-dd', 'en-US');
    this.endDate = formatDate(endOfMonth, 'yyyy-MM-dd', 'en-US');
  }

  openDetailChart() {
    this.ref = this.dialogService.open(HomeDetailComponent, {
      header: 'Biểu đồ doanh thu theo tháng',
      width: '80%',
      contentStyle: {"max-height": "100%", "overflow": "auto"},
      baseZIndex: 10000,
      data: {
        year: this.selectedYear
      }
    });
  }

  // loadMonthlyRevenueData() {
  //   this.toggleBlockUI(true);
  //   this.orderService
  //     .getMonthlyRevenueStatisticsForYear(this.selectedYear)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response: any) => {
  //         this.prepareMonthlyRevenueChartData(response);
  //         this.toggleBlockUI(false);
  //       },
  //       error: () => {
  //         this.toggleBlockUI(false);
  //       },
  //     });
  // }
  // prepareMonthlyRevenueChartData(data: any) {
  //   const labels = data.map(item => item.month);
  //   const revenues = data.map(item => item.totalRevenue);

  //   this.chartData = {
  //     labels: labels,
  //     datasets: [
  //       {
  //         label: 'Tổng doanh thu',
  //         data: revenues,
  //         backgroundColor: '#42A5F5',
  //         type: 'bar'
  //       }
  //     ]
  //   };

  //   this.chartOptions = {
  //     responsive: true,
  //     scales: {
  //       x: {
  //         scaleLabel: {
  //           display: true,
  //           labelString: 'Tháng'
  //         }
  //       },
  //       y: {
  //         scaleLabel: {
  //           display: true,
  //           labelString: 'Tổng doanh thu ($)'
  //         },
  //         ticks: {
  //           beginAtZero: true
  //         }
  //       }
  //     }
  //   };
  // }

}
