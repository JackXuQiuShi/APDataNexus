import { Component, OnInit } from '@angular/core';
import { ConfirmationComponent } from 'src/app/components/confirmation/confirmation.component';
import { DataService } from 'src/app/services/data.service';
import { ModalController } from '@ionic/angular';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { Subscription } from 'rxjs';
import { Item } from 'src/assets/model/Item';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-item-update',
    templateUrl: './item-update.page.html',
    styleUrls: ['./item-update.page.scss'],
    standalone: false
})
export class ItemUpdatePage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private dataService: DataService, private router: Router, private modalController: ModalController, private sharedService: SharedService) { }

  data: Item = new Item;
  CurrentCost!: number;
  user!: User;

  ngOnInit() {
    this.refreshData();

  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

  private refreshData() {
    this.routeParamsSubscription =
      this.sharedService.observableData.subscribe((newData: any) => {

        const userString = sessionStorage.getItem('user');
        if (userString) {
          this.user = JSON.parse(userString);
          this.data.Applicant = this.user.Name;
          this.data.RequestStoreID = this.user.Store;
          this.data.ProductID = newData.Prod_Num;
          this.data.ProductName = newData.Prod_Name;
        }

        this.dataService.getCurrentCost(this.data.ProductID).subscribe(data =>
          this.CurrentCost = data
        )

      });
  }



  // itemUpdate() {
  //   this.dataService.updateItemByID(this.data).subscribe({
  //     next: (data) => {
  //       console.log('Update successful:', this.data);
  //     },
  //     error: (error) => {
  //       alert(error.message);
  //     }
  //   });
  // }


  async itemUpdate() {
    const modal = await this.modalController.create({
      component: ConfirmationComponent,
      cssClass: 'custom-modal',
    });

    modal.onDidDismiss().then((result) => {
      if (result.data === true) {
        this.dataService.insertItemUpdateRequest(this.data).subscribe({
          next: (data) => {
            alert('Insert successful');
            this.router.navigate(['/item']);
          },
          error: (error) => {
            alert(error.message);
          }
        });
      } else { }
    });
    return await modal.present();
  }


  async itemDelete() {
    const modal = await this.modalController.create({
      component: ConfirmationComponent,
      cssClass: 'custom-modal',
    });

    modal.onDidDismiss().then((result) => {
      if (result.data === true) {

      } else { }
    });
    return await modal.present();
  }





}
