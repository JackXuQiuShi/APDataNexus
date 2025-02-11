import { Component, OnInit } from '@angular/core';
import { ProduceGroup } from 'src/assets/model/ProduceGroup';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
    selector: 'app-group-insert',
    templateUrl: './group-insert.page.html',
    styleUrls: ['./group-insert.page.scss'],
    standalone: false
})
export class GroupInsertPage implements OnInit {

  constructor(private dataService: DataService, private sharedService: SharedService, private route: ActivatedRoute, private router: Router) {
    route.params.subscribe(val => {
      this.refreshData();
    });
  }

  ngOnInit() {
    this.refreshData();
  }

  refreshData() {
    this.route.queryParams.subscribe(params => {
      if (params['url'] == "Edit" && this.sharedService.getMyData() != undefined) {
        this.data = this.sharedService.getMyData();
        this.isInsertHidden = true;
      }
      else {
        this.data = new ProduceGroup;
        this.isInsertHidden = false;
      }
    });
  }

  isInsertHidden: boolean = true;
  data: any;

  groupInsert() {
    this.dataService.insertGroup(this.data).subscribe({
      next: (data) => {
        alert(data.message);
        this.data = new ProduceGroup;
      },
      error: (error) => {
        let errorMessage = error.error.message || 'insertGroup unknown error occurred!';
        alert(errorMessage);
      }
    });
  }


  groupUpdate() {
    this.dataService.updateGroup(this.data).subscribe({
      next: (data) => {
        alert(data.message);
        this.router.navigate(['/group']);
      },
      error: (error) => {
        let errorMessage = error.error.message || 'updateGroup error occurred!';
        alert(errorMessage);
      }
    });
  }



}
