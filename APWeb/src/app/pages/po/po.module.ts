import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PoPageRoutingModule } from './po-routing.module';

import { PoPage } from './po.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PoPageRoutingModule
  ],
  declarations: []
})
export class PoPageModule {}
