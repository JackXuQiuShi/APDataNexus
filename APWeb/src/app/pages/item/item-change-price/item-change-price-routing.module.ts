import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ItemChangePricePage } from './item-change-price.page';

const routes: Routes = [
  {
    path: '',
    component: ItemChangePricePage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ItemChangePriceRoutingModule { }
