import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { User } from 'src/assets/model/User';


@Component({
    selector: 'app-inventory-return-list',
    templateUrl: './inventory-return-list.page.html',
    styleUrls: ['./inventory-return-list.page.scss'],
    standalone: false
})
export class InventoryReturnListPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  constructor(private router: Router, private dataService: DataService, private modalController: ModalController, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) { }

  user: User = new User;
  SupplierName!: string;
  SupplierID!: number;
  ReturnList: any = [];
  SupplierList: any = [];
  filteredSupplierList: any = [];

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

  private refreshData() {
    this.reset();
    this.loadUser();
    this.getProductInspectionResultSuppliers();
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
  }

  reset() {
    this.ReturnList = [];
    this.SupplierList = [];
    this.SupplierID = 0;
    this.SupplierName = "";
  }

  onSelectionChange() {
    this.ReturnList = [];
    this.getProductInspectionResult(this.SupplierID);
  }


  getProductInspectionResultSuppliers() {
    this.dataService.getProductInspectionResultSuppliers(this.user.Store).then(data => {
      if (data.length > 0) {
        this.SupplierList = data;
      }
    });
  }

  getProductInspectionResult(SupplierID: number) {
    this.ReturnList = [];
    this.dataService.getProductInspectionResult(this.user.Store, SupplierID).then(data => {
      if (data.length > 0) {
        this.ReturnList = data;
      }
    });
  }

  updateProductInspectionStatus(item: any) {
    this.dataService.updateProductInspectionStatus(item).subscribe({
      next: (data) => {
        this.getProductInspectionResult(this.SupplierID);
        this.snackBar.open('Submit Successful', 'Close', {
          duration: 3000, 
        });
      },
      error: (error) => {
        let errorMessage = error.message || 'updateProductInspectionStatus error occurred!';
        alert(errorMessage);
      }
    });
  }


  submitReturnItem(item: any) {
    this.dataService.submitReturnItem(item).subscribe({
      next: (data) => {
        this.updateProductInspectionStatus(item);
      },
      error: (error) => {
        let errorMessage = error.message || 'submitReturnItem error occurred!';
        alert(errorMessage);
      }
    });
  }

  // addToStorageReturn(item: any) {
  //   console.log(item);
  // }








}
