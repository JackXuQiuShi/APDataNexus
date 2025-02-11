import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ItemUpdatePage } from './item-update.page';

const routes: Routes = [
  {
    path: '',
    component: ItemUpdatePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ItemUpdatePageRoutingModule {}
