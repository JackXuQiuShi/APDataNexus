import { Component, OnInit, Renderer2 } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { PODetails } from 'src/assets/model/PODetails';
import { PODraft } from 'src/assets/model/PODraft';
import { Supplier } from 'src/assets/model/Supplier';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-po-draft',
    templateUrl: './po-draft.page.html',
    styleUrls: ['./po-draft.page.scss'],
    standalone: false
})
export class PoDraftPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  private sharedServiceSubscription!: Subscription;

  constructor(private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar,private router: Router) { }

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
      // this.supplierData.SupplierID = newData.SupplierID;
      // this.supplierData.CompanyName = newData.CompanyName;
      // this.isSupplierFound = !newData.SupplierID;
      // this.isSupplierFound = !!newData.SupplierID;
    });
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
  }

  user: User = new User;
  // supplierData: Supplier = new Supplier;
  isSupplierFound: boolean = false;
  frequencyList = ["Monthly", "Weekly"]
  frequency: string = "";
  supplierList: any = [];
  supplierListTest1: any = ["Supplier1", "Supplier2"];
  supplierListTest2: any = ["TestSupplier3", "Supplier4","AA1","AA2","AA3","BAA","CAA","DAA","FAA","CAA"];
  supplierListTest3: any = ["Supplier3", "Supplier4"];
  supplierListTest4: any = ["Supplier3", "Supplier4"];
  supplier: any;

  itemList: any;
  itemListTest1: any = [
    {
      ProductID: '0085369300100',
      ProductName: 'AppleAppleAppleAppleAppleApple',
      Location: 'A1-1',
      CaseQty: 10,
      UnitQty: 100,
      UnitCost: 0.5,
      UnitsPerPackage: 10,
      RetailPrice: 1.75,
      StockQty: 200,
      LastOrderDate: '2024-10-15',
      MonthlySales: 150,
      SupplierID: 200,
      StoreID: 3
    },
    {
      ProductID: '0085369300101',
      ProductName: 'Orange',
      Location: 'A1-1',
      CaseQty: 20,
      UnitQty: 150,
      UnitCost: 0.6,
      UnitsPerPackage: 15,
      RetailPrice: 1,
      StockQty: 300,
      LastOrderDate: '2024-10-10',
      MonthlySales: 100,
      SupplierID: 200,
      StoreID: 3
    },
    {
      ProductID: '0085369300102',
      ProductName: 'Banana',
      Location: 'A1-3',
      CaseQty: 15,
      UnitQty: 120,
      UnitCost: 0.4,
      UnitsPerPackage: 12,
      RetailPrice: 0.5,
      StockQty: 250,
      LastOrderDate: '2024-10-20',
      MonthlySales: 80,
      SupplierID: 200,
      StoreID: 3
    },
    {
      ProductID: '0085369300104',
      ProductName: 'Strawberry',
      Location: 'A1-4',
      CaseQty: 8,
      UnitQty: 80,
      UnitCost: 1.0,
      UnitsPerPackage: 8,
      RetailPrice: 1.1,
      StockQty: 100,
      LastOrderDate: '2024-10-18',
      MonthlySales: 60,
      SupplierID: 200,
      StoreID: 3
    },
    {
      ProductID: '0085369300106',
      ProductName: 'Grapes',
      Location: 'A1-6',
      CaseQty: 12,
      UnitQty: 90,
      UnitCost: 0.8,
      UnitsPerPackage: 10,
      RetailPrice: 0.9,
      StockQty: 150,
      LastOrderDate: '2024-10-22',
      MonthlySales: 90,
      SupplierID: 200,
      StoreID: 3
    }
  ];

  itemListTest2: any = [
    {
      ProductID: '0085369300100',
      ProductName: 'AppleAppleAppleAppleAppleApple',
      Location: 'A1-1',
      CaseQty: 10,
      UnitQty: 100,
      UnitCost: 0.5,
      UnitsPerPackage: 10,
      RetailPrice: 0.75,
      StockQty: 200,
      LastOrderDate: '2024-10-15',
      MonthlySales: 150,
      SupplierID: 2,
      StoreID: 3
    },
    {
      ProductID: '0085369300101',
      ProductName: 'Orange',
      Location: 'A2-1',
      CaseQty: 20,
      UnitQty: 150,
      UnitCost: 0.6,
      UnitsPerPackage: 15,
      RetailPrice: 0.9,
      StockQty: 300,
      LastOrderDate: '2024-10-10',
      MonthlySales: 100,
      SupplierID: 2,
      StoreID: 3
    }
  ];

  reset() {

  }

  changeSupplier() {
    this.isSupplierFound = false;
  }

  deleteItem(item: any) {
    console.log(item)
    this.itemList = this.itemList.filter((i: any) => i !== item); // 从 itemList 中移除该项
    this.snackBar.open(item.ProductID + " " + item.ProductName + ' Deleted', 'Close', {
      duration: 5000,
    });
  }

  onCaseQtyChange(item: any) {
    item.CaseQty = item.UnitQty / item.UnitsPerPackage;
  }

  onUnitQtyChange(item: any) {
    item.UnitQty = item.CaseQty * item.UnitsPerPackage;
  }

  getMargin(item: any) {
    return ((item.RetailPrice - item.UnitCost) / item.UnitCost);
  }

  onSupplierChange(supplier: any) {
    this.itemList = [];
    if (supplier == "Supplier1") {
      this.itemList = this.itemListTest1;
    } else {
      this.itemList = this.itemListTest2;
    }
  }

  onFrequencyChange(frequency: string) {
    this.itemList = [];
    this.supplierList = [];
    if (frequency == "Monthly") {
      this.supplierList = this.supplierListTest1;
    } else {
      this.supplierList = this.supplierListTest2;
    }
  }

  submit() {
    console.log(this.itemList);
    this.router.navigate(['/po/po-details'], { queryParams: { OrderID: "032430942" } });
  }

  // private routeParamsSubscription!: Subscription;

  // user: User = new User;
  // poDraft: PODraft = new PODraft;
  // NormalizedID: string = '';
  // isSupplierFound: boolean = true;

  // constructor(private router: Router, private dataService: DataService, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) { }

  // async ngOnInit() {
  //   await this.dataService.loadConfig();
  //   this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
  //     this.refreshData();
  //   });
  // }

  // ngOnDestroy(): void {
  //   if (this.routeParamsSubscription) {
  //     this.routeParamsSubscription.unsubscribe();
  //   }

  // }

  // ionViewDidLeave() {
  //   this.reset();
  // }

  // private refreshData() {
  //   this.sharedService.observableData.subscribe((newData: any) => {
  //     // this.reset();
  //     this.poDraft.SupplierID = newData.SupplierID;
  //     this.isSupplierFound = !!newData.SupplierID;
  //     this.poDraft.CompanyName = newData.CompanyName;
  //   });
  //   this.isSupplierFound = true;
  //   this.loadUser();
  // }

  // private loadUser() {
  //   const userString = sessionStorage.getItem('user');
  //   if (userString) {
  //     this.user = JSON.parse(userString);
  //     this.poDraft.StoreID = this.user.Store;
  //   }
  // }

  // changeSupplier() {
  //   this.isSupplierFound = false;
  // }

  // onProductIDChange() {
  //   if (this.poDraft.ProductID != undefined) {
  //     this.poDraft.ProductID = this.poDraft.ProductID.trim();
  //   }
  //   this.getNormalizedID();
  // }


  // priceChange(changed: string) {
  //   const { CaseCost, UnitCost, UnitsPerPackage, CaseQty, UnitQty } = this.poDraft;

  //   // Prevent calculations if UnitsPerPackage is zero or undefined to avoid division by zero
  //   if (UnitsPerPackage === 0 || UnitsPerPackage === undefined) return;

  //   switch (changed) {
  //     case 'UnitCost':
  //       this.poDraft.CaseCost = UnitCost !== undefined ? UnitCost * UnitsPerPackage : undefined;
  //       break;
  //     case 'UnitQty':
  //       this.poDraft.CaseQty = UnitQty !== undefined ? UnitQty / UnitsPerPackage : undefined;
  //       break;
  //     case 'CaseCost':
  //       this.poDraft.UnitPrice = CaseCost !== undefined ? CaseCost / UnitsPerPackage : undefined;
  //       break;
  //     case 'CaseQty':
  //       this.poDraft.UnitQty = CaseQty !== undefined ? CaseQty * UnitsPerPackage : undefined;
  //       break;
  //     case 'UnitsPerPackage':
  //       if (UnitsPerPackage !== undefined) {
  //         this.poDraft.UnitPrice = CaseCost !== undefined ? CaseCost / UnitsPerPackage : undefined;
  //         this.poDraft.UnitQty = CaseQty !== undefined ? CaseQty * UnitsPerPackage : undefined;
  //       }
  //       break;
  //     default:
  //       console.warn('Unhandled case in priceChange');
  //   }
  // }


  // reset() {
  //   this.NormalizedID = "";
  //   this.isSupplierFound = true;
  //   this.poDraft = new PODraft;
  //   this.loadUser();
  // }

  // submitPODraft() {
  //   console.log(this.poDraft)
  // }


  // getNormalizedID() {
  //   if (this.poDraft.ProductID.trim() != "" && this.poDraft.ProductID.trim() != undefined && !isNaN(Number(this.poDraft.ProductID.trim()))) {
  //     this.dataService.getNormalizedID(this.poDraft.ProductID).then(data => {
  //       if (data.normalizedID.length > 0) {
  //         this.NormalizedID = data.normalizedID;
  //         Promise.all([
  //           this.getLastPODetails(this.NormalizedID, this.poDraft.StoreID),
  //           this.getRegPrice(this.NormalizedID, this.poDraft.StoreID),
  //           this.getProductInfoByProductID(this.NormalizedID)
  //         ])
  //       }
  //     });
  //   }
  // }

  // getLastPODetails(NormalizedID: string, StoreID: number) {
  //   this.dataService.getLastPODetails(NormalizedID, StoreID).then(data => {
  //     if (data.length > 0) {
  //       this.poDraft.UnitQty = data[0].UnitsOrdered;
  //       this.poDraft.UnitCost = data[0].PriceOrdered;
  //       this.poDraft.UnitsPerPackage = data[0].UnitsPerPackage;
  //       this.poDraft.TaxRate = data[0].TaxRate;
  //       this.poDraft.OrderingDate = data[0].OrderingDate;
  //       this.poDraft.SupplierID = data[0].SupplierID;
  //       this.poDraft.CompanyName = data[0].CompanyName;
  //       this.poDraft.CaseCost = data[0].UnitsPerPackage * data[0].PriceOrdered;
  //       this.poDraft.CaseQty = data[0].UnitsOrdered / data[0].UnitsPerPackage;
  //     }
  //   });
  // }

  // getRegPrice(NormalizedID: string, StoreID: number) {
  //   this.dataService.getRegPrice(NormalizedID, StoreID).then(data => {
  //     if (data.length > 0) {
  //       this.poDraft.RegPrice = data[0].RegPrice;
  //     }
  //   });
  // }

  // getProductInfoByProductID(NormalizedID: string) {
  //   this.dataService.getProductInfoByProductID(NormalizedID).then(data => {
  //     if (data.length > 0) {
  //       this.poDraft.ProductName = data[0].ProductName;
  //       this.poDraft.Department = data[0].Department;
  //     }
  //   });
  // }






}
