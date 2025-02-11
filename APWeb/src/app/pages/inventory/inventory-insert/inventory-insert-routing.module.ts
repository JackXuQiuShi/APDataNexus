import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventoryInsertPage } from './inventory-insert.page';

const routes: Routes = [
  {
    path: '',
    component: InventoryInsertPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryInsertPageRoutingModule {}
