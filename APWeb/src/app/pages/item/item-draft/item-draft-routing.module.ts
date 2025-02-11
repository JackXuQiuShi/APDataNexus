import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ItemDraftPage } from './item-draft.page';

const routes: Routes = [
  {
    path: '',
    component: ItemDraftPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ItemDraftPageRoutingModule {}
