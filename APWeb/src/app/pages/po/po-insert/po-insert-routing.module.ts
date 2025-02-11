import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PoInsertPage } from './po-insert.page';

const routes: Routes = [
  {
    path: '',
    component: PoInsertPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PoInsertPageRoutingModule {}
