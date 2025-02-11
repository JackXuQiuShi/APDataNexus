import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ItemApprovalPageRoutingModule } from './item-approval-routing.module';

import { ItemApprovalPage } from './item-approval.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ItemApprovalPageRoutingModule
  ],
  declarations: []
})
export class ItemApprovalPageModule {}
