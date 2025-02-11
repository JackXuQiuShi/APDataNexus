import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { WarehouseInventoryPageRoutingModule } from './warehouse-inventory-routing.module';

import { WarehouseInventoryPage } from './warehouse-inventory.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    WarehouseInventoryPageRoutingModule
  ],
  declarations: []
})
export class WarehouseInventoryPageModule {}
