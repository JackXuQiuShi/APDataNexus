import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { WarehouseInPageRoutingModule } from './warehouse-in-routing.module';

import { WarehouseInPage } from './warehouse-in.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    WarehouseInPageRoutingModule
  ],
  declarations: []
})
export class WarehouseInPageModule {}
