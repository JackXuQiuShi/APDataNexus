import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
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
    selector: 'app-warehouse',
    templateUrl: './warehouse.page.html',
    styleUrls: ['./warehouse.page.scss'],
    standalone: false
})
export class WarehousePage implements OnInit {

  private routeParamsSubscription!: Subscription;
  constructor(private router: Router, private dataService: DataService, private renderer: Renderer2, private route: ActivatedRoute) { }

  transactionData: any;
  inventoryData: any;
  user: User = new User;
  
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
    this.getWarehouseTransactionSummary();
    this.getWarehouseInventory();
  }

  getWarehouseTransactionSummary() {
    this.dataService.getWarehouseTransactionSummary().then(data => {
      console.log(data)
      if (data) {
        this.transactionData = data;
      }
    });
  }

  getWarehouseInventory(){
    this.dataService.getWarehouseInventory().then(data => {
      console.log(data)
      if (data) {
        this.inventoryData = data;
      }
    });
  }


  // getWarehouseTransactionByProductID(POID: string) {
  //   this.dataService.getWarehouseTransactionByProductID(POID).then(data => {
  //     if (data) {
  //       this.POData = data;
  //     }
  //   });
  // }

  // exportExcel() {
  //   const formattedData = this.data.map((item: { ProductID: any; UnitCost: any; ProductName: any; SupplierID: any; Applicant: any; Date: Date; Location: any; UnitsPerPackage: any; UnitQty: any; CaseQty: any; }) => ({
  //     ProductID: item.ProductID,
  //     UnitCost: item.UnitCost,
  //     ProductName: item.ProductName,
  //     SupplierID: item.SupplierID,
  //     Applicant: item.Applicant,
  //     Date: this.formatDate(item.Date),
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

  // formatDate(date: Date): string {
  //   const d = new Date(date);
  //   const year = d.getFullYear();
  //   const month = (d.getMonth() + 1).toString().padStart(2, '0'); 
  //   const day = d.getDate().toString().padStart(2, '0');
  //   return `${year}-${month}-${day}`;
  // }

  // getCurrentDate(): string {
  //   const today = new Date();
  //   const year = today.getFullYear();
  //   const month = (today.getMonth() + 1).toString().padStart(2, '0'); // 月份从0开始
  //   const day = today.getDate().toString().padStart(2, '0');
  //   return `${year}${month}${day}`;
  // }

  // getWarehouseStorage() {
  //   this.dataService.getWarehouseStorage().subscribe({
  //     next: (data) => {
  //       if (data.length > 0) {
  //         this.data = data;
  //       }
  //     },
  //     error: (error) => {
  //       let errorMessage = error.message || 'getWarehouseStorage error occurred!';
  //       alert(errorMessage);
  //     }
  //   });
  // }






  // selectedDate: Date = new Date();
  // formatedDate: any;

  // getWarehouseList() {
  //   if (this.selectedDate) {
  //     this.formatedDate = this.datePipe.transform(this.selectedDate, 'yyyy-MM-dd');

  //     this.sharedService.getSupplierRequest("").then(data =>
  //       this.data = data
  //     );
  //   }

  //   console.log(this.formatedDate);
  // }

  // selectItem(item: any){
  //   console.log(item);
  // }

}
