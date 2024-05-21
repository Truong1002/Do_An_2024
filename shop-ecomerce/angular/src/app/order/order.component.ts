import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '../shared/services/notification.service';
import { OrderDto, OrderItemDto, OrdersService } from '@proxy/orders';
import { OrderStatus, PaymentMethod } from '@proxy/shop-ecommerce/orders';
import { OrderDetailComponent } from './order-detail.component';


@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: OrderDto[] = [];
  public selectedItems: OrderDto[] = [];
  selectedOrderItems: OrderItemDto[] = [];
  dialogVisible: boolean = false;

  // Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  // Filter
  keyword: string = '';

  constructor(
    private orderService: OrdersService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.toggleBlockUI(true);
    this.orderService
      .getListFilter({
        keyword: this.keyword,
        maxResultCount: this.maxResultCount,
        skipCount: this.skipCount,
      })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PagedResultDto<OrderDto>) => {
          this.items = response.items;
          this.totalCount = response.totalCount;
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  pageChanged(event: any): void {
    this.skipCount = event.first;
    this.maxResultCount = event.rows;
    this.loadData();
  }

  
  
  deleteItems(){
    if(this.selectedItems.length == 0){
      this.notificationService.showError("Phải chọn ít nhất một bản ghi");
      return;
    }
    var ids =[];
    this.selectedItems.forEach(element=>{
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message:'Bạn có chắc muốn xóa bản ghi này?',
      accept:()=>{
        this.deleteItemsConfirmed(ids);
      }
    })
  }

  deleteItemsConfirmed(ids: string[]){
    this.toggleBlockUI(true);
    this.orderService.deleteMultiple(ids).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
      next: ()=>{
        this.notificationService.showSuccess("Xóa thành công");
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error:()=>{
        this.toggleBlockUI(false);
      }
    })
  }

  getOrderStatusName(value:number ){
    return OrderStatus[value];
  }

  getOrderMethod(value:number){
    return PaymentMethod[value];
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }

  
  
  confirmOrder(order: any) {
    // Thực hiện gửi yêu cầu xác nhận đơn hàng đến API
    this.orderService.confirmOrder(order.id).subscribe({
      next: () => {
        // Nếu thành công, thông báo cho người dùng và cập nhật trạng thái
        order.status = 'Confirmed';
        this.getOrderStatusName(order.status);
        this.loadData();
        this.notificationService.showSuccess('Đơn hàng đã được xác nhận thành công.');
      },
      error: () => {
        // Xử lý lỗi nếu có
        this.notificationService.showError('Đã xảy ra lỗi khi xác nhận đơn hàng.');
      }
    });
  }

  showViewModal() {
    if (this.selectedItems.length === 0) {
      this.notificationService.showError('Bạn phải chọn một đơn hàng');
      return;
    }
    const id = this.selectedItems[0].id; // Assuming `selectedItems` stores selected orders
  
    const ref = this.dialogService.open(OrderDetailComponent, { // Change to your component that shows order details
      data: {
        id: id,
      },
      header: 'Chi tiết đơn hàng',
      width: '70%',
    });
  
    ref.onClose.subscribe(() => {
      // Actions on close, if necessary
    });
  }
  
  
  

}
