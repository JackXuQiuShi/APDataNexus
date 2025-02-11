import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { PoDraftPageRoutingModule } from './po-draft-routing.module';

import { PoDraftPage } from './po-draft.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    PoDraftPageRoutingModule
  ],
  declarations: []
})
export class PoDraftPageModule {}
