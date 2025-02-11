import { Component, ElementRef, HostListener, NgZone, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSelectChange } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Inventory } from 'src/assets/model/Inventory';
import { InventoryReturn } from 'src/assets/model/InventoryReturn';


@Component({
    selector: 'app-inventory-return',
    templateUrl: './inventory-return.page.html',
    styleUrls: ['./inventory-return.page.scss'],
    standalone: false
})
export class InventoryReturnPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  private inventorySubscription!: Subscription;

  constructor(private router: Router, private dataService: DataService, private renderer: Renderer2, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) {
    //this.startMonitoringClipboard();
  }

  user: any;
  data: any;
  // SupplierID: number = 0;
  RegPrice: number = 0;
  SupplierName!: string;
  returnData: InventoryReturn = new InventoryReturn;
  isSupplierFound: boolean = true;
  lockSupplier: boolean = false;
  updateQuantity: boolean = false;
  NormalizedID: string = '';
  foundPO: boolean = false;

  async ngOnInit() {
    await this.dataService.loadConfig();



    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      // this.returnData.ProductID = params['ProductID'];
      // this.returnData.SupplierID = params['SupplierID'];
      this.refreshData();
      this.loadUser();
    });

    // this.inventorySubscription = this.sharedService.observableData.subscribe((data) => {

    //   this.returnData = data;
    // })
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
    this.inventorySubscription?.unsubscribe();
  }

  ionViewDidLeave() {
    this.reset();
  }

  private refreshData() {

    this.inventorySubscription = this.sharedService.observableData.subscribe((newData: any) => {
      this.SupplierName = newData.CompanyName;

      if (newData != 'Initial Data') {
        this.returnData = newData;
        if (newData.Tax) {

          this.returnData.Tax = 1;
        } else {
          this.returnData.Tax = 0;
        }
      }

      this.loadUser();
      if (this.SupplierName != null) {
        this.isSupplierFound = true;
      }
    });

    // this.getNormalizedID(this.returnData.ProductID);
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
    this.returnData.StoreID = this.user.Store;
  }

  // private loadUser() {
  //   const userString = sessionStorage.getItem('user');
  //   if (userString) {
  //     this.user = JSON.parse(userString);
  //     this.returnData.StoreID = this.user.Store;
  //   }
  // }

  changeSupplier() {
    this.isSupplierFound = false;
  }

  onProductIDChange() {
    if (!this.lockSupplier) {
      this.SupplierName = "";
      this.returnData.SupplierID = undefined;
    } else {
      this.returnData.ProductName = "";
      this.returnData.ReturnQuantity = undefined;
    }

    this.returnData.ProductName = "";
    this.returnData.UnitCost = undefined;
    this.returnData.Tax = 0;
    this.RegPrice = 0;
    this.returnData.ReturnQuantity = undefined;

    if (this.returnData.ProductID != undefined) {
      this.returnData.ProductID = this.returnData.ProductID.trim();
    }

    this.getNormalizedID(this.returnData.ProductID);
  }

  reset() {
    this.updateQuantity = false;
    this.RegPrice = 0;
    this.NormalizedID = "";
    this.foundPO = true;

    if (!this.lockSupplier) {
      this.SupplierName = "";
      this.returnData = new InventoryReturn();
      this.loadUser();
    } else {
      this.returnData.ProductID = "";
      this.returnData.ProductName = "";
      this.returnData.ReturnQuantity = undefined;
    }
  }

  checkTax(event: MatCheckboxChange) {
    this.returnData.Tax = event.checked ? 1 : 0;
  }

  getCostWithMargin(RegPrice: number) {
    const margin = 0.25;
    return RegPrice / (1 + margin);
  }


  getNormalizedID(ProductID: string) {
    this.foundPO = false;
    if (ProductID != "" && ProductID != undefined && !isNaN(Number(ProductID))) {
      this.dataService.getNormalizedID(ProductID).then(data => {
        if (data.normalizedID.length > 0) {
          this.NormalizedID = data.normalizedID;
          Promise.all([
            this.getLastPODetails(this.NormalizedID, this.returnData.StoreID),
            this.getRegPrice(this.NormalizedID, this.returnData.StoreID),
            this.getProductInfoByProductID(this.NormalizedID),
            this.getReturnQuantity(this.NormalizedID, this.returnData.StoreID),
            this.getMaxMinCost(this.NormalizedID)
          ])
        }
      });
    }
  }


  getLastPODetails(NormalizedID: string, StoreID: number) {
    if (!this.lockSupplier || this.returnData.SupplierID == undefined) {
      // this.lockSupplier = false;
      this.dataService.getLastPODetails(NormalizedID, StoreID).then(data => {
        if (data.length > 0) {
          this.returnData.Tax = data[0].Tax1App;
          this.SupplierName = data[0].CompanyName;
          this.returnData.SupplierID = data[0].SupplierID;
          this.returnData.UnitCost = data[0].PriceReceived;
          this.foundPO = true;
        }
      });
    }
  }


  getRegPrice(NormalizedID: string, StoreID: number) {
    this.dataService.getRegPrice(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.RegPrice = data[0].RegPrice;
      }
    });
  }

  getProductInfoByProductID(NormalizedID: string) {
    this.dataService.getProductInfoByProductID(NormalizedID).then(data => {
      if (data) {
        this.returnData.ProductName = data.ProductName;
      }
    });
  }

  getReturnQuantity(NormalizedID: string, StoreID: number) {
    this.dataService.getReturnInfo(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.updateQuantity = true;
        this.returnData.ReturnQuantity = data[0].ReturnQuantity;
        this.returnData.ReturnID = data[0].ReturnID;
      }
    });
  }

  MinCost!: number;
  MaxCost!: number;
  MinCostStore!: number;
  MaxCostStore!: number;

  getMaxMinCost(NormalizedID: string) {
    this.MaxCost = 0;
    this.MinCost = 0;
    this.MinCostStore = 0;
    this.MaxCostStore = 0;
    this.dataService.getMaxMinCost(NormalizedID).then(data => {
      if (data) {
        this.MaxCost = data.MaxCost;
        this.MinCost = data.MinCost;
        this.MinCostStore = data.MinStoreID;
        this.MaxCostStore = data.MaxStoreID;
      }
    });
  }

  updateProductInspectionStatus(item: any) {
    this.dataService.updateProductInspectionStatus(item).subscribe({
      next: (data) => {
        this.reset();
        this.snackBar.open('Submit Successful', 'Close', {
          duration: 3000,
        });
      },
      error: (error) => {
        let errorMessage = error.message || 'updateProductInspectionStatus error occurred!';
        alert(errorMessage);
      }
    });
  }

  submitReturnItem() {
    this.returnData.ProductID = this.NormalizedID;
    this.dataService.submitReturnItem(this.returnData).subscribe({
      next: (data) => {
        this.updateProductInspectionStatus(this.returnData)
      },
      error: (error) => {
        let errorMessage = error.message || 'submitReturnItem error occurred!';
        alert(errorMessage);
      }
    });
  }






}
