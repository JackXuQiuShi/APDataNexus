import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplierApprovalPage } from './supplier-approval.page';

const routes: Routes = [
  {
    path: '',
    component: SupplierApprovalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplierApprovalPageRoutingModule {}
