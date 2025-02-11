import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CreatePOPageRoutingModule } from './create-po-routing.module';

import { CreatePOPage } from './create-po.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CreatePOPageRoutingModule
  ],
  declarations: []
})
export class CreatePOPageModule {}
