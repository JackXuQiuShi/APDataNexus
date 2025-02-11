import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InventoryInsertPageRoutingModule } from './inventory-insert-routing.module';

import { InventoryInsertPage } from './inventory-insert.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InventoryInsertPageRoutingModule
  ],
  declarations: []
})
export class InventoryInsertPageModule {}
