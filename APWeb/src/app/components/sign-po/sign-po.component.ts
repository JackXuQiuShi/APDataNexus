import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-sign-po',
    templateUrl: './sign-po.component.html',
    styleUrls: ['./sign-po.component.scss'],
    standalone: false
})
export class SignPOComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() { }

  getTotalSum() {
    return this.data.PODetailsData.reduce((acc: number, detail: { UnitsOrdered: number; PriceOrdered: number; TaxRate: number; }) =>
      acc + (detail.UnitsOrdered * detail.PriceOrdered * (1 + detail.TaxRate)), 0);
  }

  getSubTotalSum() {
    return this.data.PODetailsData.reduce((acc: number, detail: { UnitsOrdered: number; PriceOrdered: number }) =>
      acc + (detail.UnitsOrdered * detail.PriceOrdered), 0);
  }




}
