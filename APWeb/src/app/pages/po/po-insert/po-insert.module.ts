import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PoInsertPageRoutingModule } from './po-insert-routing.module';

import { PoInsertPage } from './po-insert.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PoInsertPageRoutingModule
  ],
  declarations: []
})
export class PoInsertPageModule {}
