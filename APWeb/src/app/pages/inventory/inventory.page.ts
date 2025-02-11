import { Component, ElementRef, HostListener, NgZone, OnInit, Renderer2, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSelectChange } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Inventory } from 'src/assets/model/Inventory';
import { InventoryReturn } from 'src/assets/model/InventoryReturn';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { ModalController } from '@ionic/angular';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { User } from 'src/assets/model/User';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
    selector: 'app-inventory',
    templateUrl: './inventory.page.html',
    styleUrls: ['./inventory.page.scss'],
    standalone: false
})
export class InventoryPage implements OnInit {
  private routeParamsSubscription!: Subscription;

  itemList: any = [];
  listReturnID: number[] = [];
  supplierList: any = [];
  CreditList: any = [];
  Location: string = "";
  user: User = new User;
  returnData: InventoryReturn = new InventoryReturn;
  pickupDate: Date = new Date;
  today: Date = new Date;
  totalAmount: number = 0;
  totalSub: number = 0;
  totalTax: number = 0;
  CreditNumber: string = "";
  printArea: boolean = false;
  rePrint: boolean = false;
  needInfo: boolean = false;

  constructor(private router: Router, private dataService: DataService, private modalController: ModalController, private route: ActivatedRoute, private sharedService: SharedService, private snackBar: MatSnackBar) { }

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
    this.getReturnSuppliers();
    this.setPickupDate(7);
  }

  private loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      this.user = JSON.parse(userString);
      this.returnData.StoreID = this.user.Store;
      this.setLocation(this.returnData.StoreID)
    }
  }

  setLocation(StoreID: number) {
    if (StoreID == 39) {
      this.Location = "1970 Eglinton Avenue East in Scarborough";
    } else if (StoreID == 7) {
      this.Location = "250 Alton Towers Circle in Scarborough";
    } else if (StoreID == 3) {
      this.Location = "888 Dundas St E in Mississauga";
    } else if (StoreID == 80) {
      this.Location = "1080";
    } else if (StoreID == 52) {
      this.Location = "3652";
    } else if (StoreID == 17) {
      this.Location = "1117";
    } else if (StoreID == 35) {
      this.Location = "3135";
    }
  }


  setPickupDate(DateNumber: number) {
    const currentDate = new Date();
    currentDate.setDate(currentDate.getDate() + DateNumber);
    this.pickupDate = currentDate;
  }
  // getInventory() {
  //   this.itemList = [];
  //   this.dataService.getInventory(this.Location).subscribe(data => {
  //     this.itemList = data;
  //   })
  // }
  reset() {
    this.listReturnID = [];
    this.itemList = [];
    this.Location = "";
    this.supplierList = [];
    this.returnData = new InventoryReturn;
    this.pickupDate = new Date;
    this.setPickupDate(7);
    this.today = new Date;
    this.totalAmount = 0;
    this.totalSub = 0;
    this.totalTax = 0;
    this.printArea = false;
    this.rePrint = false;
    this.needInfo = false;
    this.loadUser();
  }

  onTabChange(event: MatTabChangeEvent) {
    switch (event.index) {
      case 0:
        this.reset();
        this.getReturnSuppliers();
        break;
      case 1:
        this.reset();
        this.getCreditList();
        break;
      default:
        console.log('Unknown tab');
    }
  }

  navigateToReturn(inventoryReturn: any) {
    if (!this.rePrint) {
      this.sharedService.setMyData(inventoryReturn)
      this.router.navigate(['/inventory/inventory-return'])
    }
  }




  countAll() {
    this.totalAmount = 0;
    this.totalSub = 0;
    this.totalTax = 0;
    this.needInfo = false;
    this.itemList.forEach((element: InventoryReturn) => {
      this.totalSub += this.countSubTotal(element);
      this.totalTax += this.countTax(element);
      if (element.UnitCost == null || element.ProductName == null || element.ReturnQuantity == null) {
        this.needInfo = true;
      }
    });
    this.totalAmount = this.totalSub + this.totalTax;
  }

  countSubTotal(inventoryReturn: InventoryReturn) {
    if (inventoryReturn.UnitCost && inventoryReturn.ReturnQuantity) {
      return inventoryReturn.UnitCost * inventoryReturn.ReturnQuantity;
    }
    return 0;
  }

  countTax(inventoryReturn: InventoryReturn) {
    if (inventoryReturn.Tax && inventoryReturn.Tax == 1) {
      return this.countSubTotal(inventoryReturn) * 0.13;
    }
    return 0;
  }

  countTotalAmount(inventoryReturn: InventoryReturn) {
    return this.countSubTotal(inventoryReturn) + this.countTax(inventoryReturn)
  }


  printTable() {
    const printContent = document.getElementById('print-section');
    // this.printTableToPDF();
    const WindowPrt = window.open('', '', 'width=900,height=650');
    if (WindowPrt && printContent) {
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
      if (!this.rePrint) {
        this.generateCreditNote();
      }
    } else {
      alert('Failed to open print window. Please check your browser settings.');
    }
  }

  getReturnSuppliers() {
    this.supplierList = [];
    this.dataService.getReturnSuppliers(this.returnData.StoreID).then(data => {
      if (data.length > 0) {
        this.supplierList = data;
      }
    });
  }

  getCreditList() {
    this.dataService.getCreditList(this.returnData.StoreID).then(data => {
      if (data.length > 0) {
        this.CreditList = data;
      }
    });
  }

  getCreditByCreditNumber(CreditNumber: string) {
    this.dataService.getCreditByCreditNumber(CreditNumber).then(data => {
      if (data.length > 0) {
        this.printArea = true;
        this.rePrint = true;
        this.itemList = data;
        this.countAll();
      }
    });
  }

  getReturnData() {
    if (this.returnData.SupplierID) {
      this.dataService.getReturnData(this.returnData.SupplierID, this.returnData.StoreID).then(data => {
        if (Array.isArray(data) && data.length > 0) {
          this.printArea = true;
          this.itemList = data;
          this.countAll();
          this.listReturnID = data.map(item => item.ReturnID);
        }
      });
    }
  }

  generateCreditNote() {
    this.confirmAction(() => {
      this.dataService.generateCreditNote(this.listReturnID, this.returnData.StoreID).subscribe({
        next: (data) => {
          this.reset();
        },
        error: (error) => {
          let errorMessage = error.message || 'generateCreditNote error occurred!';
          alert(errorMessage);
        }
      });
    })

  }


  async confirmAction(action: () => void) {
    const modal = await this.modalController.create({
      component: ConfirmationComponent,
      componentProps: {
        message: 'Did the print succeed?'
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
