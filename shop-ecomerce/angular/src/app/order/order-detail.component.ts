import { Component, OnInit } from "@angular/core";
import { OrderItemDto, OrdersService } from "@proxy/orders";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";
import { NotificationService } from "../shared/services/notification.service";
import { ProductInListDto, ProductsService } from "@proxy/catalog/products";
import { Subject, takeUntil } from "rxjs";
import { DomSanitizer } from "@angular/platform-browser";
import { ManufacturersService } from "@proxy/catalog/manufacturers";

// OrderDetailComponent or similar
@Component({
    selector: 'app-order-detail',
    templateUrl: './order-detail.component.html'
  })
  export class OrderDetailComponent implements OnInit {
    private ngUnsubscribe = new Subject<void>();
    items: OrderItemDto[] = [];
    products: ProductInListDto[] = [];
    dialogVisible: boolean = true;
    manufacturers: { [key: string]: string } = {};
  
    constructor(
      private productService: ProductsService,
      private orderService: OrdersService,
      public ref: DynamicDialogRef,
      public config: DynamicDialogConfig,
      private notificationService: NotificationService,
      private sanitizer: DomSanitizer,
      private manufacturerService: ManufacturersService
    ) {}
  
    ngOnInit(): void {
      if (this.config.data.id) {
        this.loadOrderItems(this.config.data.id);
      }
    }
  
    // loadOrderItems(orderId: string) {
    //   this.orderService.getOrderItems(orderId).subscribe({
    //     next: (items: OrderItemDto[]) => {
    //       this.items = items;
    //     },
    //     error: () => {
    //       this.notificationService.showError('Failed to load order items.');
    //     }
    //   });
    // }
    loadOrderItems(orderId: string) {
      this.orderService.getOrderItems(orderId).subscribe({
        next: (items: OrderItemDto[]) => {
          this.items = items;
          this.fetchProductDetails();
        },
        error: () => {
          this.notificationService.showError('Failed to load order items.');
        }
      });
    }

    fetchProductDetails() {
      const productIds = this.items.map(item => item.productId).filter((value, index, self) => self.indexOf(value) === index);
      productIds.forEach(id => {
        this.productService.get(id).subscribe({
          next: (productDetail: ProductInListDto) => {
            this.products.push(productDetail);
            this.loadThumbnail(productDetail);
            this.fetchManufacturerDetails(productDetail.manufacturerId);
          },
          error: () => {
            this.notificationService.showError('Failed to load product details.');
          }
        });
      });
    }
    getProductDetail(productId: string): ProductInListDto | undefined {
      return this.products.find(p => p.id === productId);
    }

    loadThumbnail(product: ProductInListDto) {
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

    fetchManufacturerDetails(manufacturerId: string) {
      if (!this.manufacturers[manufacturerId]) {
        this.manufacturerService.get(manufacturerId).subscribe({
          next: (manufacturer) => {
            this.manufacturers[manufacturerId] = manufacturer.name; // Store manufacturer name
          },
          error: () => {
            this.notificationService.showError('Failed to load manufacturer details.');
          }
        });
      }
    }

    getManufacturerName(manufacturerId: string): string | undefined {
      return this.manufacturers[manufacturerId];
    }
  }
  