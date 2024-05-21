import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderComponent } from './order.component';
import { PermissionGuard } from '@abp/ng.core';


const routes: Routes = 
[{ 
  path: '', 
component: OrderComponent ,
canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminOrder.Order',
    },

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrderRoutingModule {}