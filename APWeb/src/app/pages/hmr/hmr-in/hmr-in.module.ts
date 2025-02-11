import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { HmrInPageRoutingModule } from './hmr-in-routing.module';

import { HmrInPage } from './hmr-in.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HmrInPageRoutingModule
  ],
  declarations: []
})
export class HmrInPageModule {}
