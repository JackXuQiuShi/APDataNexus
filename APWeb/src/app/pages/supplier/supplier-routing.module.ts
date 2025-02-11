import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplierPage } from './supplier.page';

const routes: Routes = [
  {
    path: '',
    component: SupplierPage
  },
  {
    path: 'supplier-insert',
    loadChildren: () => import('./supplier-insert/supplier-insert.module').then( m => m.SupplierInsertPageModule)
  },
  {
    path: 'supplier-update',
    loadChildren: () => import('./supplier-update/supplier-update.module').then( m => m.SupplierUpdatePageModule)
  },
  {
    path: 'supplier-approval',
    loadChildren: () => import('./supplier-approval/supplier-approval.module').then( m => m.SupplierApprovalPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplierPageRoutingModule {}
