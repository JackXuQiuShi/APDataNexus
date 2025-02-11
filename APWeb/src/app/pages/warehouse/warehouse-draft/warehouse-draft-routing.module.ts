import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WarehouseDraftPage } from './warehouse-draft.page';

const routes: Routes = [
  {
    path: '',
    component: WarehouseDraftPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WarehouseDraftPageRoutingModule {}
