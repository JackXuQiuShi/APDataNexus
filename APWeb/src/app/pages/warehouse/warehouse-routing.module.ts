import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WarehousePage } from './warehouse.page';

const routes: Routes = [
  {
    path: '',
    component: WarehousePage
  },
  {
    path: 'warehouse-in',
    loadChildren: () => import('./warehouse-in/warehouse-in.module').then( m => m.WarehouseInPageModule)
  },
  {
    path: 'warehouse-inventory',
    loadChildren: () => import('./warehouse-inventory/warehouse-inventory.module').then( m => m.WarehouseInventoryPageModule)
  },  {
    path: 'warehouse-draft',
    loadChildren: () => import('./warehouse-draft/warehouse-draft.module').then( m => m.WarehouseDraftPageModule)
  }


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WarehousePageRoutingModule {}
