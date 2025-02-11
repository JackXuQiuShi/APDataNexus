import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventoryLocationPage } from './inventory-location.page';

const routes: Routes = [
  {
    path: '',
    component: InventoryLocationPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryLocationPageRoutingModule {}
