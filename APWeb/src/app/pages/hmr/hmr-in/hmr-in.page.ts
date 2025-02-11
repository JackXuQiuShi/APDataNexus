import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { HMR } from 'src/assets/model/HMR';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-hmr-in',
    templateUrl: './hmr-in.page.html',
    styleUrls: ['./hmr-in.page.scss'],
    standalone: false
})
export class HmrInPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  ProductData: HMR = new HMR;
  InventoryData: HMR[] = [];
  TransactionData: HMR[] = [];


  NormalizedID!: string;
  user: User = new User;
  action!: string;
  actionList = ["Add In", "Take Out"];
  backgroundColor!: string;
  isAddIn: boolean = true;
  isChecked = false;
  isSupplierFound: boolean = true;

  ProductID: string = "";
  ProductName: string = "";
  TotalQty: number = 0;

  SellToList = ["OFC", "ALP", "OFMM", "OFCD"];
  filteredProducts: HMR[] = [];
  selectedDate: Date = new Date;

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

      this.ProductData.SupplierName = newData.CompanyName;
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

  //用户选择某个产品时的操作
  selectProduct(product: any) {
    this.ProductData = product;
    this.loadUser();
    Promise.all([
      this.getHMRInventoryByProductID(this.ProductData.ProductID),
      this.getHMRTotalQtyByProductID(this.ProductData.ProductID),
      this.getLastPODetails(this.ProductData.ProductID, this.ProductData.StoreID)
    ])

    this.filteredProducts = [];
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
    console.log("Add:")
    console.log(this.ProductData)
    this.addInHMR(this.ProductData);
  }

  takeOut() {
    console.log("Take:")
    console.log(this.ProductData);
    this.takeOutHMR(this.ProductData);
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
    this.ProductData = new HMR;
    this.TotalQty = 0;
  }

  onProductIDChange() {
    this.InventoryData = [];
    if (this.ProductData.ProductID != undefined) {
      this.ProductData.ProductID = this.ProductData.ProductID.trim();
    }

    this.ProductID = this.ProductData.ProductID;
    this.reset();
    this.loadUser();
    this.ProductData.ProductID = this.ProductID;
    this.getHMRProductByProductID(this.ProductData.ProductID);
    console.log(this.ProductData)
    Promise.all([
      this.getHMRInventoryByProductID(this.ProductData.ProductID),
      this.getHMRTotalQtyByProductID(this.ProductData.ProductID),
      this.getLastPODetails(this.ProductData.ProductID, this.ProductData.StoreID)
    ])


  }

  onProductNameChange() {
    this.InventoryData = [];
    if (this.ProductData.ProductName != undefined) {
      this.ProductData.ProductName = this.ProductData.ProductName.trim();
    }

    this.ProductName = this.ProductData.ProductName;
    this.reset();
    this.ProductData.ProductName = this.ProductName;
    this.getHMRProductByProductName(this.ProductData.ProductName);
  }

  getHMRProductByProductID(ProductID: string) {
    if (ProductID != "" && ProductID.length >= 2) {
      this.dataService.getHMRProductByProductID(ProductID).then(data => {
        if (data) {
          this.ProductData = data;
        }
      });
    }
  }

  getHMRProductByProductName(ProductName: string) {
    if (ProductName != "" && ProductName.length >= 2) {
      this.dataService.getHMRProductByProductName(ProductName).then(data => {
        if (data) {
          this.filteredProducts = data;
        }
      });
    }
  }

  getHMRInventoryByProductID(ProductID: string) {
    if (ProductID != "" && ProductID.length >= 3) {
      this.dataService.getHMRInventoryByProductID(ProductID).then(data => {
        if (data.length > 0) {
          this.InventoryData = data;
        }
      });
    }
  }

  // getHMRTransactionByProductID(ProductID: string) {
  //   if (ProductID != "" && ProductID.length >= 3) {
  //     this.dataService.getHMRTransactionByProductID(ProductID).then(data => {
  //       if (data.length > 0) {
  //         this.TransactionData = data;
  //       }
  //     });
  //   }
  // }

  getHMRTotalQtyByProductID(ProductID: string) {
    if (ProductID != "" && ProductID.length >= 3) {
      this.dataService.getHMRTotalQtyByProductID(ProductID).then(data => {
        if (data) {
          this.TotalQty = data;
        }
      });
    }
  }

  getLastPODetails(ProductID: string, StoreID: number) {
    if (ProductID != "" && ProductID.length >= 3) {
      this.dataService.getLastPODetails(ProductID, StoreID).then(data => {
        if (data.length > 0) {
          this.ProductData.SupplierID = data[0].SupplierID;
          this.ProductData.SupplierName = data[0].CompanyName;
        }
      });
    }
  }

  addInHMR(ProductData: HMR) {
    this.dataService.addInHMR(ProductData).subscribe({
      next: (data) => {
        this.snackBar.open('Add in successful', 'Close', {
          duration: 3000,
        });
        this.reset();
      },
      error: (error) => {
        let errorMessage = error.message || 'addInHMR error occurred!';
        alert(errorMessage);
      }
    });
  }

  takeOutHMR(ProductData: HMR) {
    this.dataService.takeOutHMR(ProductData).subscribe({
      next: (data) => {
        this.snackBar.open('Take out successful', 'Close', {
          duration: 3000,
        });
        this.reset();
      },
      error: (error) => {
        let errorMessage = error.message || 'takeOutHMR error occurred!';
        alert(errorMessage);
      }
    });
  }

}
