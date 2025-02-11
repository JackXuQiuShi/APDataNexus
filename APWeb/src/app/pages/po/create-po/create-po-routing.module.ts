import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreatePOPage } from './create-po.page';

const routes: Routes = [
  {
    path: '',
    component: CreatePOPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreatePOPageRoutingModule {}
