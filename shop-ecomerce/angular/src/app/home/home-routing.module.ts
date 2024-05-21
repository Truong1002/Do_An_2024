import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { PermissionGuard } from '@abp/ng.core';

const routes: Routes = 
[{ 
  path: '', 
component: HomeComponent ,
canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'ShopEcomAdminOrder.Order',
    },

}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
