import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-online',
    templateUrl: './online.page.html',
    styleUrls: ['./online.page.scss'],
    standalone: false
})
export class OnlinePage implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  data: any;
  itemName!:string;




  getitemByName(){}
  createItem(){}
  selectItem(result: any){}







}
