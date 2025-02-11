import { Component, OnInit } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { LastReceivingData } from 'src/assets/model/LastReceivingData';
import { PO } from 'src/assets/model/PO';
import { User } from 'src/assets/model/User';

@Component({
  selector: 'app-po-details',
  templateUrl: './po-details.page.html',
  styleUrls: ['./po-details.page.scss'],
  standalone: false
})
export class PoDetailsPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  OrderID: string = '';
  ProductID: string = '';

  OrderDetailsData: any = [];
  OrderData: any = new PO;
  OrderStatus: string = "";
  user: User = new User;
  totalCaseQty: number = 0;
  totalUnitQty: number = 0;
  totalCost: number = 0;

  isAllSelected = false;
  selectedRows: Set<string> = new Set();
  listItems: any = [];
  expandedProductID: string | null = null;

  lastReceivingData: LastReceivingData = new LastReceivingData;
  RetailPrice!: number;

  searchText: string = '';
  searchProductID: string = '';
  filteredOrderDetailsData: any[] = [];

  constructor(private route: ActivatedRoute, private sharedService: SharedService, private dataService: DataService, private router: Router, private snackBar: MatSnackBar, private modalController: ModalController) {
  }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.OrderID = params['OrderID'];
      this.refreshData();
    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
  }

  ionViewDidLeave() {
    this.reset();
  }

  refreshData() {
    this.loadUser();
    this.loadOrderByOrderID(this.OrderID);
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
  }

  reset() {
    this.OrderStatus = "";
    this.OrderDetailsData = [];
    this.OrderData = "";
    this.resetData();
    this.loadUser();
  }

  resetData() {
    this.lastReceivingData = new LastReceivingData;
    this.RetailPrice = 0;
    this.totalCaseQty = 0;
    this.totalUnitQty = 0;
    this.totalCost = 0;
  }

  toggleFooter(ProductID: string): void {
    this.loadLastPODetails(ProductID, this.user.Store);
    this.loadRegPrice(ProductID, this.user.Store);
    this.expandedProductID = this.expandedProductID === ProductID ? null : ProductID;
  }

  onCaseQtyChange(item: any) {
    if (item.CaseQty != null && item.UnitsPerPackage > 0) {
      item.UnitQty = parseFloat((item.CaseQty * item.UnitsPerPackage).toFixed(2)); // 根据 CaseQty 计算 UnitQty
    }
    this.calculateTotals();
  }

  onUnitQtyChange(item: any) {
    if (item.UnitQty != null && item.UnitsPerPackage > 0) {
      item.CaseQty = parseFloat((item.UnitQty / item.UnitsPerPackage).toFixed(2)); // 根据 UnitQty 计算 CaseQty
    }
    this.calculateTotals();
  }

  onCaseCostChange(value: any, item: any) {
    const caseCost = parseFloat(value); // 直接使用传递的值
    if (!isNaN(caseCost) && item.UnitsPerPackage > 0) {
      item.CaseCost = parseFloat(caseCost.toFixed(2)); // 更新 CaseCost
      item.UnitCost = parseFloat((item.CaseCost / item.UnitsPerPackage).toFixed(3)); // 更新 UnitCost
    }
    this.calculateTotals();
  }

  onUnitCostChange(item: any) {
    if (item.UnitCost != null && item.UnitsPerPackage > 0) {
      item.UnitCost = parseFloat(item.UnitCost.toFixed(2)); // 确保 UnitCost 为两位小数
      item.CaseCost = parseFloat((item.UnitCost * item.UnitsPerPackage).toFixed(2)); // 根据 UnitCost 计算 CaseCost
    }
    this.calculateTotals();
  }

  onUnitsPerPackageChange(item: any) {
    const units = item.UnitsPerPackage || 0;

    if (units > 0) {
      // 动态计算 UnitQty 和 CaseQty
      if (item.CaseQty != null) {
        item.UnitQty = parseFloat((item.CaseQty * units).toFixed(2));
      }
      if (item.UnitQty != null) {
        item.CaseQty = parseFloat((item.UnitQty / units).toFixed(2));
      }

      // 如果 CaseCost 为空，则根据 UnitCost 初始化
      if (item.CaseCost == null && item.UnitCost != null) {
        item.CaseCost = parseFloat((item.UnitCost * units).toFixed(2));
      }

      // 如果 UnitCost 为空，则根据 CaseCost 初始化
      if (item.UnitCost == null && item.CaseCost != null) {
        item.UnitCost = parseFloat((item.CaseCost / units).toFixed(2));
      }

      // 更新库存相关字段
      if (item.SourceCurrentStock != null) {
        item.SourceStockCaseQty = parseFloat((item.SourceCurrentStock / units).toFixed(2));
      }
      if (item.DestinationCurrentStock != null) {
        item.DestinationStockCaseQty = parseFloat((item.DestinationCurrentStock / units).toFixed(2));
      }
    }

    this.calculateTotals();
  }

  calculateTotals() {
    // 初始化总计
    const totals = this.OrderDetailsData.reduce((acc: { totalCaseQty: any; totalUnitQty: any; totalCost: number; }, item: { CaseQty: number; UnitQty: number; UnitCost: number; }) => {
      const caseQty = item.CaseQty || 0;
      const unitQty = item.UnitQty || 0;
      const unitCost = item.UnitCost || 0;

      acc.totalCaseQty += caseQty;
      acc.totalUnitQty += unitQty;
      acc.totalCost += unitQty * unitCost;

      return acc;
    }, {
      totalCaseQty: 0,
      totalUnitQty: 0,
      totalCost: 0,
    });

    // 将计算结果保存到类属性中
    this.totalCaseQty = totals.totalCaseQty;
    this.totalUnitQty = totals.totalUnitQty;
    this.totalCost = totals.totalCost;
  }

  totalOrderedQty: number = 0;
  totalOrderedCost: number = 0;
  calculateOrdered() {
    this.totalOrderedQty = this.OrderDetailsData.reduce((total: any, item: { OrderedUnitQty: any; }) => total + (item.OrderedUnitQty || 0), 0);
    this.totalOrderedCost = this.OrderDetailsData.reduce((total: any, item: { OrderedUnitCost: any; OrderedUnitQty: any }) => total + (item.OrderedUnitCost * item.OrderedUnitQty || 0), 0);
  }



  isItemCompleted(item: any): boolean {
    return item.ProductMovementItemStatusID == 2 || item.ProductMovementItemStatusID == 3;
  }

  getMargin(item: any) {
    return ((this.RetailPrice - item.UnitCost) / item.UnitCost);
  }

  setOrderStatus(OrderData: any) {
    const OrderStatusMap: { [key: number]: string } = {
      1: "Draft",
      2: "Receiving",
      3: "Completed"
    };

    this.OrderStatus = OrderStatusMap[OrderData.OrderStatusID] || "Unknown";
  }

  async loadOrderByOrderID(OrderID: string) {
    if (!OrderID) return;
    const data = await this.dataService.getOrderByOrderID(OrderID);
    if (data) {
      this.OrderData = data;
      this.OrderDetailsData = data.OrderItems || [];
      this.filteredOrderDetailsData = data.OrderItems || [];
      this.setOrderStatus(this.OrderData);
      this.initializeOrderItems(); // 将初始化逻辑集中
    }
  }

  private initializeOrderItems(): void {
    this.OrderDetailsData.forEach((item: any) => {
      item.SupplierID = this.OrderData.SupplierID;
      item.StoreID = this.user.Store;

      item.OrderedUnitQty = item.UnitQty;
      item.OrderedUnitCost = item.UnitCost;
      this.calculateOrdered();
      if (item.ProductMovementUnitCost != 0) {
        item.UnitCost = item.ProductMovementUnitCost;
      }

      if (item.ProductMovementUnitQty != 0) {
        item.UnitQty = item.ProductMovementUnitQty;
      }
      item.CaseCost = parseFloat((item.UnitCost * item.UnitsPerPackage).toFixed(2));
      item.CaseQty = parseFloat((item.UnitQty / item.UnitsPerPackage).toFixed(2));

      this.isItemCompleted(item); // 检查完成状态
    });

    // 数据处理完成后计算总数值
    this.calculateTotals();
  }

  async loadLastPODetails(ProductID: string, StoreID: number) {
    const data = await this.dataService.getLastPODetails(ProductID, StoreID);
    if (data.length > 0) {
      this.lastReceivingData = data[0];
    }
  }

  async loadRegPrice(ProductID: string, StoreID: number) {
    const data = await this.dataService.getRegPrice(ProductID, StoreID);
    if (data.length > 0) {
      this.RetailPrice = data[0].RegPrice;
    }
  }


  receiveOrderItem(item: any) {
    delete this.OrderData.OrderItems;
    const mergedObj = { ...this.OrderData, ...item };
    mergedObj.ProductMovementItemStatusID = 2;
    console.log(mergedObj);
    this.dataService.updateProductMovementItem(mergedObj).subscribe({
      next: (data) => {
        this.snackBar.open(data.Message, 'Close', {
          duration: 3000,
        });
        this.resetData();
        this.loadOrderByOrderID(this.OrderID);
      },
      error: (error) => {
        let errorMessage = error.message || 'receiveOrderItem error occurred!';
        alert(errorMessage);
      }
    });
  }


  confirmOrder() {
    console.log(this.OrderData)
    this.confirmAction(() => {
      this.dataService.confirmOrder(this.OrderData.OrderID).subscribe({
        next: (data) => {
          this.snackBar.open('confirmOrder Successful!', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/po']);
        },
        error: (error) => {
          let errorMessage = error.message || 'confirmOrder error occurred!';
          alert(errorMessage);
        }
      });
    })
  }

  draftOrderItem(item: any) {
    if (item.UnitQty > 0 && item.CaseQty > 0 && item.UnitsPerPackage > 0 && item.UnitCost > 0 && item.OrderItemStatusID == 1) {
      this.dataService.draftOrderItem(item).subscribe({
        next: (data) => {
          this.snackBar.open('Update Successful', 'Close', {
            duration: 3000,
          });
        },
        error: (error) => {
          let errorMessage = error.message || 'draftOrderItem error occurred!';
          alert(errorMessage);
        }
      });
    }
  }


  addNewOrderItem() {
    this.router.navigate(['/po/po-insert'], {
      queryParams: {
        OrderID: this.OrderData.OrderID,
        SupplierName: this.OrderData.SupplierName,
        SupplierID: this.OrderData.SupplierID
      }
    });
  }

  deleteOrderItem(item: any) {
    this.confirmAction(() => {
      this.dataService.deleteOrderItem(item).subscribe({
        next: (data) => {
          this.snackBar.open(item.ProductID + " " + item.ProductName + ' Deleted', 'Close', {
            duration: 3000,
          });
          this.reset();
          this.loadOrderByOrderID(this.OrderID);
        },
        error: (error) => {
          let errorMessage = error.message || 'deleteOrderItem error occurred!';
          alert(errorMessage);
        }
      });
    })
  }


  async confirmAction(action: () => void) {
    const modal = await this.modalController.create({
      component: ConfirmationComponent,
      componentProps: {
        message: 'Are you sure you want to proceed?'
      },
      cssClass: 'custom-modal',
    });

    modal.onDidDismiss().then(result => {
      if (result.data === true) {
        action();
      }
    });
    return await modal.present();
  }


  print() {
    const printContent = document.getElementById('print-section');
    // this.printTableToPDF();
    const WindowPrt = window.open('', '', 'width=900,height=650');
    if (WindowPrt && printContent) {
      WindowPrt.document.write('<html><head><title>Print</title>');
      WindowPrt.document.write('<style>');
      WindowPrt.document.write('table { width: 100%; border-collapse: collapse; }');
      WindowPrt.document.write('th, td { border: 1px solid #ddd; padding: 8px; }');
      WindowPrt.document.write('th { background-color: #f2f2f2; text-align: left; }');
      WindowPrt.document.write('.container { display: flex; flex-direction: column; align-items: flex-end; text-align: right; }');
      WindowPrt.document.write('.info { display: flex; justify-content: space-between; width: 100%; max-width: 800px; }');
      WindowPrt.document.write('.left-info { flex: 95%; margin: 10px; }');
      WindowPrt.document.write('.right-info { flex: 5%; margin: 10px; }');
      WindowPrt.document.write('</style>');
      WindowPrt.document.write('</head><body>');
      WindowPrt.document.write('<div style="text-align: center; margin-bottom: 20px;">');
      WindowPrt.document.write('<img src="/assets/images/logoALP.png" alt="Logo" style="max-width: 60%; height: auto;">');
      WindowPrt.document.write('</div>');
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.write('</body></html>');
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();

    } else {
      alert('Failed to open print window. Please check your browser settings.');
    }
  }


  filterProducts() {
    // 如果两个搜索框都为空，显示所有产品
    if ((!this.searchText || this.searchText.trim() === '') &&
      (!this.searchProductID || this.searchProductID.trim() === '')) {
      this.filteredOrderDetailsData = this.OrderDetailsData;
      return;
    }

    this.filteredOrderDetailsData = this.OrderDetailsData.filter((item: { ProductName: string; ProductID: string; }) => {
      const matchesName = !this.searchText ||
        item.ProductName.toLowerCase().includes(this.searchText.toLowerCase().trim());
      const matchesId = !this.searchProductID ||
        item.ProductID.toLowerCase().includes(this.searchProductID.toLowerCase().trim());

      // 两个条件都满足才显示
      return matchesName && matchesId;
    });
  }

  clearSearch() {
    this.searchText = '';
    this.filterProducts();
  }

  clearProductIDSearch() {
    this.searchProductID = '';
    this.filterProducts();
  }



}
