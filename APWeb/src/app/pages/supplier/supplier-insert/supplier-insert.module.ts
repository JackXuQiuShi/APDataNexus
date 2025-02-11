import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplierInsertPageRoutingModule } from './supplier-insert-routing.module';

import { SupplierInsertPage } from './supplier-insert.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplierInsertPageRoutingModule
  ],
  declarations: []
})
export class SupplierInsertPageModule {}
