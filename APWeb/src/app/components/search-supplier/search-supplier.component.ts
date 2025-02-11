import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { Supplier } from 'src/assets/model/Supplier';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
    selector: 'app-search-supplier',
    templateUrl: './search-supplier.component.html',
    styleUrls: ['./search-supplier.component.scss'],
    standalone: false
})
export class SearchSupplierComponent implements OnInit {

  constructor(private sharedService: SharedService, private router: Router, private dataService: DataService) { }

  ngOnInit() { }

  @Input() functionType!: string;
  @Output() newItemEvent = new EventEmitter<string>();

  data: any;
  supplierData: any = {};
  SupplierName !: string;


  getSupplierByName() {
    if (this.SupplierName.length >= 3) {
      this.dataService.getSupplierByName(this.SupplierName).then(
        data => this.data = data
      );
    }
  }

  getSupplierByCFIA() { }


  selectSupplier(selectedSupplier: Supplier) {
    const currentUrl = this.router.url;

    this.SupplierName = selectedSupplier.CompanyName;
    this.data = [];
    this.supplierData = selectedSupplier;
    this.sharedService.setMyData(this.supplierData);

    if (currentUrl == "/supplier") {
      this.router.navigate(['/supplier/supplier-update']);
    }
    else if (currentUrl == "/item") {
      this.router.navigate(['/item/item-insert']);
    }
    else if (currentUrl == "/po") {
      this.router.navigate(['/po/po-insert']);
    }
    else if (currentUrl == "/inventory") {
      this.router.navigate(['/inventory/inventory-return']);
    }
  }

  createSupplier() {
    this.router.navigate(['/supplier/supplier-insert']);
  }


  // createNewItem(selectedSupplier: Supplier){
  //   this.newItemEvent.emit(selectedSupplier.CompanyName);
  //   this.data = [];
  //   this.router.navigate(['/item/item-insert']);
  // }



  // createPO(){
  //   this.router.navigate(['/po']);
  // }
}
