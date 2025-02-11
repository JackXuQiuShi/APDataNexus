import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { AlertController } from '@ionic/angular';
import { DataService } from 'src/app/services/data.service';
import { Supplier } from 'src/assets/model/Supplier';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-supplier-approval',
    templateUrl: './supplier-approval.page.html',
    styleUrls: ['./supplier-approval.page.scss'],
    standalone: false
})
export class SupplierApprovalPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  
  constructor(private sharedService: SharedService, private dataService: DataService, private alertController: AlertController) { }

  ngOnInit() {
    this.getSupplierRequest()
  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

  CompanyName: string = "";
  data: any;

  getSupplierRequest() {
    this.sharedService.getSupplierRequest(this.CompanyName).then(data =>
      this.data = data
    );
  }

  // Assuming dataService.updateSupplierRequest returns an Observable
  updateSupplierRequest(CompanyName: string, Status: number) {
    return new Promise((resolve, reject) => {
      this.dataService.updateSupplierRequest(CompanyName, Status).subscribe({
        next: (data) => {
          alert(data.message);
          resolve(data); // Resolve the promise on success
        },
        error: (error) => {
          let errorMessage = error.error.message || 'updateSupplierRequest error occurred!';
          alert(errorMessage);
          reject(error); // Reject the promise on error
        }
      });
    });
  }

  async selectSupplier(supplier: any) {
    const alert = await this.alertController.create({
      header: 'Supplier',
      message: `Approve/Deny ${supplier.CompanyName}?`,
      buttons: [
        {
          text: 'Approve',
          handler: async () => { // Mark this handler as async
            await this.updateSupplierRequest(supplier.CompanyName, 1);
            this.getSupplierRequest();
          }
        },
        {
          text: 'Deny',
          handler: async () => { // Use async here directly
            await this.updateSupplierRequest(supplier.CompanyName, 0);
            this.getSupplierRequest();
          }
        }
      ]
    });
    await alert.present();
  }







}
