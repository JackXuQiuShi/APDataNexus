import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { Supplier } from 'src/assets/model/Supplier';
import { User } from 'src/assets/model/User';

@Component({
  selector: 'app-create-po',
  templateUrl: './create-po.page.html',
  styleUrls: ['./create-po.page.scss'],
})
export class CreatePOPage implements OnInit {

  private routeParamsSubscription!: Subscription;
  private sharedServiceSubscription!: Subscription;
  supplierData: Supplier = new Supplier;
  user: User = new User;

  constructor(private dataService: DataService, private route: ActivatedRoute, private sharedService: SharedService) { }

  async ngOnInit() {
    await this.dataService.loadConfig();
    this.routeParamsSubscription = this.route.queryParams.subscribe(params => {
      this.refreshData();
    });
  }

  ngOnDestroy(): void {
    this.routeParamsSubscription?.unsubscribe();
    this.sharedServiceSubscription?.unsubscribe();
  }

  private refreshData() {
    this.sharedServiceSubscription = this.sharedService.observableData.subscribe((newData: any) => {
      this.loadUser();
      this.supplierData = newData;
    });
  }

  private loadUser() {
    this.user = this.sharedService.loadUser();
  }

  createNewPO() {
    if (typeof (this.supplierData) != 'string') {
      console.log(this.supplierData)
    }
    else{
      alert("Please select a supplier.")
    }
  }










}
