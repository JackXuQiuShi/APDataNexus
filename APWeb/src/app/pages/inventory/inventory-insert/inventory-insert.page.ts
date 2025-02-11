import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Inventory } from 'src/assets/model/Inventory';
import { InventoryReturn } from 'src/assets/model/InventoryReturn';
import { LastReceivingData } from 'src/assets/model/LastReceivingData';


@Component({
    selector: 'app-inventory-insert',
    templateUrl: './inventory-insert.page.html',
    styleUrls: ['./inventory-insert.page.scss'],
    standalone: false
})
export class InventoryInsertPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  user: any;
  Role: string = "";
  inventoryData: Inventory = new Inventory;
  NormalizedID: string = '';
  // returnData: InventoryReturn = new InventoryReturn();
  salesQTY: number = 0;
  salesQTY30: number = 0;
  salesQTY60: number = 0;
  salesQTY90: number = 0;

  lastReceiving?: string;
  RegPrice: number = 0;
  Cost: number = 0;
  color?: string | null = null;;
  expired: number = 0;
  doubleRed: boolean = false;
  lastScanDate: string = "";
  lastLocation: string = "";

  SupplierName!: string;
  locationTypeOptions = Object.entries({
    "Warehouse": "(仓库)",
    "StoreFloor": "(店内)",
    "Bayshelf": "(货架)",
    "Endcap": "(端架)",
    "Pile-Up-1": "(Pallet,Skid,托盘)",
    "Pile-Up-2": "(Pallet,Skid,托盘)"
  }).map(([key, value]) => ({ key, value }));

  @ViewChild('inputElement') inputElement!: ElementRef;

  constructor(private router: Router, private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.refreshData();
      this.inventoryData.LocationType = "Warehouse";
    });
  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

  ionViewDidLeave() {
    this.reset();
  }

  private refreshData() {
    this.loadUser();
    this.reset();
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
    this.inventoryData.InspectedBy = this.user.Name;
    this.inventoryData.StoreID = this.user.Store;
    this.Role = this.user.Role;
  }

  reset() {
    this.NormalizedID = "";
    this.inventoryData.ProductID = "";
    this.inventoryData.Product_ID = "";
    this.inventoryData.Units = undefined;
    this.inventoryData.ProductName = "";
    this.inventoryData.Barcode = "";
    this.inventoryData.Department = "";

    this.salesQTY = 0;
    this.RegPrice = 0;
    this.doubleRed = false;
    this.color = null;
    this.lastScanDate = "";
    this.lastReceiving = "";
    this.Cost = 0;
    this.SupplierName = "";
    this.salesQTY30 = 0;
    this.salesQTY60 = 0;
    this.salesQTY90 = 0;
    this.expired = 0;
    this.lastLocation = "";
    // this.returnData = new InventoryReturn();

    setTimeout(() => {
      this.inputElement.nativeElement.focus();
    }, 500);
  }


  onStatusChange() {
    this.expired = 0;
    this.doubleRed = false;

    if (this.inventoryData.Units == 1) {
      this.color = "red";
    } else if (this.inventoryData.Units == 2) {
      this.color = "yellow";
    } else {
      this.color = "blue";
    }
  }

  isExpired(event: MatCheckboxChange) {
    this.expired = event.checked ? 1 : 0;
    this.setStatus();
  }

  setStatus() {
    if (this.expired) {
      this.inventoryData.Units = -1;
      this.color = "red";
      this.doubleRed = true;
      return;
    }
    this.doubleRed = false;

    if (this.salesQTY30 == 0) {
      this.inventoryData.Units = 1;
      this.color = "red";
    } else {
      this.inventoryData.Units = 3
      this.color = "blue";
    }
    // if (this.data.StoreID == 39 || this.data.StoreID == 3 || this.data.StoreID == 7) {
    //   if (this.salesQTY == 0) {
    //     this.data.Units = 1;
    //     this.color = "red";
    //   } else if (this.salesQTY >= 4) {
    //     this.data.Units = 3
    //     this.color = "blue";
    //   } else {
    //     this.data.Units = 2
    //     this.color = "yellow";
    //   }
    // } else {
    //   if (this.salesQTY30 == 0) {
    //     this.data.Units = 1;
    //     this.color = "red";
    //   } else if (this.salesQTY30 >= 3) {
    //     this.data.Units = 3
    //     this.color = "blue";
    //   } else {
    //     this.data.Units = 2
    //     this.color = "yellow";
    //   }
    // }

  }

  onProductIDChange() {
    this.color = null;
    this.salesQTY = 0;
    this.salesQTY30 = 0;
    this.salesQTY60 = 0;
    this.salesQTY90 = 0;
    this.lastScanDate = "";
    this.SupplierName = "";
    this.lastReceiving = "";
    this.RegPrice = 0;
    this.Cost = 0;
    this.inventoryData.Department = "";
    this.inventoryData.ProductName = "";
    this.inventoryData.Barcode = "";
    this.lastLocation = "";

    if (this.inventoryData.ProductID != undefined) {
      this.inventoryData.ProductID = this.inventoryData.ProductID.trim();
    }
    this.getNormalizedID(this.inventoryData.ProductID);
  }

  onLocationChange() {
    this.getIfLocationExist(this.inventoryData.Location, this.inventoryData.StoreID);
  }

  getNormalizedID(ProductID: string) {
    if (this.inventoryData.ProductID != "" && this.inventoryData.ProductID != undefined && !isNaN(Number(this.inventoryData.ProductID))) {
      this.dataService.getNormalizedID(ProductID).then(data => {
        if (data.normalizedID.length > 0) {
          this.NormalizedID = data.normalizedID;
          Promise.all([
            this.getLastScanInfo(this.NormalizedID, this.inventoryData.StoreID),
            this.getLastPODetails(this.NormalizedID, this.inventoryData.StoreID),
            this.getRegPrice(this.NormalizedID, this.inventoryData.StoreID),
            this.getProductStoresInfoByProductID(this.NormalizedID, this.inventoryData.StoreID)
          ]).then(() => {
            // 在所有操作完成后调用 getQTY306090
            this.getQTY306090(this.inventoryData.Barcode);

          });
        }
      });
    }
  }

  getLastPODetails(NormalizedID: string, StoreID: number) {
    this.dataService.getLastPODetails(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.lastReceiving = data[0].ReceivingDate;
        this.Cost = data[0].PriceReceived;
        this.SupplierName = data[0].CompanyName;
      }
    });
  }

  getRegPrice(NormalizedID: string, StoreID: number) {
    this.dataService.getRegPrice(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.RegPrice = data[0].RegPrice;
      }
    });
  }

  async getProductStoresInfoByProductID(NormalizedID: string, StoreID: number) {
    const data = await this.dataService.getProductStoresInfoByProductID(NormalizedID, StoreID);
    if (data) {
      console.log(data)
      this.inventoryData.ProductName = data.ProductName;
      this.inventoryData.Department = data.Department;
      this.inventoryData.Barcode = data.Barcode;
    }
  }


  getQTY306090(Barcode: string) {
    this.dataService.getInfoFromPOS306090(Barcode).then(data => {
      if (data) {
        this.salesQTY30 = data[0].Total_QTY_30;
        this.salesQTY60 = data[0].Total_QTY_60;
        this.salesQTY90 = data[0].Total_QTY_90;
        this.salesQTY = data[0].Total_QTY_30 + data[0].Total_QTY_60 + data[0].Total_QTY_90;
      }
      this.setStatus();
    });
  }

  getLastScanInfo(NormalizedID: string, StoreID: number) {
    this.dataService.getLastScanInfo(NormalizedID, StoreID).then(data => {
      if (data.length > 0) {
        this.lastScanDate = data[0].LatestDate;
        this.lastLocation = data[0].Location;
      }
    });
  }

  getIfLocationExist(Location: string, StoreID: number) {
    this.dataService.getIfLocationExist(Location, StoreID).then(data => {
      if (!data.exists) {
        this.inventoryData.Location = "";
        alert("Invalid Location.");
      }
    });
  }


  insertInventory() {
    this.inventoryData.ProductID = this.NormalizedID;
    this.inventoryData.Product_ID = this.NormalizedID;
    this.inventoryData.Store_ID = this.user.Store;
    this.inventoryData.StatusID = this.inventoryData.Units;
    this.inventoryData.Units = this.inventoryData.UnitQty;

    if(this.inventoryData.LocationType == "Warehouse" || this.inventoryData.LocationType == "StoreFloor"){
      this.inventoryData.Location = this.inventoryData.LocationType;
    }
    this.dataService.insertInventory(this.inventoryData).subscribe({
      next: (data) => {
        this.submitStorageLocation();
        setTimeout(() => {
          this.inputElement.nativeElement.focus();
        }, 500); // 插入库存后添加短暂延迟
      },
      error: (error) => {
        let errorMessage = error.message || 'insertInventory error occurred!';
        alert(errorMessage);
      }
    });
  }

  submitStorageLocation() {
    this.inventoryData.ProductID = this.NormalizedID;
    this.dataService.submitStorageLocation(this.inventoryData).subscribe({
      next: (data) => {
        this.snackBar.open('Insert Successful', 'Close', {
          duration: 3000,
        });
        this.reset();
      },
      error: (error) => {
        let errorMessage = error.message || 'submitStorageLocation error occurred!';
        alert(errorMessage);
      }
    });

  }





}
