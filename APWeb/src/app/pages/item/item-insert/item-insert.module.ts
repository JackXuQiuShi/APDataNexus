import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ItemInsertPageRoutingModule } from './item-insert-routing.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ItemInsertPageRoutingModule
  ],
  declarations: []
})
export class ItemInsertPageModule {}
