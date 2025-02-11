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
import * as XLSX from 'xlsx';
@Component({
    selector: 'app-warehouse-draft',
    templateUrl: './warehouse-draft.page.html',
    styleUrls: ['./warehouse-draft.page.scss'],
    standalone: false
})
export class WarehouseDraftPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  constructor(private router: Router, private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  OrderData: any;
  user: User = new User;
  StoreID!: number;
  OrderID: string = '01241100003';
  InvoiceID: string = '';
  // ProductID: string = '';
  AddTo!: number;

  totalCaseQty: number = 0;
  Source!: number;

  backgroundColor!: string;
  isChecked = false;
  isAddIn: boolean = true;
  //selectedDate: Date = new Date;

  SourceOptions = Object.entries({
    "OFC": 3,
    "OFMM": 7,
    "ALP": 39,
    "OFCD": 1
  }).map(([key, value]) => ({ key, value }));

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
    this.reset();
    this.loadUser();
    this.isChecked = false;
    this.ToggleAction(this.isChecked);
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
    this.StoreID = this.user.Store;
  }

  ToggleAction(event: any) {
    this.Source = 0;
    if (event.value) {
      this.isAddIn = false;
      this.backgroundColor = 'lightcoral';
    } else {
      this.isAddIn = true;
      this.backgroundColor = 'lightgreen';
    }
  }

  onOrderIDChange() {
    this.getOrderByOrderID(this.OrderID);
  }

  // onProductIDChange(){
  //   this.getLastPODetails(this.ProductID, this.StoreID);
  // }

  reset() {
    this.OrderData = [];
    this.totalCaseQty = 0;
  }

  addIn() {
    this.addInWarehouseBatch(this.OrderData);
  }

  takeOut() {
    this.takeOutWarehouseBatch(this.OrderData);
  }


  getCaseQty(item: any) {
    item.CaseQty = item.UnitQty / item.UnitsPerPackage;
    item.DestinationStockCaseQty = item.DestinationCurrentStock / item.UnitsPerPackage;
    item.SourceStockCaseQty = item.SourceCurrentStock / item.UnitsPerPackage;
    this.calculateTotalCaseQty();
  }

  getUnitQty(item: any) {
    item.UnitQty = item.CaseQty * item.UnitsPerPackage;
    this.calculateTotalCaseQty();
  }

  calculateTotalCaseQty() {
    this.totalCaseQty = this.OrderData.reduce((total: any, item: { CaseQty: any; }) => total + (item.CaseQty || 0), 0);
  }

  addSource() {
    this.OrderData.forEach((item: any) => {
      item.Source = this.Source;
    });
  }

  // searchResult(){
  //   if(this.ProductID != ""){
  //     this.getWarehouseProductByProductID(this.ProductID);
  //   }else{
  //     this.getPOByOrderID(this.OrderID, this.Source);
  //   }
  // }

  getOrderByOrderID(OrderID: string) {
    this.dataService.getOrderByOrderID(OrderID).then(data => {
      if (data) {
        this.OrderData = data.OrderItems;
        this.OrderData.forEach((item: any) => {
          console.log(item)
          this.getCaseQty(item);
        });

      }
    });
  }

  // getPOByOrderID(OrderID: string, Source?: number | undefined) {
  //   //const formattedDate = this.sharedService.formatDateForSQL(this.selectedDate);
  //   // 将格式化后的日期赋值给 ProductData.Date
  //   if (this.isAddIn) {
  //     Source = undefined;
  //   }

  //   this.dataService.getPOByOrderID(OrderID, Source).then(data => {
  //     if (data) {
  //       this.OrderData = data;
  //       this.OrderData.forEach((item: any) => {
  //         //item.Date = formattedDate;
  //         item.UnitQty = item.UnitsOrdered;
  //         this.getCaseQty(item);
  //       });
  //     }
  //   });
  // }

  // getWarehouseProductByProductID(ProductID: string) {
  //   const formattedDate = this.sharedService.formatDateForSQL(this.selectedDate);
  //   // 将格式化后的日期赋值给 ProductData.Date

  //   if (ProductID != "" && ProductID.length >= 2) {
  //     this.dataService.getWarehouseProductByProductID(ProductID).then(data => {
  //       if (data) {
  //         this.getLastPODetails(ProductID, this.StoreID);
  //         this.OrderData[0] = data;
  //         this.OrderData[0].Date = formattedDate;
  //         this.OrderData[0].UnitQty = data.UnitsOrdered;
  //         this.getCaseQty(this.OrderData[0]);
  //       }
  //       //console.log(this.OrderData);
  //     });
  //   }
  // }




  addInWarehouseBatch(OrderData: Warehouse) {
    this.addSource();
    console.log(OrderData)
    // this.dataService.addInWarehouseBatch(OrderData).subscribe({
    //   next: (data) => {
    //     this.snackBar.open(data.message, 'Close', {
    //       duration: 3000,
    //     });
    //     this.reset();
    //   },
    //   error: (error) => {
    //     let errorMessage = error.message || 'addInWarehouse error occurred!';
    //     alert(errorMessage);
    //   }
    // });
  }

  takeOutWarehouseBatch(OrderData: Warehouse) {
    this.addSource();
    console.log(OrderData)
    this.dataService.takeOutWarehouseBatch(OrderData).subscribe({
      next: (data) => {
        this.snackBar.open(data.message, 'Close', {
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

  @ViewChild('printSection', { static: false }) printSection!: ElementRef;
  printTable() {
    const printContent = document.getElementById('print-section');
    const WindowPrt = window.open('', '', 'width=900,height=650');
    if (WindowPrt && printContent) {
      const printContent = this.printSection.nativeElement.cloneNode(true);

      // 替换克隆内容中的所有 input 元素为其值的文本节点
      const inputs = printContent.querySelectorAll('input');
      inputs.forEach((input: HTMLInputElement) => {
        const parent = input.parentNode;
        if (parent) {
          const textNode = document.createTextNode(input.value);
          parent.replaceChild(textNode, input);
        }
      });

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
      WindowPrt.document.write('<img src="../assets/images/logoALP.png" alt="Logo" style="max-width: 60%; height: auto;">');
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

  // getWarehouseDrafts(StoreID: number) {
  //   this.dataService.getWarehouseDrafts(StoreID).then(data => {
  //     if (data) {
  //       console.log(data)
  //       this.draftData = data;
  //     }
  //   });
  // }

  // submitDraft(item: any) {
  //   console.log(item)
  // }



  // exportExcel() {
  //   const formattedData = this.draftData.map((item: { ProductID: any; UnitCost: any; ProductName: any; SupplierID: any; Applicant: any; Date: Date; Location: any; UnitsPerPackage: any; UnitQty: any; CaseQty: any; }) => ({
  //     ProductID: item.ProductID,
  //     UnitCost: item.UnitCost,
  //     ProductName: item.ProductName,
  //     SupplierID: item.SupplierID,
  //     Applicant: item.Applicant,
  //     Date: item.Date,
  //     Location: item.Location,
  //     UnitsPerPackage: item.UnitsPerPackage,
  //     UnitQty: item.UnitQty,
  //     CaseQty: item.CaseQty
  //   }));

  //   // 创建一个工作簿
  //   const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(formattedData);
  //   const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };

  //   const today = new Date();
  //   // 写入Excel文件
  //   const fileName = `WarehouseExportData_${this.getCurrentDate()}.xlsx`;
  //   XLSX.writeFile(workbook, fileName);
  // }

  // // formatDate(date: Date): string {
  // //   const d = new Date(date);
  // //   const year = d.getFullYear();
  // //   const month = (d.getMonth() + 1).toString().padStart(2, '0');
  // //   const day = d.getDate().toString().padStart(2, '0');
  // //   return `${year}-${month}-${day}`;
  // // }

  // getCurrentDate(): string {
  //   const today = new Date();
  //   const year = today.getFullYear();
  //   const month = (today.getMonth() + 1).toString().padStart(2, '0'); // 月份从0开始
  //   const day = today.getDate().toString().padStart(2, '0');
  //   return `${year}${month}${day}`;
  // }

}
