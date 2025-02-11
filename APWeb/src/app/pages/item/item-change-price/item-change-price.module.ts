import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { ItemChangePricePage } from './item-change-price.page';
import { ItemChangePriceRoutingModule } from './item-change-price-routing.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  imports: [CommonModule, FormsModule, IonicModule, ItemChangePriceRoutingModule,MatFormFieldModule,
    MatInputModule],
  declarations: []
})
export class ItemChangePriceModule {}
