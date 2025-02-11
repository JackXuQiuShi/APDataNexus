import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ItemDraftPageRoutingModule } from './item-draft-routing.module';

import { ItemDraftPage } from './item-draft.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ItemDraftPageRoutingModule
  ],
  declarations: []
})
export class ItemDraftPageModule {}
