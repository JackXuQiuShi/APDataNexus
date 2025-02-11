import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertController } from '@ionic/angular';
import { Subscription } from 'rxjs';
import { SignPOComponent } from 'src/app/components/sign-po/sign-po.component';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { PO } from 'src/assets/model/PO';
import { PODetails } from 'src/assets/model/PODetails';

@Component({
    selector: 'app-po-receive',
    templateUrl: './po-receive.page.html',
    styleUrls: ['./po-receive.page.scss'],
    standalone: false
})
export class PoReceivePage implements OnInit {

  constructor(){}
  ngOnInit(){}
  // private routeParamsSubscription!: Subscription;

  // constructor(private route: ActivatedRoute,
  //   private sharedService: SharedService,
  //   private dataService: DataService,
  //   private router: Router,
  //   private alertController: AlertController) {
  //   this.refreshData();
  // }

  // ngOnInit() {
  //   this.refreshData();
  // }

  // ngOnDestroy(): void {
  //   if (this.routeParamsSubscription) {
  //     this.routeParamsSubscription.unsubscribe();
  //   }
  // }

  // POdata: PO = new PO;
  // PODetailsData: any;

  // isDetailsAllSelected = false;
  // selectedDetailRows: Set<string> = new Set();
  // PODetails: any[] = [];

  // async refreshData() {
  //   this.selectedDetailRows.clear();
  //   this.routeParamsSubscription =
  //     this.route.queryParams.subscribe(async params => {
  //       const data = this.sharedService.getPOData();
  //       if (data) {
     
  //         this.POdata = data;
  //         await this.getPODetails(this.POdata.PO_ID)
  //       }
  //     });

  // }

  // async getPODetails(PO_ID: string) {
  //   this.PODetailsData = await this.sharedService.getPODetails(PO_ID);
  //   console.log(this.PODetailsData)
  // }

  // updateReceivingPODetails(detail: any) {
  //   this.sharedService.setMyData(detail);
  //   this.router.navigate(['/po/po-details'], { queryParams: { url: 'Edit', state: 'Receiving', SupplierID: this.POdata.SupplierID } });
  // }


  // selectDetailsAll() {
  //   if (this.isDetailsAllSelected || this.selectedDetailRows.size === this.PODetailsData.length) {
  //     // 如果已经全选或所有项都已选中，则清空selectedStoreRows实现全不选
  //     this.selectedDetailRows.clear();
  //     this.isDetailsAllSelected = false; // 重置全选标志
  //   } else {
  //     // 否则，添加所有项到selectedStoreRows中实现全选
  //     this.PODetailsData.forEach((details: PODetails) => this.selectedDetailRows.add(details.ProductID));
  //     this.isDetailsAllSelected = true; // 设置全选标志
  //   }
  // }

  // toggleDetailsSelection(ProductID: string) {
  //   if (this.selectedDetailRows.has(ProductID)) {
  //     this.selectedDetailRows.delete(ProductID);
  //   } else {
  //     this.selectedDetailRows.add(ProductID);
  //   }
  // }

  // updatePODetailsReceivedStatus(PODetails: any) {
  //   this.dataService.updatePODetailsReceivedStatus(PODetails).subscribe({
  //     next: (data) => {
  //       this.PODetails = [];
  //       this.selectedDetailRows.clear();
  //     },
  //     error: (error) => {
  //       let errorMessage = error.message || 'updatePODetailsReceivedStatus error occurred!';
  //       alert(errorMessage);
  //     }
  //   });
  // }

  // updatePOToReceived(PO: any) {
  //   this.dataService.updatePOToReceived(PO).subscribe({
  //     next: (data) => {
  //       alert("Receved Successful.");
  //     },
  //     error: (error) => {
  //       let errorMessage = error.message || 'updatePOToReceived error occurred!';
  //       alert(errorMessage);
  //     }
  //   });
  // }

  // POReceive() {
  //   this.PODetails = [];
  //   if (this.selectedDetailRows.size != 0) {
  //     this.confirm();
  //   }
  // }

  
  // async confirm() {
  //   const alert = await this.alertController.create({
  //     header: 'PO Receive',
  //     message: `Confirm/Cancel`,
  //     buttons: [
  //       {
  //         text: 'Confirm',
  //         handler: async () => { // Mark this handler as async
  //           this.selectedDetailRows.forEach((ProductID) =>
  //             this.PODetails.push({ PO_ID: this.POdata.PO_ID, ProductID: ProductID, OrderedBy: this.POdata.Ordered_By })
  //           )
  //           this.updatePODetailsReceivedStatus(this.PODetails);
  //           this.updatePOToReceived(this.POdata);
  //           this.router.navigate(['/po']);
  //         }
  //       },
  //       {
  //         text: 'Cancel',
  //         handler: async () => { // Use async here directly
  //         }
  //       }
  //     ]
  //   });
  //   await alert.present();
  // }


}
