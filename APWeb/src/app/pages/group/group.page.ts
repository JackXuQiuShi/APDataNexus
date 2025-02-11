import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { ProduceGroup } from 'src/assets/model/ProduceGroup';

@Component({
    selector: 'app-group',
    templateUrl: './group.page.html',
    styleUrls: ['./group.page.scss'],
    standalone: false
})
export class GroupPage implements OnInit {

  constructor(private sharedService: SharedService, private router: Router) { }

  ngOnInit() {
    this.getProductGroup();
  }

  data: any;
  name: string = "";

  getProductGroup(){
    this.sharedService.getProductGroup(this.name).then(data =>
      this.data = data
    );
  }

  editGroup(group: ProduceGroup){
    this.sharedService.setMyData(group);
    this.router.navigate(['/group/group-insert'], { queryParams: { url: 'Edit' } });
    
  }

  mapGroup(group: ProduceGroup){
    //this.sharedService.setMyData(group);
    this.router.navigate(['/group/group-details'], { queryParams: { id:group.ProduceGroupID, name: group.ProduceGroupName } });
  }
}
