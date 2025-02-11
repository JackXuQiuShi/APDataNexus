import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventoryReturnListPage } from './inventory-return-list.page';

const routes: Routes = [
  {
    path: '',
    component: InventoryReturnListPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryReturnListPageRoutingModule {}
