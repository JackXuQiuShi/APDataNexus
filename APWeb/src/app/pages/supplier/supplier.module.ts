import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplierPageRoutingModule } from './supplier-routing.module';

import { SupplierPage } from './supplier.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplierPageRoutingModule
  ],
  declarations: []
})
export class SupplierPageModule {}
