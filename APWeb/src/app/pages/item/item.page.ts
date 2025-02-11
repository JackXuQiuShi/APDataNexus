import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';

@Component({
    selector: 'app-item',
    templateUrl: './item.page.html',
    styleUrls: ['./item.page.scss'],
    standalone: false
})
export class ItemPage implements OnInit {

  private routeParamsSubscription!: Subscription;

  constructor(private router: Router, private dataService: DataService, private sharedService: SharedService) { }

  ngOnInit() {

  }

  ngOnDestroy(): void {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

  itemList: any = [];
  input: any = '0000000000002';

  search() {
    this.itemList = [];

    if (this.input.length > 3) {
      if (isNaN(Number(this.input))) {
        this.getItemByProductName(this.input);
      }
      else {
        this.getItemByProductID(this.input);
      }
    }
  }



  getItemByProductID(input: any) {
    this.sharedService.getItemByProductID(input).then(data => {
      this.itemList = data;
    });
  }


  getItemByProductName(input: any) {
    this.dataService.getItemByProductName(input).subscribe({
      next: (data) => {
        this.itemList = data;
      },
      error: (error) => {
        alert(error.message);
      }
    });
  }

  editItem(item: any) {
    // delete item.EffectiveDate;
    // delete item.CreateDate;

    this.sharedService.setMyData(item);
    this.router.navigate(['/item/item-update']);

  }

  buttonUpdateItem(item: any) {
    this.editItem(item);
  }

  buttondeleteItem(item: any) {
    console.log("delete", item);
  }

  test() { }


  // insertData() {30079407
  //   this.data = ({
  //     Item_Id: this.Item_Id,
  //     Item_Name: this.Item_Name
  //   });

  //   this.dataService.insertItem(this.data).subscribe(data => { });
  //   alert("Done");
  // }

  // deleteData() {
  //   this.dataService.deleteItemByID(this.Item_Id).subscribe(data => { });
  //   alert("Done");
  // }

  // updateData(){
  //   this.dataService.updateItemByID(this.Item_Id, this.Item_Name).subscribe(data => { });
  //   alert("Done");
  // }

}
