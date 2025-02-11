import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { ModalController } from '@ionic/angular';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-inventory-list',
    templateUrl: './inventory-list.page.html',
    styleUrls: ['./inventory-list.page.scss'],
    standalone: false
})
export class InventoryListPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  constructor(private router: Router, private dataService: DataService, private modalController: ModalController, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) { }

  user: User = new User;
  data: any;
  supplierList: any = [];
  itemList: any = [];
  
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
    }
  }







  getItemList(){
    
  }


  getReturnSuppliers() {
    this.supplierList = [];
    this.dataService.getReturnSuppliers(this.data.StoreID).then(data => {
      if (data.length > 0) {
        this.supplierList = data;
      }
    });
  }






  reset(){

  }









}
