import { PagedResultDto } from '@abp/ng.core';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';

import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { ProductType } from '@proxy/shop-ecommerce/products';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ConfirmationService } from 'primeng/api';
import { ProductCategoriesService, ProductCategoryDto, ProductCategoryInListDto } from '@proxy/catalog/product-categories';
import { ProductDto } from '@proxy/catalog/products';
import { NotificationService } from '../shared/services/notification.service';
import { PromotionDto, PromotionService } from '@proxy/promotions';
import { PromotionDetailComponent } from './promotion-detail.component';



@Component({
  selector: 'app-promotion',
  templateUrl: './promotion.component.html',
  styleUrls: ['./promotion.component.scss'],
  
})
export class PromotionComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: PromotionDto[] = [];
  public selectedItems: PromotionDto[] = [];

  //Image
  public thumbnailImage;
  selectedEntity = {} as ProductDto;
  public thumbnailImageUrl: SafeUrl;
  products: any[] =[];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;
  

  //Filter
  productCategories: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  constructor(
    private promotionService: PromotionService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private cd: ChangeDetectorRef,
    private sanitizer: DomSanitizer,
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
    this.promotionService
      .getListFilter({
        keyword: this.keyword,
        maxResultCount: this.maxResultCount,
        skipCount: this.skipCount,
      })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PagedResultDto<PromotionDto>) => {
          this.items = response.items;
          this.totalCount = response.totalCount;
            // Tải thumbnail cho từng sản phẩm
          //   this.items.forEach(product => {
          //     if (product.thumbnailPicture) {
          //         this.loadThumbnail(product);
          //     }
          // });
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
  showAddModal() {
    const ref = this.dialogService.open(PromotionDetailComponent, {
      header: 'Thêm mới mã giảm giá',
      width: '70%',
    });

    ref.onClose.subscribe((data: PromotionDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Thêm mã giảm giá thành công');
        this.selectedItems = [];
      }
    });
  }

  showEditModal() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError('Bạn phải chọn một bản ghi');
      return;
    }
    const id = this.selectedItems[0].id;
    const ref = this.dialogService.open(PromotionDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật mã giảm giá',
      width: '70%',
    });

    ref.onClose.subscribe((data: PromotionDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật mã giảm giá thành công');
      }
    });
  }

  deleteItems(){
    if(this.selectedItems.length == 0){
      this.notificationService.showError('Phải chọn ít nhất 1 bản ghi');
      return;
    }
    var ids =[];
    this.selectedItems.forEach(element=>{
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message:"Bạn có chắc muốn xóa bản ghi này?",
      accept:()=>{
        this.deleteItemsConfirmed(ids);
      }
    })
  }

  deleteItemsConfirmed(ids: string[]){
    this.toggleBlockUI(true);
    this.promotionService.deleteMultiple(ids).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
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

  getProductTypeName(value:number ){
    return ProductType[value];
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

  resetForm() {
    // Reset search/filter fields
    this.keyword = '';
    this.categoryId = null;
    this.maxResultCount = 10;  // Giả sử đây là số lượng mặc định bạn muốn tải ban đầu
    this.skipCount = 0;        // Reset về trang đầu tiên

    this.loadData();
}
  
  // loadThumbnail(product: ProductInListDto) {
  //   if (!product.thumbnailPicture) {
  //     console.log('No thumbnail available for this product.');
  //     return;
  //   }
  
  //   this.productService.getThumbnailImage(product.thumbnailPicture)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response: string) => {
  //         const fileExt = product.thumbnailPicture.split('.').pop();
  //         product.safeThumbnailUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
  //           `data:image/${fileExt};base64,${response}`
  //         );
  //       },
  //       error: () => {
  //         console.error(`Failed to load thumbnail for ${product.thumbnailPicture}`);
  //         product.safeThumbnailUrl = undefined; // Optionally set a default image in case of error
  //       }
  //     });
  // }
  
}