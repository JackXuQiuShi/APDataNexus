import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventoryReturnPage } from './inventory-return.page';

const routes: Routes = [
  {
    path: '',
    component: InventoryReturnPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryReturnPageRoutingModule {}
