import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplierApprovalPageRoutingModule } from './supplier-approval-routing.module';

import { SupplierApprovalPage } from './supplier-approval.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplierApprovalPageRoutingModule
  ],
  declarations: []
})
export class SupplierApprovalPageModule {}
