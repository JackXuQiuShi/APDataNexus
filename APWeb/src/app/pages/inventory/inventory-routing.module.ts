import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InventoryPage } from './inventory.page';

const routes: Routes = [
  {
    path: '',
    component: InventoryPage
  },
  {
    path: 'inventory-insert',
    loadChildren: () => import('./inventory-insert/inventory-insert.module').then( m => m.InventoryInsertPageModule)
  },
  {
    path: 'inventory-return',
    loadChildren: () => import('./inventory-return/inventory-return.module').then( m => m.InventoryReturnPageModule)
  },
  {
    path: 'inventory-location',
    loadChildren: () => import('./inventory-location/inventory-location.module').then( m => m.InventoryLocationPageModule)
  },
  {
    path: 'inventory-list',
    loadChildren: () => import('./inventory-list/inventory-list.module').then( m => m.InventoryListPageModule)
  },
  {
    path: 'inventory-return-list',
    loadChildren: () => import('./inventory-return-list/inventory-return-list.module').then( m => m.InventoryReturnListPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryPageRoutingModule {}
