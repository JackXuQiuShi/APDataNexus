import { Component, ElementRef, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SearchSupplierComponent } from 'src/app/components/search-supplier/search-supplier.component';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { User } from 'src/assets/model/User';


@Component({
  selector: 'app-po',
  templateUrl: './po.page.html',
  styleUrls: ['./po.page.scss'],
  standalone: false
})
export class PoPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private route: ActivatedRoute, private sharedService: SharedService, private router: Router, private dataService: DataService, public dialog: MatDialog, private elRef: ElementRef) { }

  orderListData!: any;
  SupplierName!: string;
  SupplierID!: number;
  SupplierList: any = [];
  user: User = new User;
  //StatusList: string[] = ["Ordering", "Ordered", "Receiving", "Received"];
  StatusList = [
    { key: "Draft", value: 1 },
    { key: "Submited", value: 2 },
    { key: "Completed", value: 3 }
  ];
  StatusID: number = 1;


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

  ionViewDidLeave() {
    this.reset();
  }

  refreshData() {
    this.loadUser();
    this.getOrdersSuppliersByStatusID(this.StatusID, this.user.Store);
    if (this.SupplierID) {
      this.getOrdersByStatusIDAndStoreID(this.StatusID, this.user.Store, this.SupplierID);
    }
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
  }

  reset() {
    this.orderListData = [];
  }

  toCreatePO() {
    this.router.navigate(['/po/create-po']);
  }


  toOrderDetails(order: any) {
    this.router.navigate(['/po/po-details'], { queryParams: { OrderID: order.OrderID } });
  }

  onStatusChange() {
    this.SupplierID = 0;
    this.orderListData = [];
    this.SupplierList = [];
    this.getOrdersByStatusIDAndStoreID(this.StatusID, this.user.Store, this.SupplierID);
    this.getOrdersSuppliersByStatusID(this.StatusID, this.user.Store);
  }

  onSupplierChange() {
    this.orderListData = [];
    this.getOrdersByStatusIDAndStoreID(this.StatusID, this.user.Store, this.SupplierID);
  }


  getOrdersSuppliersByStatusID(StatusID: number, StoreID: number) {
    this.dataService.getOrdersSuppliersByStatusID(StatusID, StoreID).then(data => {
      if (data.length > 0) {
        this.SupplierList = data;
        this.onSupplierChange();
      }
    });
  }

  getOrdersByStatusIDAndStoreID(StatusID: number, StoreID: number, SupplierID: number) {
    this.dataService.getOrdersByStatusIDAndStoreID(StatusID, StoreID, SupplierID).then(data => {
      if (data.length > 0) {
        this.orderListData = data;
      }
    });
  }

  

  

}
