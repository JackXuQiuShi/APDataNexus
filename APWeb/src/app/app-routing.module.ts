import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './services/AuthGuard';
import { environment } from '../environments/environment';
import { ItemPage } from './pages/item/item.page';

const routes: Routes = [];

if (environment.store) {
  routes.push(
    {
      path: '',
      redirectTo: 'login',
      pathMatch: 'full'
    },
    {
      path: 'login',
      loadChildren: () => import('./pages/login/login.module').then(m => m.LoginPageModule)
    },
    {
      path: 'home',
      loadChildren: () => import('./home/home.module').then(m => m.HomePageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'item',
      component: ItemPage,
      canActivate: [AuthGuard]
    },
    {
      path: 'item/item-insert',
      loadChildren: () => import('./pages/item/item-insert/item-insert.module').then(m => m.ItemInsertPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'item/item-draft',
      loadChildren: () => import('./pages/item/item-draft/item-draft.module').then(m => m.ItemDraftPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'inventory',
      loadChildren: () => import('./pages/inventory/inventory.module').then(m => m.InventoryPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: '**',
      redirectTo: 'login'
    }
  );
}
else {
  routes.push({
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
    {
      path: 'login',
      loadChildren: () => import('./pages/login/login.module').then(m => m.LoginPageModule)
    },
    {
      path: 'home',
      loadChildren: () => import('./home/home.module').then(m => m.HomePageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'item',
      loadChildren: () => import('./pages/item/item.module').then(m => m.ItemPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'supplier',
      loadChildren: () => import('./pages/supplier/supplier.module').then(m => m.SupplierPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'po',
      loadChildren: () => import('./pages/po/po.module').then(m => m.PoPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'online',
      loadChildren: () => import('./pages/online/online.module').then(m => m.OnlinePageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'warehouse',
      loadChildren: () => import('./pages/warehouse/warehouse.module').then(m => m.WarehousePageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'hmr',
      loadChildren: () => import('./pages/hmr/hmr.module').then(m => m.HmrPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'group',
      loadChildren: () => import('./pages/group/group.module').then(m => m.GroupPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: 'inventory',
      loadChildren: () => import('./pages/inventory/inventory.module').then(m => m.InventoryPageModule),
      canActivate: [AuthGuard]
    },
    {
      path: '**',
      redirectTo: 'login'
    });
}

// const routes: Routes = [
//   {
//     path: '',
//     redirectTo: 'login',
//     pathMatch: 'full'
//   },
//   {
//     path: 'login',
//     loadChildren: () => import('./pages/login/login.module').then(m => m.LoginPageModule)
//   },
//   {
//     path: 'home',
//     loadChildren: () => import('./home/home.module').then(m => m.HomePageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: 'item',
//     loadChildren: () => import('./pages/item/item.module').then(m => m.ItemPageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: 'supplier',
//     loadChildren: () => import('./pages/supplier/supplier.module').then(m => m.SupplierPageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: 'po',
//     loadChildren: () => import('./pages/po/po.module').then(m => m.PoPageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: 'online',
//     loadChildren: () => import('./pages/online/online.module').then(m => m.OnlinePageModule),
//     canActivate: [AuthGuard]
//   },

//   {
//     path: 'warehouse',
//     loadChildren: () => import('./pages/warehouse/warehouse.module').then(m => m.WarehousePageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: 'group',
//     loadChildren: () => import('./pages/group/group.module').then(m => m.GroupPageModule),
//     canActivate: [AuthGuard]
//   },
//   {
//     path: '**',
//     redirectTo: 'login'
//   }
// ];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
