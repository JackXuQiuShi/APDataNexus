import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WarehouseInventoryPage } from './warehouse-inventory.page';

const routes: Routes = [
  {
    path: '',
    component: WarehouseInventoryPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WarehouseInventoryPageRoutingModule {}
