import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { Supplier } from 'src/assets/model/Supplier';

@Component({
    selector: 'app-supplier',
    templateUrl: './supplier.page.html',
    styleUrls: ['./supplier.page.scss'],
    standalone: false
})
export class SupplierPage implements OnInit {

  constructor(private sharedService: SharedService) { }

  ngOnInit() {}


  getData(){
  }



}
