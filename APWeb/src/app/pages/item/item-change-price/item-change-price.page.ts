import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { ModalController } from '@ionic/angular';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { Permission } from 'src/assets/model/Permission';
import * as XLSX from 'xlsx';

@Component({
    selector: 'app-item-change-price',
    templateUrl: './item-change-price.page.html',
    styleUrls: ['./item-change-price.page.scss'],
    standalone: false
})
export class ItemChangePricePage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(
    private sharedService: SharedService, 
    private dataService: DataService, 
    private router: Router, 
    private route: ActivatedRoute, 
    private modalController: ModalController, 
    private cdr: ChangeDetectorRef
  ) {}

  data: any;
  dtTrigger: Subject<any> = new Subject<any>();
  selectedRows: Set<string> = new Set();
  isAllSelected = false;
  productId: string = '';
  itemId: string = '';
  newPrice: number = 0;
  searchResult: any = null;
  StoreID: number = 0;
  records: any[] = [];
  displayedColumns: string[] = ['productId', 'costType', 'draftDate', 'unitCostOld', 'unitCostNew', 'actions'];
  permission: Permission = new Permission;

  dtOptions: any = {};

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.dtOptions = {
      pagingType: 'full_numbers',
      paging: false,
      destroy: true
    };
    this.permission = this.sharedService.getPermission();

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
  }

  private clearSelections() {
    this.selectedRows.clear();
    this.data = [];
    this.isAllSelected = false;
    $('#tableID').DataTable().clear().draw();
    $('#tableID').DataTable().destroy();
  }

  searchPrice() {
    if (!this.productId) {
      alert('Please enter Product ID.');
      return;
    }

    this.dataService.getPrice(this.productId).then(data => {
      if (data) {
        this.searchResult = String(data);
        this.newPrice = data;
      } else {
        alert('Item not found.');
      }
    });
  }

  approvePriceChange(record: any) {
    this.confirmAction(() => {
      this.dataService.approveCostChange(record.ProductId, record.ChangeId, 2, record.UnitCostNew).subscribe({
        next: () => {
          this.refreshData();
          alert('Approved.');
          setTimeout(() => {
            this.getPriceHistory();
          }, 500);
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while Approving!';
          alert(errorMessage);
        }
      });
    });
  }

  rejectPriceChange(record: any) {
    this.confirmAction(() => {
      this.dataService.rejectCostChange(record.ProductId, record.ChangeId, 2).subscribe({
        next: () => {
          this.refreshData();
          alert('Approved.');
          setTimeout(() => {
            this.getPriceHistory();
          }, 500);
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while Approving!';
          alert(errorMessage);
        }
      });
    });
  }

  requestChange() {
    if (!this.newPrice) {
      alert('Please enter a valid new price.');
      return;
    }

    this.confirmAction(() => {
      this.dataService.draftChangeRequest(this.productId, "", this.newPrice).subscribe({
        next: () => {
          this.refreshData();
          alert('Request Sent.');

        this.productId = "";
        this.searchResult = null;
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while requesting!';
          alert(errorMessage);
        }
      });
    });
  }
  
  getDraft() {
    this.dataService.getDraft(1).then(data => {
      if (data && data.length > 0) {
        this.records = data;
      } else {
        this.records = []; 
      }
    }).catch(error => {
      console.error('Error fetching draft records:', error);
      alert('Failed to load records');
    });
  }

  getPriceHistory(){
    this.dataService.getPriceHistory(this.productId).then(data => {
      if (data && data.length > 0) {
        this.records = data;
      } else {
        this.records = []; 
      }
    }).catch(error => {
      console.error('Error fetching draft records:', error);
      alert('Failed to load records');
    });
  }
  
  submitRecord(record: any) {
    this.confirmAction(() => {
      this.dataService.submitCostChange(record.ProductId, record.ChangeId, record.DraftUserId).subscribe({
        next: () => {
          this.refreshData();
          alert('Submitted.');
          setTimeout(() => {
            this.getDraft();
          }, 500);
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while submitting!';
          alert(errorMessage);
        }
      });
    });
  }

  deletRecord(record: any) {
    this.confirmAction(() => {
      this.dataService.deletCostChange(record.ProductId, record.ChangeId).subscribe({
        next: () => {
          this.refreshData();
          alert('Deleted.');
          setTimeout(() => {
            this.getDraft();
          }, 500);
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while submitting!';
          alert(errorMessage);
        }
      });
    });
  }

  editRecord(record: any){
    record.isEditing = true;
  }

  saveRecord(record: any) {
    if (!record.UnitCostNew || record.UnitCostNew.toString().trim() === '') {
      alert('Unit Cost (New) cannot be empty.');
      record.isEditing = false;
      return;
    }
    this.confirmAction(() => {
      this.dataService.editCostChange(record.ProductId, record.ChangeId, record.UnitCostNew).subscribe({
        next: () => {
          this.refreshData();
          alert('Edited.');
          setTimeout(() => {
            this.getDraft();
          }, 500);
        },
        error: (error) => {
          let errorMessage = error.message || 'Error occurred while submitting!';
          alert(errorMessage);
        }
      });
    });
    record.isEditing = false;
  }

  updateUnitCostNew(record: any, event: Event) {
    const inputElement = event.target as HTMLInputElement;
    record.UnitCostNew = inputElement.value;
  }


onFileChange(event: any) {
  const target: DataTransfer = <DataTransfer>event.target;
  
  if (target.files.length !== 1) {
    alert('Please upload a single Excel file.');
    return;
  }

  const reader: FileReader = new FileReader();

  reader.onload = (e: any) => {
    const binaryStr: string = e.target.result;
    const workbook: XLSX.WorkBook = XLSX.read(binaryStr, { type: 'binary' });
    const sheetName: string = workbook.SheetNames[0];
    const worksheet: XLSX.WorkSheet = workbook.Sheets[sheetName];

    // Convert Excel data to JSON
    const excelData: any[] = XLSX.utils.sheet_to_json(worksheet, { raw: true });

    // Now you have an array of objects from the Excel file
    console.log(excelData); 
  };

}

  selectAll() {
    if (this.isAllSelected || this.selectedRows.size === this.data.length) {
      this.selectedRows.clear();
      this.isAllSelected = false;
    } else {
      this.data.forEach((item: any) => this.selectedRows.add(item.ItemID));
      this.isAllSelected = true;
    }
  }

  toggleSelection(ItemID: string) {
    if (this.selectedRows.has(ItemID)) {
      this.selectedRows.delete(ItemID);
    } else {
      this.selectedRows.add(ItemID);
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
}
