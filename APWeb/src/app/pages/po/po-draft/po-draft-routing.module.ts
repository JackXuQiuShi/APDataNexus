import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PoDraftPage } from './po-draft.page';

const routes: Routes = [
  {
    path: '',
    component: PoDraftPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PoDraftPageRoutingModule {}
