import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplierUpdatePage } from './supplier-update.page';

const routes: Routes = [
  {
    path: '',
    component: SupplierUpdatePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplierUpdatePageRoutingModule {}
