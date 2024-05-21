import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
  selector: 'app-menu',
  templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
  model: any[] = [];

  constructor(public layoutService: LayoutService) {}

  ngOnInit() {
    this.model = [
      {
        label: 'Trang chủ',
        items: [
          { label: 'Dashboard',
           icon: 'pi pi-fw pi-home',
            routerLink: ['/'],
            permission: 'ShopEcomAdminOrder.Order', 
          }
        ],
      },
      {
        label: 'Đơn hàng',
        items: [
          { 
            label: 'Danh sách đơn hàng',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/order'], 
            permission: 'ShopEcomAdminOrder.Order',
          }, 
          
          ],
      },
      {
        label: 'Sản phẩm',
        items: [
          {
            label: 'Danh sách sản phẩm',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/catalog/product'],
            permission: 'ShopEcomAdminCatalog.Product',
          },
          {
            label: 'Danh sách nhà sản xuất',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/catalog/manufacturer'],
            permission: 'ShopEcomAdminCatalog.Manufacturer',
          },
          {
            label: 'Danh sách loại sản phẩm',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/catalog/category'],
            permission: 'ShopEcomAdminCatalog.Category',
          },
          {
            label: 'Danh sách thuộc tính',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/catalog/attribute'],
            permission: 'ShopEcomAdminCatalog.Attribute',
          },
        ],
      },
      {
        label: 'Hệ thống',
        items: [
          {
            label: 'Danh sách quyền',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/system/role'],
            permission: 'AbpIdentity.Roles',
          },
          {
            label: 'Danh sách người dùng',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/system/user'],
            permission: 'AbpIdentity.Users',
          },
        ],
      },
      {
        label: 'Phiếu giảm giá',
        items: [
          { 
            label: 'Danh sách phiếu giảm giá',
            icon: 'pi pi-fw pi-circle',
            routerLink: ['/promotion'], 
           
          }, 
          
          ],
      },
    ];
  }
}