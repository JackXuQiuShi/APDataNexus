import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PoReceivePageRoutingModule } from './po-receive-routing.module';

import { PoReceivePage } from './po-receive.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PoReceivePageRoutingModule
  ],
  declarations: []
})
export class PoReceivePageModule {}
