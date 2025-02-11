import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Inventory } from 'src/assets/model/Inventory';
import { LastReceivingData } from 'src/assets/model/LastReceivingData';
import { User } from 'src/assets/model/User';
import { Warehouse } from 'src/assets/model/Warehouse';

@Component({
    selector: 'app-warehouse-inventory',
    templateUrl: './warehouse-inventory.page.html',
    styleUrls: ['./warehouse-inventory.page.scss'],
    standalone: false
})
export class WarehouseInventoryPage implements OnInit {
  private routeParamsSubscription!: Subscription;
  constructor(private router: Router, private dataService: DataService, private renderer: Renderer2, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.refreshData();
    });
  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

  private refreshData() {
    this.loadUser();
  }

  private loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      this.user = JSON.parse(userString);
      this.data.Applicant = this.user.Name;
      this.data.StoreID = this.user.Store;
    }
  }

  data: Warehouse = new Warehouse;
  user: User = new User;
  NormalizedID: string = '';

  onProductIDChange() {
    this.data.ProductName = "";
    this.data.UnitCost = undefined;
    this.data.UnitsPerPackage = undefined;
    this.data.SupplierName = "";
    this.data.SupplierID = 0;
    this.data.CaseQty = undefined;

    if (this.data.ProductID != undefined) {
      this.data.ProductID = this.data.ProductID.trim();
    }
    this.getNormalizedID(this.data.ProductID);
  }



  getNormalizedID(ProductID: string) {
    if (this.data.ProductID != "" && this.data.ProductID != undefined && !isNaN(Number(this.data.ProductID))) {
      this.dataService.getNormalizedID(ProductID).then(data => {
        if (data.normalizedID.length > 0) {
          this.NormalizedID = data.normalizedID;
          Promise.all([
            this.getCaseQty(this.data.ProductID),
            this.getProductInfoByProductID(this.data.ProductID),
            this.getLastPODetails(this.data.ProductID, this.data.StoreID),
          ])
        }
      });
    }
  }

  getLastPODetails(NormalizedID: string, StoreID: number) {
    this.dataService.getLastPODetails(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.data.SupplierID = data[0].SupplierID;
        this.data.SupplierName = data[0].CompanyName;
        this.data.UnitCost = data[0].PriceReceived;
        this.data.UnitsPerPackage = data[0].UnitsPerPackage;
      }
    });
  }

  getProductInfoByProductID(NormalizedID: string) {
    this.dataService.getProductInfoByProductID(NormalizedID).then(data => {
      if (data.length > 0) {
        this.data.ProductName = data[0].ProductName;
      }
    });
  }

  getCaseQty(NormalizedID: string) {
    this.dataService.getCaseQty(NormalizedID).then(data => {
      if (data.length > 0) {
        this.data.CaseQty = data[0].CaseQty;
      }
    });
  }

  submitWarehouseItem() {
    this.dataService.submitWarehouseItem(this.data).subscribe({
      next: (data) => {
        this.reset();
        this.snackBar.open('Submit Successful', 'Close', {
          duration: 3000, // duration in milliseconds
        });
      },
      error: (error) => {
        let errorMessage = error.message || 'submitWarehouseItem error occurred!';
        alert(errorMessage);
      }
    });
  }

  reset() {
    // this.data = new Warehouse;
    this.data.ProductID = '';
    this.data.CaseQty = undefined;
    this.data.ProductName = "";
    this.data.SupplierName = "";
    this.data.UnitCost = undefined;
    this.data.Location = "";
    this.data.UnitsPerPackage = undefined;
    this.NormalizedID = "";
  }

  getUnitQty() {
    if (this.data.CaseQty && this.data.UnitsPerPackage) {
      return this.data.CaseQty * this.data.UnitsPerPackage;
    }
    return 'N/A';
  }


}
