import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HmrInPage } from './hmr-in.page';

const routes: Routes = [
  {
    path: '',
    component: HmrInPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HmrInPageRoutingModule {}
