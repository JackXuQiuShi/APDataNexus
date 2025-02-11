import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PoDetailsPage } from './po-details.page';

const routes: Routes = [
  {
    path: '',
    component: PoDetailsPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PoDetailsPageRoutingModule {}
