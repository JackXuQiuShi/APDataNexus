import { Component, OnInit } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { PO } from 'src/assets/model/PO';
import { PODetails } from 'src/assets/model/PODetails';
import { Supplier } from 'src/assets/model/Supplier';
import { User } from 'src/assets/model/User';

@Component({
  selector: 'app-po-insert',
  templateUrl: './po-insert.page.html',
  styleUrls: ['./po-insert.page.scss'],
  standalone: false
})
export class PoInsertPage implements OnInit {
  private routeParamsSubscription!: Subscription;
  private sharedServiceSubscription!: Subscription;

  constructor(private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar, private router: Router) { }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.reset();
      this.refreshData();
      this.OrderID = params['OrderID'];
      this.SupplierName = params['SupplierName'];
      this.SupplierID = params['SupplierID'];
    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
    this.sharedServiceSubscription?.unsubscribe();
  }

  private refreshData() {
    this.sharedServiceSubscription = this.sharedService.observableData.subscribe((newData: any) => {
      this.loadUser();
      this.itemData.SupplierID = newData.SupplierID;
      this.SupplierName = newData.CompanyName;
    });
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
    this.itemData.StoreID = this.user.Store;
  }

  user: User = new User;
  itemData: PODetails = new PODetails;
  NormalizedID: string = '';
  salesQTY: number = 0;
  salesQTY30: number = 0;
  salesQTY60: number = 0;
  salesQTY90: number = 0;
  MinCost!: number;
  MaxCost!: number;
  MinCostStore!: number;
  MaxCostStore!: number;
  lastReceivingDate?: string;
  lastUnitsReceived: number = 0;
  lastUnitsPerPackage: number = 0;
  lastSupplierName?: string;
  RegPrice: number = 0;
  Cost: number = 0;
  lastLocation: string = "";
  currentTabIndex: number = 0; // 用于存储当前的标签页引
  SupplierName!: string;
  SupplierID: number = 0;
  SupplierList: Supplier[] = [];
  OrderID!: number;
  isProductExist: boolean = false;

  reset() {
    this.itemData = new PODetails;
    this.SupplierList = [];
    this.resetData();
    this.loadUser();
  }

  resetData() {
    this.MaxCost = 0;
    this.MinCost = 0;
    this.MinCostStore = 0;
    this.MaxCostStore = 0;
    this.salesQTY = 0;
    this.salesQTY30 = 0;
    this.salesQTY60 = 0;
    this.salesQTY90 = 0;
    // this.SupplierName = "";
    this.lastReceivingDate = "";
    this.lastUnitsReceived = 0;
    this.lastSupplierName = "";
    this.RegPrice = 0;
    this.Cost = 0;
    this.lastLocation = "";
    this.NormalizedID = "";
    this.itemData.Barcode = "";
    this.itemData.Department = "";
    this.itemData.ProductName = "";
    this.isProductExist = false;
  }

  async onProductIDChange() {
    this.resetData();

    if (!this.itemData.ProductID || isNaN(Number(this.itemData.ProductID.trim()))) return;
    this.itemData.ProductID = this.itemData.ProductID.trim();

    await this.getNormalizedID(this.itemData.ProductID);
  }

  onSupplierChange() {
    this.reset();
  }

  onCaseQtyChange(itemData: any) {
    itemData.UnitQty = itemData.CaseQty * itemData.UnitsPerPackage;
  }

  onUnitQtyChange(itemData: any) {
    itemData.CaseQty = itemData.UnitQty / itemData.UnitsPerPackage;
    //itemData.DestinationStockCaseQty = itemData.DestinationCurrentStock / itemData.UnitsPerPackage;
    // itemData.SourceStockCaseQty = itemData.SourceCurrentStock / itemData.UnitsPerPackage;
  }

  checkTax1(event: MatCheckboxChange) {
    if (event.checked) {
      // 如果选中 Tax1，则取消选中 Tax2
      this.itemData.Tax1App = 1;
      this.itemData.Tax2App = 0;
    } else {
      // 如果取消 Tax1，保留 Tax2 的现状
      this.itemData.Tax1App = 0;
    }
  }

  checkTax2(event: MatCheckboxChange) {
    if (event.checked) {
      // 如果选中 Tax2，则取消选中 Tax1
      this.itemData.Tax2App = 1;
      this.itemData.Tax1App = 0;
    } else {
      // 如果取消 Tax2，保留 Tax1 的现状
      this.itemData.Tax2App = 0;
    }
  }


  async getNormalizedID(ProductID: string) {
    if (!ProductID) return;
    const data = await this.dataService.getNormalizedID(ProductID);
    if (data?.normalizedID?.length > 0) {
      this.NormalizedID = data.normalizedID;


      const isExist = await this.isProductItemExist(this.NormalizedID, this.SupplierID);
      if (!isExist) {
        this.itemData.ProductID = "";
        alert('The product is not associated with this supplier. Please contact Michael for more details.');
        return; // 如果产品不存在，直接退出函数
      }


      await this.loadLastPODetails(this.NormalizedID, this.user.Store);
      await this.loadProductStoresInfo(this.NormalizedID, this.user.Store);

      // await Promise.all([
      //   this.loadProductStoresInfo(this.NormalizedID, this.user.Store),
      //   this.loadLastPODetails(this.NormalizedID, this.user.Store)
      // ]);

      if (this.isProductExist) {
        await Promise.all([
          this.loadSupplierInfo(this.NormalizedID),
          this.loadCostInfo(this.NormalizedID),
          this.loadRegPrice(this.NormalizedID, this.user.Store),
          this.loadSalesData(this.itemData.Barcode),
          this.onUnitQtyChange(this.itemData)
        ]);
      }

    }
  }

  private async loadLastPODetails(NormalizedID: string, StoreID: number) {
    const data = await this.dataService.getLastPODetails(NormalizedID, StoreID);
    if (data?.length > 0) {
      const detail = data[0];
      this.lastReceivingDate = detail.ReceivingDate;
      this.Cost = detail.PriceReceived;
      this.lastUnitsReceived = detail.UnitsReceived;
      this.itemData.UnitQty = detail.UnitsReceived;
      this.itemData.UnitCost = detail.PriceReceived;
      this.lastUnitsPerPackage = detail.UnitsPerPackage;
      this.itemData.UnitsPerPackage = detail.UnitsPerPackage;
      this.lastSupplierName = detail.CompanyName;
      // if (this.currentTabIndex == 0) {
      //   this.SupplierName = detail.CompanyName;
      //   this.itemData.SupplierID = detail.SupplierID;
      // }
    }
  }

  private async loadProductStoresInfo(NormalizedID: string, StoreID: number) {
    const data = await this.dataService.getProductStoresInfoByProductID(NormalizedID, StoreID);
    if (data) {
      this.isProductExist = true;
      this.itemData.ProductName = data.ProductName;
      this.itemData.Department = data.Department;
      this.itemData.Barcode = data.Barcode;
      this.itemData.Tax1App = data.Tax1App;
      this.itemData.Tax2App = data.Tax2App;

      if (this.itemData.UnitsPerPackage == 0) {
        this.itemData.UnitsPerPackage = data.NumPerPack;
      }

    } else {
      this.isProductExist = false;
      this.resetData();
    }
  }

  private async loadRegPrice(NormalizedID: string, StoreID: number) {
    const data = await this.dataService.getRegPrice(NormalizedID, StoreID);
    if (data?.length > 0) {
      this.RegPrice = data[0].RegPrice;
    }
  }

  private async loadCostInfo(NormalizedID: string) {
    const data = await this.dataService.getMaxMinCost(NormalizedID);
    if (data) {
      this.MaxCost = data.MaxCost;
      this.MinCost = data.MinCost;
      this.MinCostStore = data.MinStoreID;
      this.MaxCostStore = data.MaxStoreID;
    }
  }

  private async loadSupplierInfo(NormalizedID: string) {
    const data = await this.dataService.getSuppliersByProductID(NormalizedID);
    if (data) {
      this.SupplierList = data;
      if (!this.itemData.SupplierID && data[0]) {
        this.itemData.SupplierID = data[0].SupplierID;
      }
    }
  }

  private async loadSalesData(Barcode: string) {
    const data = await this.dataService.getInfoFromPOS306090(Barcode);
    if (data) {
      this.salesQTY30 = data[0]?.Total_QTY_30 || 0;
      this.salesQTY60 = data[0]?.Total_QTY_60 || 0;
    }
  }

  private async isProductItemExist(ProductID: string, SupplierID: number) {
    const data = await this.dataService.isProductItemExist(ProductID, SupplierID);
    return data.isExist;
  }



  draftOrderItem() {
    this.itemData.TaxRate = 0.13 * this.itemData.Tax1App + 0.05 * this.itemData.Tax2App;
    this.itemData.ProductID = this.itemData.Barcode;
    console.log(this.itemData)

    const currentSupplierName = this.SupplierName;
    const currentSupplierID = this.itemData.SupplierID;

    this.dataService.draftOrderItem(this.itemData).subscribe({
      next: () => {
        this.snackBar.open('Insert Successful', 'Close', { duration: 3000 });
        this.reset();
        if (this.currentTabIndex === 1) {
          this.SupplierName = currentSupplierName;
          this.itemData.SupplierID = currentSupplierID;
        }
      },
      error: (error) => {
        let errorMessage = error.message || 'draftOrderItem error occurred!';
        alert(errorMessage);
      }
    });
  }





}
