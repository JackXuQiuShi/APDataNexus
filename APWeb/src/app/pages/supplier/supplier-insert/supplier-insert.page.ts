import { Component, OnInit } from '@angular/core';
import { Supplier } from 'src/assets/model/Supplier';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-supplier-insert',
    templateUrl: './supplier-insert.page.html',
    styleUrls: ['./supplier-insert.page.scss'],
    standalone: false
})
export class SupplierInsertPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  
  constructor(private dataService: DataService, private sharedService: SharedService) { }

  ngOnInit() {
  }
 
  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }
  
  data: Supplier = new Supplier;

  

  //insert supplier to Supplier_Request table
  supplierInsert() {
    this.dataService.insertSupplierRequest(this.data).subscribe({
      next: (data) => {
        alert(data.message);
        this.data = new Supplier;
      },
      error: (error) => {
        let errorMessage = error.error.message || 'insertSupplierRequest error occurred!';
        alert(errorMessage);
      }
    });
  }

  test(){}


}
