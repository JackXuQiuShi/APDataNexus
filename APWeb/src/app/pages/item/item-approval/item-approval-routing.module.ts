import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ItemApprovalPage } from './item-approval.page';

const routes: Routes = [
  {
    path: '',
    component: ItemApprovalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ItemApprovalPageRoutingModule {}
