import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HmrPage } from './hmr.page';

const routes: Routes = [
  {
    path: '',
    component: HmrPage
  },
  {
    path: 'hmr-in',
    loadChildren: () => import('./hmr-in/hmr-in.module').then( m => m.HmrInPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HmrPageRoutingModule {}
