import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Permission } from 'src/assets/model/Permission';
import { User } from 'src/assets/model/User';
import { SharedService } from '../services/shared.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-home',
    templateUrl: 'home.page.html',
    styleUrls: ['home.page.scss'],
    standalone: false
})
export class HomePage {
  private routeParamsSubscription!: Subscription;

  constructor(private router: Router, private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute) { }

  data: any;

  user: User = new User;
  userString!: any;

  permission: Permission = new Permission;
  listStores = [
    { key: "ALP", value: 39 },
    { key: "OFMM", value: 7 },
    { key: "OFC", value: 3 },
    { key: "1080", value: 80 },
    { key: "3652", value: 52 }
  ];

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.permission = this.sharedService.getPermission();

      this.userString = sessionStorage.getItem('user');
      if (this.userString) {
        const user = JSON.parse(this.userString);
        this.user = user;
      }

    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
  }

  onStoreChange(){
    const user = JSON.parse(this.userString);
    user.Store = this.user.Store;
    sessionStorage.setItem('user', JSON.stringify(user));
  }


  toItem() {
    this.router.navigate(['/item'])
  }

  toItemInsert() {
    this.router.navigate(['/item/item-insert'])
  }

  toItemApproval() {
    this.router.navigate(['/item/item-approval'])
  }

  toItemDraft() {
    this.router.navigate(['/item/item-draft'])
  }

  toSupplier() {
    this.router.navigate(['/supplier'])
  }

  toSupplierInsert() {
    this.router.navigate(['/supplier/supplier-insert'])
  }

  toSupplierApproval() {
    this.router.navigate(['/supplier/supplier-approval'])
  }

  toPO() {
    this.router.navigate(['/po'])
  }

  toPOInsert(){
    this.router.navigate(['/po/po-insert'])
  }

  toPOFrequency(){
    this.router.navigate(['/po/po-draft'])
  }

  toggleSplitPane() {

  }

  toOnline() {
    this.router.navigate(['/online'])
  }

  toWarehouse() {
    this.router.navigate(['/warehouse'])
  }

  toWarehouseByPO(){
    this.router.navigate(['/warehouse/warehouse-draft'])
  }

  toWarehouseOneItem(){
    this.router.navigate(['/warehouse/warehouse-in'])
  }

  toGroupInsert() {
    this.router.navigate(['/group/group-insert'])
  }

  toGroup() {
    this.router.navigate(['/group'])
  }

  toGroupDetails() {
    this.router.navigate(['/group/group-details'])
  }

  toInventory() {
    this.router.navigate(['/inventory'])
  }

  toInventoryInsert() {
    this.router.navigate(['/inventory/inventory-insert'])
  }

  toInventoryReturn() {
    this.router.navigate(['/inventory/inventory-return'])
  }
  
  toInventoryReturnList() {
    this.router.navigate(['/inventory/inventory-return-list'])
  }
  
  toInventoryLocation() {
    this.router.navigate(['/inventory/inventory-location'])
  }

  toItemChangePrice() {
    this.router.navigate(['/item/item-change-price']);
  }

  scan() { }
}
