import { PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AttributeComponent } from './attribute/attribute.component';
import { ProductComponent } from './product/product.component';
import { CategoryComponent } from './category/category.component';
import { ManufacturerComponent } from './manufacturer/manufacturer.component';

const routes: Routes = [
  {
    path: 'product',
    component: ProductComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminCatalog.Product',
    },
  },
  {
    path: 'manufacturer',
    component: ManufacturerComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminCatalog.Manufacturer',
    },
  },
  {
    path: 'category',
    component: CategoryComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminCatalog.Product',
    },
  },
  {
    path: 'attribute',
    component: AttributeComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminCatalog.Attribute',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}