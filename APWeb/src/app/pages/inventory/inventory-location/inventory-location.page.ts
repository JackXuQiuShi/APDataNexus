import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { ModalController } from '@ionic/angular';
import { User } from 'src/assets/model/User';


@Component({
    selector: 'app-inventory-location',
    templateUrl: './inventory-location.page.html',
    styleUrls: ['./inventory-location.page.scss'],
    standalone: false
})
export class InventoryLocationPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private router: Router, private dataService: DataService, private modalController: ModalController, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) { }

  user: User = new User;
  Location: string = "";
  ProductID: string = "";
  itemList: any = [];
  supplierList: any = [];
  data: any;
  doubleRed: boolean = false;
  SupplierID!: number;
  StoreID!: number;

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
    this.loadUser();
  }

  private loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      this.user = JSON.parse(userString);
      this.StoreID = this.user.Store;
    }
  }



  setColor(StatusID: number) {
    this.doubleRed = false;
    if (StatusID == -1) {
      this.doubleRed = true;
      return 'red'
    } else if (StatusID == 1) {
      return "red";
    } else if (StatusID == 2) {
      return "yellow";
    } else if (StatusID == 3) {
      return "blue";
    }
    return 'defaultColor';
  }


  reset() {
    this.Location = "";
    this.ProductID = "";
    this.itemList = [];
  }


  getReturnSuppliers() {
    this.supplierList = [];
    this.dataService.getReturnSuppliers(this.StoreID).then(data => {
      if (data.length > 0) {
        this.supplierList = data;
      }
    });
  }

  getInventoryByLocation() {
    this.itemList = [];
    this.dataService.getInventoryByLocation(this.Location.trim()).then(data => {
      if (data.length > 0) {
        this.itemList = data;
      }
    });
  }

  getProductLocation() {
    this.itemList = [];
    this.dataService.getProductLocation(this.ProductID.trim()).then(data => {
      if (data.length > 0) {
        this.itemList = data;
      }
    });
  }

  getNormalizedID() {
    if (this.ProductID != "" && this.ProductID != undefined && !isNaN(Number(this.ProductID))) {
      this.dataService.getNormalizedID(this.ProductID).then(data => {
        if (data.normalizedID.length > 0) {
          this.ProductID = data.normalizedID;
          this.getProductLocation();
        }
      });
    }
  }

  getItemList() {

  }






}
