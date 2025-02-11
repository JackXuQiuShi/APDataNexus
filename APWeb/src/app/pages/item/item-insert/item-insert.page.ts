import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Item } from 'src/assets/model/Item';
import { DataService } from 'src/app/services/data.service';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { Subscription } from 'rxjs';
import { Department, Departments } from 'src/assets/model/Department';
import { HttpClient } from '@angular/common/http';
import { Converter } from 'opencc-js';
import { Buyer } from 'src/assets/model/Buyer';
import { User } from 'src/assets/model/User';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-item-insert',
    templateUrl: './item-insert.page.html',
    styleUrls: ['./item-insert.page.scss'],
    standalone: false
})
export class ItemInsertPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  private sharedServiceSubscription!: Subscription;

  user: User = new User;
  data: Item = new Item;
  SupplierName!: string;
  SupplierID!: number;
  isSupplierFound: boolean = false;
  Departments: Department[] = Departments;
  listBuyer: Buyer[] = [];
  edit: boolean = false;

  Ethnics = ['Canadian', 'Caribbean', 'Chinese', 'European', 'Japanese & Korean', 'Philippines', 'South Asian', 'Vietnam & Thailand', 'Middle Eastern']
  Measures = ['EA', 'LB']
  UOM = ['G', 'KG', 'LB', 'ML', 'L', 'CT', 'INCH', 'CM', 'OZ', 'PIECES']

  detail_productID = 'Barcode,条码'
  detail_fullname = '产品全名:有批发商提供或者用品牌+品名+口味+规格,Length must be greater than 10,长度至少为10'
  detail_productname = 'Length must be greater than 10, and less than or equal 25.长度至少为10,最多25'
  detail_chinesename = 'Maximum 15. 繁体中文'
  detail_unitsize = 'Number only数字'
  detail_unitsizeuom = '单位'
  detail_packagespec = 'Indicate product weight产品重量信息'
  detail_measure = 'Is the product sold individually (EA) or by weight(LB)?产品售卖方式按个还是按磅'
  detail_numPerPack = 'How many units are in a case?一箱有多少个产品？'
  detail_volumecost = 'Is there any promotion price if order more quantity? 是否有买十送一等的优惠价'
  detail_minvolume = 'The minimum quantity order to get the promotion price. 订多少量可以有特价'
  detail_countryoforigin = 'Enter country of origin on the product. 填写产品包装上的产地'

  constructor(private sharedService: SharedService, private dataService: DataService, private route: ActivatedRoute, private router: Router, private snackBar: MatSnackBar) {
  }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {

      if (params['url'] === "Edit") {
        this.edit = true;
        this.editMode();
      } else {
        this.edit = false;
        this.addMode();
        this.subscribeToSharedService();
      };
      this.getBuyer();
    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
    this.sharedServiceSubscription?.unsubscribe();
  }

  private subscribeToSharedService() {
    this.sharedServiceSubscription = this.sharedService.observableData.subscribe((newData: any) => {
      this.handleDataChange(newData);
    });
  }

  private editMode() {
    this.isSupplierFound = true;
    this.data = this.sharedService.getMyData();
    //this.SupplierName = this.data.CompanyName;
    if (!this.data || undefined) {
      this.router.navigate(['/item/item-draft']);
    }
  }

  private addMode() {
    this.isSupplierFound = false;
    this.data = new Item(); // Initialize a new Item
    this.loadUser();
  }

  private loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      this.user = JSON.parse(userString);
      this.data.Applicant = this.user.Name;
      this.data.RequestStoreID = this.user.Store;
    }
  }

  private handleDataChange(newData: any) {
    this.data.SupplierID = newData.SupplierID;
    //this.SupplierID = newData.SupplierID;
    this.isSupplierFound = !!newData.SupplierID;
    this.data.CompanyName = newData.CompanyName;
  }

  checkTax1(event: MatCheckboxChange) {
    this.data.Tax1App = event.checked ? 1 : 0;
  }
  checkTax2(event: MatCheckboxChange) {
    this.data.Tax2App = event.checked ? 1 : 0;
  }

  changeSupplier() {
    this.isSupplierFound = false;
  }

  onProductIDChange() {
    const desiredLength = 13;

    if (this.data.ProductID.length > desiredLength) {
      alert("Length > 13. Make sure it's correct.");
    }

    if (this.data.ProductID.length < desiredLength && this.data.ProductID.length > 0) {
      const zerosToAdd = desiredLength - this.data.ProductID.length;
      const leadingZeros = '0'.repeat(zerosToAdd);
      this.data.ProductID = leadingZeros + this.data.ProductID;
    }

    this.isItemExist(this.data.ProductID, this.data.RequestStoreID);
  }

  updatePackageSpec() {
    this.data.PackageSpec = `${this.data.UnitSize} ${this.data.UnitSizeUom}`;
  }

  convertToTraditional() {
    const converter = Converter({ from: 'cn', to: 'tw' });
    this.data.ProductAlias = converter(this.data.ProductAlias)
  }

  getMargin(item: any) {
    return ((item.RetailPrice - item.UnitCost) / item.UnitCost);
  }



  getBuyer() {
    this.dataService.getItemBuyer(this.data.RequestStoreID).then(data => {
      if (data.length > 0) {
        this.listBuyer = data;
      }
    });
  }

  isItemExist(ProductID: string, RequestStoreID: number) {
    this.dataService.isItemExist(ProductID, RequestStoreID).then(data => {
      if (data) {
        alert(this.data.ProductID + " already exist.");
        this.data.ProductID = "";
      }
    });
  }

  itemInsert() {
    this.dataService.insertItemRequest(this.data).subscribe({
      next: (data) => {
        this.snackBar.open('Insert Successful.', 'Close', {
          duration: 3000,
        });
        this.data = new Item;
        this.data.Applicant = this.user.Name;
        this.data.RequestStoreID = this.user.Store;
        this.data.SupplierID = this.SupplierID;
      },
      error: (error) => {
        let errorMessage = error.message || 'insertItemRequest error occurred!';
        alert(errorMessage);
      }
    });
  }

  itemInsertAndSaveInfo() {
    this.dataService.insertItemRequest(this.data).subscribe({
      next: (data) => {
        this.snackBar.open('Insert Successful.', 'Close', {
          duration: 3000,
        });
      },
      error: (error) => {
        let errorMessage = error.message || 'insertItemRequest error occurred!';
        alert(errorMessage);
      }
    });
  }

  updateItemDraft() {
    this.dataService.updateItemDraft(this.data).subscribe({
      next: (data) => {
        if (data) {
          this.snackBar.open('Update Successful.', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/item/item-draft']);
        }
      },
      error: (error) => {
        let errorMessage = error.message || 'updateItemDraft error occurred!';
        alert(errorMessage);
      }
    });
  }








}
