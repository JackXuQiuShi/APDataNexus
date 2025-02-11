import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PoDetailsPageRoutingModule } from './po-details-routing.module';

import { PoDetailsPage } from './po-details.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PoDetailsPageRoutingModule
  ],
  declarations: []
})
export class PoDetailsPageModule {}
