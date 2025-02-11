import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { User } from 'src/assets/model/User';
import { Warehouse } from 'src/assets/model/Warehouse';


@Component({
    selector: 'app-warehouse-in',
    templateUrl: './warehouse-in.page.html',
    styleUrls: ['./warehouse-in.page.scss'],
    standalone: false
})
export class WarehouseInPage implements OnInit {
  private routeParamsSubscription!: Subscription;

  constructor(private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  ProductData: Warehouse = new Warehouse;
  InventoryData: Warehouse[] = [];
  TransactionData: Warehouse[] = [];

  //NormalizedID!: string;
  user: User = new User;
  action!: string;
  actionList = ["Add In", "Take Out"];
  backgroundColor!: string;
  isAddIn: boolean = true;
  isChecked = false;
  isSupplierFound: boolean = true;

  Source!: number;
  AddTo: number = 3;
  StoreID!: number;

  inputProductID: string = "";
  ProductName: string = "";
  TotalQty: number = 0;
  DepartmentName!: string;
  UnitsPerPackage!: number;
  SupplierName!: string;

  selectedDate: Date = new Date;
  SellToList = ["OFC", "ALP", "OFMM", "OFCD"];
  filteredProducts: Warehouse[] = [];
  SourceOptions = Object.entries({
    "OFC": 3,
    "OFMM": 7,
    "ALP": 39,
    "OFCD": 1
  }).map(([key, value]) => ({ key, value }));





  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.reset();
      this.refreshData();
    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
  }

  private refreshData() {
    this.sharedService.observableData.subscribe((newData: any) => {
      this.loadUser();
      this.isChecked = false;
      this.ToggleAction(this.isChecked);
      this.SupplierName = newData.CompanyName;
      this.ProductData.SupplierID = newData.SupplierID;

      if (this.ProductData.SupplierName != null) {
        this.isSupplierFound = true;
      }
    });
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
    this.ProductData.StoreID = this.user.Store;
  }

  // 合并获取库存和总数量的逻辑
  private async loadWarehouseData(productID: string) {
    if (productID) {
      this.getWarehouseProductQty(productID, this.ProductData.StoreID)
      this.getLastPODetails(productID, this.ProductData.StoreID)
    }
  }

  // 用户选择某个产品时的操作
  selectProduct(product: any) {
    this.ProductData = product;
    this.onProductChange('ProductID');
    this.filteredProducts = [];
  }

  async onProductChange(field: 'ProductID' | 'ProductName') {
    this.InventoryData = [];
    const value = this.ProductData[field]?.trim();

    if (value) {
      this.ProductData[field] = value;
      this.reset();
      //this.loadUser();
      console.log(this.ProductData);
      if (this.action == "Add In") {
        this.ProductData.StoreID = this.AddTo;
      } else {
        this.ProductData.StoreID = this.ProductData.Source;
      }


      if (field == "ProductID") {
        this.getNormalizedID(value);
        //this.getWarehouseProductByProductID(value);
      } else {
        this.getWarehouseProductByProductName(value);
      }
    }
  }

  // 调用示例
  onProductIDChange() {
    this.ProductData.ProductID = this.inputProductID;
    this.onProductChange('ProductID');
  }

  onProductNameChange() {
    this.onProductChange('ProductName');
  }

  onSourceChange() {
    this.onProductChange('ProductID');
  }

  changeSupplier() {
    this.isSupplierFound = false;
  }

  addIn() {
    const formattedDate = this.sharedService.formatDateForSQL(this.selectedDate);
    // 将格式化后的日期赋值给 ProductData.Date
    if (formattedDate) {
      this.ProductData.Date = formattedDate;
    }
    this.addInWarehouse(this.ProductData);
  }

  takeOut() {
    this.takeOutWarehouse(this.ProductData);
  }


  ToggleAction(event: any) {
    if (event.value) {
      this.action = 'Take Out';
      this.isAddIn = false;
      this.backgroundColor = 'lightcoral';
      this.ProductData.Action = "Take Out"
    } else {
      this.action = 'Add In';
      this.isAddIn = true;
      this.backgroundColor = 'lightgreen';
      this.ProductData.Action = "Add In";
    }
  }


  reset() {
    this.selectedDate = new Date;
    this.TransactionData = [];
    this.InventoryData = [];
    this.filteredProducts = [];
    //this.ProductData = new Warehouse;
    this.inputProductID = "";
    this.TotalQty = 0;
    this.SupplierName = "";
    this.UnitsPerPackage = 0;
    this.DepartmentName = "";
  }



  // getLastPODetails(ProductID: string, StoreID: number) {
  //   const formattedDate = this.sharedService.formatDateForSQL(this.selectedDate);

  //   if (ProductID != "" && ProductID.length >= 2) {
  //     this.dataService.getLastPODetails(ProductID, StoreID).then(data => {

  //       if (data) {
  //         this.POData = data;
  //         this.POData[0].Date = formattedDate;
  //         this.POData[0].UnitQty = data.UnitsOrdered;
  //         //this.getCaseQty(this.POData[0]);
  //       }
  //       console.log(this.POData);
  //     });
  //   }
  // }

  // getWarehouseTotalQtyByProductID(ProductID: string) {
  //   if (ProductID != "" && ProductID.length >= 3) {
  //     this.dataService.getWarehouseTotalQtyByProductID(ProductID).then(data => {
  //       if (data) {
  //         this.TotalQty = data;
  //       }
  //     });
  //   }
  // }

  getWarehouseProductByProductID(ProductID: string) {
    if (ProductID != "" && ProductID.length >= 2) {
      this.dataService.getProductInfoByProductID(ProductID).then(data => {
        if (data) {
          this.ProductData.ProductName = data.ProductName;
          this.DepartmentName = data.Department;
        }
      });
    }
  }

  getWarehouseProductByProductName(ProductName: string) {
    if (ProductName != "" && ProductName.length >= 2) {
      this.dataService.getProductInfoByProductName(ProductName).then(data => {
        if (data) {
          this.filteredProducts = data;
          //this.loadWarehouseData(this.ProductData.ProductID);
        }
      });
    }
  }

  getWarehouseProductQty(ProductID: string, Source: number) {
    console.log("getWarehouseProductQty");
    if (ProductID != "" && ProductID.length >= 3) {
      this.dataService.getWarehouseProductQty(ProductID, Source).then(data => {
        if (data) {
          this.TotalQty = data;
          console.log(this.TotalQty);
        }
      });
    }
  }

  getNormalizedID(ProductID: string) {
    if (ProductID != "" && ProductID != undefined && !isNaN(Number(ProductID))) {
      this.dataService.getNormalizedID(ProductID).then(data => {
        if (data.normalizedID.length > 0) {
          this.ProductData.ProductID = data.normalizedID;
          this.getWarehouseProductByProductID(this.ProductData.ProductID);
          this.loadWarehouseData(this.ProductData.ProductID);
        }
      });
    }
  }

  getLastPODetails(ProductID: string, StoreID: number) {
    if (ProductID != "" && ProductID.length >= 3) {
      this.dataService.getLastPODetails(ProductID, StoreID).then(data => {
        if (data.length > 0) {
          this.ProductData.SupplierID = data[0].SupplierID;
          this.SupplierName = data[0].CompanyName;
          this.UnitsPerPackage = data[0].UnitsPerPackage;
        }
      });
    }
  }

  addInWarehouse(ProductData: Warehouse) {
    this.dataService.addInWarehouse(ProductData).subscribe({
      next: (data) => {
        this.snackBar.open('Add in successful', 'Close', {
          duration: 3000,
        });
        this.reset();
      },
      error: (error) => {
        let errorMessage = error.message || 'addInWarehouse error occurred!';
        alert(errorMessage);
      }
    });
  }

  takeOutWarehouse(ProductData: Warehouse) {
    this.dataService.takeOutWarehouse(ProductData).subscribe({
      next: (data) => {
        this.snackBar.open('Take out successful', 'Close', {
          duration: 3000,
        });
        this.reset();
      },
      error: (error) => {
        let errorMessage = error.message || 'takeOutWarehouse error occurred!';
        alert(errorMessage);
      }
    });
  }






  // getWarehouseInventoryByProductName(ProductName: string) {
  //   if (ProductName != "" && ProductName.length >= 4) {
  //     this.dataService.getWarehouseInventoryByProductName(ProductName).then(data => {
  //       if (data.length > 0) {
  //         this.InventoryData = data;
  //       }
  //     });
  //   }
  // }
  // getWarehouseTransactionByProductName(ProductName: string) {
  //   if (ProductName != "" && ProductName.length >= 4) {
  //     this.dataService.getWarehouseTransactionByProductName(ProductName).then(data => {
  //       if (data.length > 0) {
  //         this.TransactionData = data;
  //       }
  //     });
  //   }
  // }

  // getBuyer() {
  //   this.dataService.getItemBuyer().subscribe(data => {
  //     this.listBuyer = data;
  //   })
  // }

  // changeSupplier() {
  //   this.isSupplierFound = false;
  // }

  // onProductIDChange() {
  //   if (!isNaN(Number(this.ProductData.ProductID))) {
  //     this.getNormalizedID(this.ProductData.ProductID);
  //   }
  // }

  // onInvoiceChange() {
  //   if (this.ProductData.Invoice.length > 0) {
  //     this.isInvoice = true;
  //     this.getWarehouseByInvoice();
  //   } else {
  //     this.isInvoice = false;
  //   }
  // }

  // getNormalizedID(ProductID: string) {
  //   if (ProductID != "" && ProductID != undefined && !isNaN(Number(ProductID))) {
  //     this.dataService.getNormalizedID(ProductID).then(data => {
  //       if (data.normalizedID.length > 0) {
  //         this.ProductData.ProductID = data.normalizedID;
  //         Promise.all([

  //         ])
  //       }
  //     });
  //   }
  // }

  // reset() {
  //   this.ProductData.ProductID = "";
  //   this.ProductData.ProductName = "";
  //   this.ProductData.CaseCost = undefined;
  //   this.ProductData.CaseQty = undefined;
  //   this.ProductData.UnitsPerPackage = undefined;
  //   this.ProductData.UnitCost = undefined;
  //   this.ProductData.UnitQty = undefined;
  // }

  // priceChange(changed: string) {
  //   const { CaseCost, UnitPrice, UnitsPerPackage, CaseQty, UnitQty } = this.ProductData;

  //   // Prevent calculations if UnitsPerPackage is zero or undefined to avoid division by zero
  //   if (UnitsPerPackage === 0 || UnitsPerPackage === undefined) return;

  //   switch (changed) {
  //     case 'UnitPrice':
  //       this.ProductData.CaseCost = UnitPrice !== undefined ? UnitPrice * UnitsPerPackage : undefined;
  //       break;
  //     case 'UnitQty':
  //       this.ProductData.CaseQty = UnitQty !== undefined ? UnitQty / UnitsPerPackage : undefined;
  //       break;
  //     case 'CaseCost':
  //       this.ProductData.UnitPrice = CaseCost !== undefined ? CaseCost / UnitsPerPackage : undefined;
  //       break;
  //     case 'CaseQty':
  //       this.ProductData.UnitQty = CaseQty !== undefined ? CaseQty * UnitsPerPackage : undefined;
  //       break;
  //     case 'UnitsPerPackage':
  //       if (UnitsPerPackage !== undefined) {
  //         this.ProductData.UnitPrice = CaseCost !== undefined ? CaseCost / UnitsPerPackage : undefined;
  //         this.ProductData.UnitQty = CaseQty !== undefined ? CaseQty * UnitsPerPackage : undefined;
  //       }
  //       break;
  //     default:
  //       console.warn('Unhandled case in priceChange');
  //   }
  // }

  // insertWarehouseInvoice() {
  //   if (!Number.isInteger(this.ProductData.UnitQty)) {
  //     alert("UnitQty should be integer.")
  //   } else if (!Number.isInteger(this.ProductData.CaseQty)) {
  //     alert("CaseQty should be integer.")
  //   } else {
  //     this.dataService.insertWarehouseInvoice(this.ProductData).subscribe({
  //       next: (data) => {
  //         this.reset();
  //         this.getWarehouseByInvoice();
  //         this.snackBar.open('Submit Successful', 'Close', {
  //           duration: 3000
  //         });
  //       },
  //       error: (error) => {
  //         let errorMessage = error.message || 'submitWarehouseItem error occurred!';
  //         alert(errorMessage);
  //       }
  //     });
  //   }
  // }

  // getWarehouseByInvoice() {
  //   if (this.ProductData.Invoice != null) {
  //     this.dataService.getWarehouseByInvoice(this.ProductData.Invoice, 0).subscribe({
  //       next: (data) => {
  //         this.listItem = data;
  //       },
  //       error: (error) => {
  //         let errorMessage = error.message || 'getWarehouseByInvoice error occurred!';
  //         alert(errorMessage);
  //       }
  //     });
  //   }

  // }

  // insertWarehouseItem() {
  //   this.dataService.insertWarehouseItem(this.ProductData).subscribe({
  //     next: (data) => {
  //       alert("Insert Successful");
  //       // this.reset();
  //     },
  //     error: (error) => {
  //       let errorMessage = error.message || 'insertWarehouseItem error occurred!';
  //       alert(errorMessage);
  //     }
  //   });
  // }

  // comfirmInvoice() {
  //   this.dataService.comfirmInvoice(this.ProductData.Invoice).subscribe({
  //     next: (data) => {
  //       alert("comfirm Successful");
  //     },
  //     error: (error) => {
  //       let errorMessage = error.message || 'comfirmInvoice error occurred!';
  //       alert(errorMessage);
  //     }
  //   });
  // }


}
