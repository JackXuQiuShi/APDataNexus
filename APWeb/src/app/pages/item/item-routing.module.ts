import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ItemPage } from './item.page';

const routes: Routes = [
  {
    path: '',
    component: ItemPage
  },
  {
    path: 'item-update',
    loadChildren: () => import('./item-update/item-update.module').then( m => m.ItemUpdatePageModule)
  },
  {
    path: 'item-insert',
    loadChildren: () => import('./item-insert/item-insert.module').then( m => m.ItemInsertPageModule)
  },
  {
    path: 'item-approval',
    loadChildren: () => import('./item-approval/item-approval.module').then( m => m.ItemApprovalPageModule)
  },
  {
    path: 'item-draft',
    loadChildren: () => import('./item-draft/item-draft.module').then( m => m.ItemDraftPageModule)
  },
  {
    path: 'item-change-price',
    loadChildren: () => import('./item-change-price/item-change-price.module').then( m => m.ItemChangePriceModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ItemPageRoutingModule {}
