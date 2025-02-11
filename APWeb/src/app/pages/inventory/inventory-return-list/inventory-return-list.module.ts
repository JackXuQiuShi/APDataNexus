import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InventoryReturnListPageRoutingModule } from './inventory-return-list-routing.module';

import { InventoryReturnListPage } from './inventory-return-list.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InventoryReturnListPageRoutingModule
  ],
  declarations: []
})
export class InventoryReturnListPageModule {}
