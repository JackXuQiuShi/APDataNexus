import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { DatePipe } from '@angular/common';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './modules/material-ui/material-ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http'
import { NavbarComponent } from './components/navbar/navbar.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatCommonModule } from '@angular/material/core';

import { HomePage } from './home/home.page';
import { ItemPage } from './pages/item/item.page';
import { ItemInsertPage } from './pages/item/item-insert/item-insert.page';
import { ItemUpdatePage } from './pages/item/item-update/item-update.page';
import { ItemApprovalPage } from './pages/item/item-approval/item-approval.page';
import { ItemDraftPage } from './pages/item/item-draft/item-draft.page';
import { SupplierPage } from './pages/supplier/supplier.page';
import { SupplierInsertPage } from './pages/supplier/supplier-insert/supplier-insert.page';
import { SupplierApprovalPage } from './pages/supplier/supplier-approval/supplier-approval.page';
import { SupplierUpdatePage } from './pages/supplier/supplier-update/supplier-update.page';
import { ConfirmationComponent } from './components/confirmation/confirmation.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SignPOComponent } from './components/sign-po/sign-po.component';
import { PoPage } from './pages/po/po.page';
import { SearchSupplierComponent } from './components/search-supplier/search-supplier.component';
import { OnlinePage } from './pages/online/online.page';
import { WarehousePage } from './pages/warehouse/warehouse.page';
import { WarehouseInPage } from './pages/warehouse/warehouse-in/warehouse-in.page';

import { WarehouseInventoryPage } from './pages/warehouse/warehouse-inventory/warehouse-inventory.page';
import { GroupPage } from './pages/group/group.page';
import { GroupInsertPage } from './pages/group/group-insert/group-insert.page';
import { GroupDetailsPage } from './pages/group/group-details/group-details.page';
import { PoInsertPage } from './pages/po/po-insert/po-insert.page';
import { PoDetailsPage } from './pages/po/po-details/po-details.page';
import { PoReceivePage } from './pages/po/po-receive/po-receive.page';
import { CreatePOPage } from './pages/po/create-po/create-po.page';
import { InventoryPage } from './pages/inventory/inventory.page';
import { InventoryInsertPage } from './pages/inventory/inventory-insert/inventory-insert.page';
import { InventoryReturnPage } from './pages/inventory/inventory-return/inventory-return.page';
import { InventoryLocationPage } from './pages/inventory/inventory-location/inventory-location.page';
import { InventoryReturnListPage } from './pages/inventory/inventory-return-list/inventory-return-list.page';

import { LoadingInterceptor } from './loading.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DataTablesModule } from "angular-datatables";
import { ClipboardModule } from '@angular/cdk/clipboard';
import { PoDraftPage } from './pages/po/po-draft/po-draft.page';
import { InventoryListPage } from './pages/inventory/inventory-list/inventory-list.page';
import { HmrPage } from './pages/hmr/hmr.page';
import { HmrInPage } from './pages/hmr/hmr-in/hmr-in.page';
import { WarehouseDraftPage } from './pages/warehouse/warehouse-draft/warehouse-draft.page';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ItemChangePriceModule } from './pages/item/item-change-price/item-change-price.module';
import { ItemChangePricePage } from './pages/item/item-change-price/item-change-price.page';

@NgModule({
    declarations: [AppComponent,
        NavbarComponent,
        SidebarComponent,
        HomePage,
        ItemPage,
        ItemInsertPage,
        ItemUpdatePage,
        ItemApprovalPage,
        ItemChangePricePage,
        ItemDraftPage,
        SupplierPage,
        SupplierInsertPage,
        SupplierApprovalPage,
        SupplierUpdatePage,
        ConfirmationComponent,
        SignPOComponent,
        PoPage,
        SearchSupplierComponent,
        OnlinePage,
        WarehousePage,
        WarehouseInPage,
        WarehouseInventoryPage,
        WarehouseDraftPage,
        GroupPage,
        GroupInsertPage,
        GroupDetailsPage,
        PoInsertPage,
        PoDetailsPage,
        PoReceivePage,
        InventoryPage,
        InventoryInsertPage,
        InventoryReturnPage,
        InventoryLocationPage,
        InventoryListPage,
        InventoryReturnListPage,
        PoDraftPage,
        CreatePOPage,
        HmrPage,
        HmrInPage
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule, 
        IonicModule.forRoot(),
        AppRoutingModule,
        MaterialModule,
        BrowserAnimationsModule,
        FormsModule,
        DragDropModule,
        DataTablesModule,
        ClipboardModule],
    providers: [
        { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
        { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
        DatePipe, provideHttpClient(withInterceptorsFromDi()), provideAnimationsAsync()]
})
export class AppModule { }