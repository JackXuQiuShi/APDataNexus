import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Department, Departments } from 'src/assets/model/Department';
import { Item } from 'src/assets/model/Item';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { ModalController } from '@ionic/angular';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-item-draft',
    templateUrl: './item-draft.page.html',
    styleUrls: ['./item-draft.page.scss'],
    standalone: false
})
export class ItemDraftPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private sharedService: SharedService, private dataService: DataService, private router: Router, private route: ActivatedRoute, private modalController: ModalController) {
  }

  user: User = new User;
  data: any;
  pendingData: any;
  approvedData: any;
  dtTrigger: Subject<any> = new Subject<any>();
  selectedRows: Set<string> = new Set();
  isAllSelected = false;

  listProductID: string[] = [];
  listStatus = [
    { key: "Draft", value: 0 },
    { key: "Pending", value: 1 },
    { key: "Aproved", value: 2 },
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
  
  Status: number = 0;
  StatusID: number = 0;

  dtOptions: any = {};

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.refreshData();
    });

  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
    this.dtTrigger.unsubscribe();
  }

  private refreshData() {
    this.clearSelections();
    this.loadUser();
    this.getItemDraft();
  }

  private clearSelections() {
    this.selectedRows.clear();
    this.data = [];
    this.isAllSelected = false;
    this.Status = 0;
    this.StatusID = 0;
  }

  private loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      this.user = JSON.parse(userString);
    }
  }

  //get Department Name from Department model
  getDepartmentName(departmentID: number): string {
    const department = Departments.find(dep => dep.DepartmentID === departmentID);
    return department?.DepartmentName || 'Unknown Department';
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

  //if selectedRows has this product, then delete. else add
  toggleSelection(ProductID: string) {
    if (this.selectedRows.has(ProductID)) {
      this.selectedRows.delete(ProductID);
    } else {
      this.selectedRows.add(ProductID);
    }
  }

  //send item to sharedService, and navigate to item-insert with url 'Edit' for update
  selectItem(item: Item) {
    if (this.StatusID == 0 || this.StatusID == -1) {
      this.sharedService.setMyData(item);
      this.router.navigate(['/item/item-insert'], { queryParams: { url: 'Edit' } });
    }
  }

  addSelectRowsToList() {
    this.listProductID = [];
    this.listProductID = Array.from(this.selectedRows);
  }

  getMargin(item: any) {
    return ((item.RetailPrice - item.UnitCost) / item.UnitCost);
  }

  //get draft by using StatusID, and StoreID, map with departments from Department model
  getItemDraft() {
    this.StatusID = this.Status;

    this.dataService.getItemRequest(this.StatusID, this.user.Store).then(data => {
      if (data) {
        this.data = data.map((item: { DepartmentID: number; }) => {
          const departmentName = this.getDepartmentName(item.DepartmentID);
          return { ...item, DepartmentName: departmentName };
        });
      }
    });
  }

  submitItemDraft() {
    this.addSelectRowsToList();

    if (this.listProductID.length != 0) {
      this.confirmAction(() => {
        this.dataService.submitItemDraft(this.listProductID).subscribe({
          next: (data) => {
            this.refreshData();
            alert("Done");
          },
          error: (error) => {
            let errorMessage = error.message || 'submitItemDraft error occurred!';
            alert(errorMessage);
          }
        });
      })
    };
  }

  deleteItemDraft(item: Item) {
    this.confirmAction(() => {
      this.dataService.deleteItemDraft(item.ProductID).subscribe({
        next: data => {
          alert(data.message);
          this.refreshData();
        },
        error: error => {
          let errorMessage = error.message || 'deleteItemDraft error occurred!';
          alert(errorMessage);
        }
      });
    });
  }

  async confirmAction(action: () => void) {
    const modal = await this.modalController.create({
      component: ConfirmationComponent,
      componentProps: {
        message: `请核对价格。利润40%是为压低进价。卖价过高会有严重后果。\nCheck pricing. The 40% profit is to lower costs. High retail prices have serious consequences.`
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







}
