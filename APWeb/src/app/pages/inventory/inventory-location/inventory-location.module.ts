import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { InventoryLocationPageRoutingModule } from './inventory-location-routing.module';

import { InventoryLocationPage } from './inventory-location.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    InventoryLocationPageRoutingModule
  ],
  declarations: []
})
export class InventoryLocationPageModule {}
