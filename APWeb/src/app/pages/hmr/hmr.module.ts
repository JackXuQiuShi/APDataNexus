import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { HmrPageRoutingModule } from './hmr-routing.module';

import { HmrPage } from './hmr.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HmrPageRoutingModule
  ],
  declarations: []
})
export class HmrPageModule {}
