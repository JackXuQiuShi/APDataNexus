import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WarehouseInPage } from './warehouse-in.page';

const routes: Routes = [
  {
    path: '',
    component: WarehouseInPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WarehouseInPageRoutingModule {}
