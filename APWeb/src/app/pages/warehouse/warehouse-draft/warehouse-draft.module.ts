import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { WarehouseDraftPageRoutingModule } from './warehouse-draft-routing.module';

import { WarehouseDraftPage } from './warehouse-draft.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    WarehouseDraftPageRoutingModule
  ],
  declarations: []
})
export class WarehouseDraftPageModule {}
