import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InventoryReturnPageRoutingModule } from './inventory-return-routing.module';

import { InventoryReturnPage } from './inventory-return.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InventoryReturnPageRoutingModule
  ],
  declarations: []
})
export class InventoryReturnPageModule {}
