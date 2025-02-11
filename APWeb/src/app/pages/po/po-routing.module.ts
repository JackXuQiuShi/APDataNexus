import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PoPage } from './po.page';

const routes: Routes = [
  {
    path: '',
    component: PoPage
  },
  {
    path: 'po-insert',
    loadChildren: () => import('./po-insert/po-insert.module').then( m => m.PoInsertPageModule)
  },
  {
    path: 'po-details',
    loadChildren: () => import('./po-details/po-details.module').then( m => m.PoDetailsPageModule)
  },
  {
    path: 'po-receive',
    loadChildren: () => import('./po-receive/po-receive.module').then( m => m.PoReceivePageModule)
  },  {
    path: 'po-draft',
    loadChildren: () => import('./po-draft/po-draft.module').then( m => m.PoDraftPageModule)
  },
  {
    path: 'create-po',
    loadChildren: () => import('./create-po/create-po.module').then( m => m.CreatePOPageModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PoPageRoutingModule {}
