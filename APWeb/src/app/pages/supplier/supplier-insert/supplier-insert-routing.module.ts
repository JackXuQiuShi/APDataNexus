import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplierInsertPage } from './supplier-insert.page';

const routes: Routes = [
  {
    path: '',
    component: SupplierInsertPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplierInsertPageRoutingModule {}
