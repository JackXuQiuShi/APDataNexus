import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { GroupInsertPageRoutingModule } from './group-insert-routing.module';

import { GroupInsertPage } from './group-insert.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    GroupInsertPageRoutingModule
  ],
  declarations: []
})
export class GroupInsertPageModule {}
