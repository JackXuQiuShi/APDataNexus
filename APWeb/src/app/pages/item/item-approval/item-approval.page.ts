import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Department, Departments } from 'src/assets/model/Department';
import { Item } from 'src/assets/model/Item';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { ModalController } from '@ionic/angular';


@Component({
    selector: 'app-item-approval',
    templateUrl: './item-approval.page.html',
    styleUrls: ['./item-approval.page.scss'],
    standalone: false
})
export class ItemApprovalPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private sharedService: SharedService, private dataService: DataService, private router: Router, private route: ActivatedRoute, private modalController: ModalController, private cdr: ChangeDetectorRef) {
  }

  data: any;
  dtTrigger: Subject<any> = new Subject<any>();
  selectedRows: Set<string> = new Set();
  isAllSelected = false;
  Status: number = 1;
  StatusID: number = 1;
  listProductID: string[] = [];
  StoreID: number = 0;

  listStatus = [
    { key: "Pending", value: 1 },
    { key: "Submited", value: 2 },
    { key: "Rejected", value: -1 }
  ];

  listStores = [
    { key: "ALL", value: 0 },
    { key: "ALP", value: 39 },
    { key: "OFMM", value: 7 },
    { key: "OFC", value: 3 },
    { key: "1080", value: 80 },
    { key: "3652", value: 52 }
  ];


  dtOptions: any = {};

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.dtOptions = {
      pagingType: 'full_numbers',
      paging: false,
      destroy: true
    }

    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.refreshData();
    });

  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
    this.dtTrigger.unsubscribe();
  }

  private refreshData() {
    this.clearSelections();
    this.getItemRequest();
  }

  private clearSelections() {
    this.selectedRows.clear();
    this.data = [];
    this.isAllSelected = false;
    $('#tableID').DataTable().clear().draw();
    $('#tableID').DataTable().destroy();
  }

  getItemRequest() {
    this.StatusID = this.Status;
    this.clearSelections();

    this.dataService.getItemRequest(this.StatusID, this.StoreID !== 0 ? this.StoreID : undefined).then(data => {
      if (data) {
        this.data = data.map((item: { DepartmentID: number; }) => {
          const departmentName = this.getDepartmentName(item.DepartmentID);
          return { ...item, DepartmentName: departmentName };
        });
        this.dtTrigger.next(null);
      }
    });
  }

  getDepartmentName(departmentID: number): string {
    const department = Departments.find(dep => dep.DepartmentID === departmentID);
    return department ? department.DepartmentName : 'Unknown Department';
  }

  selectAll() {
    if (this.isAllSelected || this.selectedRows.size === this.data.length) {
      // 如果已经全选或所有项都已选中，则清空selectedStoreRows实现全不选
      this.selectedRows.clear();
      this.isAllSelected = false; // 重置全选标志
    } else {
      // 否则，添加所有项到selectedStoreRows中实现全选
      this.data.forEach((product: any) => this.selectedRows.add(product.ProductID));
      this.isAllSelected = true; // 设置全选标志
    }
  }

  toggleSelection(ProductID: string) {
    if (this.selectedRows.has(ProductID)) {
      this.selectedRows.delete(ProductID);
    } else {
      this.selectedRows.add(ProductID);
    }
  }



  selectItem(item: Item) {

  }


  addSelectRowsToList() {
    this.listProductID = [];
    this.listProductID = Array.from(this.selectedRows);
  }

  approveNewProducts() {
    this.addSelectRowsToList();

    if (this.listProductID.length != 0) {
      this.confirmAction(() => {
        this.dataService.approveNewProducts(this.listProductID).subscribe({
          next: (data) => {
            this.refreshData();
            alert("Done");
          },
          error: (error) => {
            let errorMessage = error.message || 'approveNewProducts error occurred!';
            alert(errorMessage);
          }
        });
      })
    }
  }

  rejectNewProducts() {
    this.addSelectRowsToList();

    if (this.listProductID.length != 0) {
      this.confirmAction(() => {
        this.dataService.rejectNewProducts(this.listProductID).subscribe({
          next: (data) => {
            this.refreshData();
            alert("Done");
          },
          error: (error) => {
            let errorMessage = error.message || 'rejectNewProducts error occurred!';
            alert(errorMessage);
          }
        });
      })
    }
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

  getMargin(item: any) {
    return ((item.RetailPrice - item.UnitCost) / item.UnitCost);
  }


  test() {
    console.log(this.selectedRows)
  }

}
