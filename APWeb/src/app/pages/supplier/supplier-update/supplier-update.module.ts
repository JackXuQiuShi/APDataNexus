import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplierUpdatePageRoutingModule } from './supplier-update-routing.module';

import { SupplierUpdatePage } from './supplier-update.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplierUpdatePageRoutingModule
  ],
  declarations: []
})
export class SupplierUpdatePageModule {}
