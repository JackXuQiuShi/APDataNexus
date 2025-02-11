import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { Supplier } from 'src/assets/model/Supplier';

@Component({
    selector: 'app-supplier-update',
    templateUrl: './supplier-update.page.html',
    styleUrls: ['./supplier-update.page.scss'],
    standalone: false
})
export class SupplierUpdatePage implements OnInit {

  constructor(private sharedService: SharedService, route: ActivatedRoute) {
    route.params.subscribe(val => {
      this.ngOnInit();
    });
  }

  data: Supplier = new Supplier;

  ngOnInit() {
    if (this.sharedService.getMyData() != undefined) {
      this.data = this.sharedService.getMyData();
    }
  }

  // ngOnChanges(){
  //   console.log("this.sharedService.getMyData()")
  //   if(this.sharedService.getMyData() != undefined){
  //     this.data = this.sharedService.getMyData();  
  //     console.log(this.data)
  //   }
  // }


  supplierUpdate() { }
  supplierDelete() { }
  createProduct() { }
  createPO() { }
}
