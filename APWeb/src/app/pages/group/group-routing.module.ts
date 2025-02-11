import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GroupPage } from './group.page';

const routes: Routes = [
  {
    path: '',
    component: GroupPage
  },
  {
    path: 'group-insert',
    loadChildren: () => import('./group-insert/group-insert.module').then( m => m.GroupInsertPageModule)
  },
  {
    path: 'group-details',
    loadChildren: () => import('./group-details/group-details.module').then( m => m.GroupDetailsPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupPageRoutingModule {}
