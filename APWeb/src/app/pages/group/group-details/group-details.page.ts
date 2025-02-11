import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { SharedService } from 'src/app/services/shared.service';
import { GroupDetails } from 'src/assets/model/GroupDetails';
import { AlertController } from '@ionic/angular';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

@Component({
    selector: 'app-group-details',
    templateUrl: './group-details.page.html',
    styleUrls: ['./group-details.page.scss'],
    standalone: false
})
export class GroupDetailsPage implements OnInit {

  constructor(private route: ActivatedRoute, private sharedService: SharedService, private dataService: DataService) { }

  //get groupname and groupid from group page when user select
  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.itemName = params['name'];
      this.groupID = params['id'];
    });
  }

  storeData!: any;
  warehouseData!: any;
  //groupDetailsData!: any;

  groupDetailsStoreData!: any;
  groupDetailsWarehouseData!: any;


  itemName!: string;
  groupID!: number;

  isStoreAllSelected = false;
  isWarehouseAllSelected = false;
  selectedStoreRows: Set<number> = new Set();
  selectedWarehouseRows: Set<number> = new Set();

  groupDetails: GroupDetails[] = [];

  //get item by group name, user can also manually input.
  groupDetail() {
    //get store item
    this.sharedService.getItemNotInDetails(2, this.itemName).then(data =>
      this.storeData = data
    );

    //get warehouse item
    this.sharedService.getItemNotInDetails(1, this.itemName).then(data =>
      this.warehouseData = data
    );

    this.sharedService.getItemFromGroupDetails(this.groupID).then(data => {
      // 使用filter方法筛选OrganizationID等于1的项
      this.groupDetailsStoreData = data.filter((item: { OrganizationID: number; }) => item.OrganizationID === 2);
      this.groupDetailsWarehouseData = data.filter((item: { OrganizationID: number; }) => item.OrganizationID === 1);
    });

  }



  deleteGroupDetails(groupDetails: GroupDetails) {
    this.dataService.deleteGroupDetails(groupDetails).subscribe({
      next: (data) => {
        //alert(data.message);
        this.groupDetail();
      },
      error: (error) => {
        let errorMessage = error.error.message || 'deleteGroupDetails error occurred!';
        alert(errorMessage);
      }
    });
  }


  selectStoreAll() {
    if (this.isStoreAllSelected || this.selectedStoreRows.size === this.storeData.length) {
      // 如果已经全选或所有项都已选中，则清空selectedStoreRows实现全不选
      this.selectedStoreRows.clear();
      this.isStoreAllSelected = false; // 重置全选标志
    } else {
      // 否则，添加所有项到selectedStoreRows中实现全选
      this.storeData.forEach((item: { Item_Nbr: number; }) => this.selectedStoreRows.add(item.Item_Nbr));
      this.isStoreAllSelected = true; // 设置全选标志
    }
  }

  selectWarehouseAll() {
    if (this.isWarehouseAllSelected || this.selectedWarehouseRows.size === this.warehouseData.length) {
      // 如果已经全选或所有项都已选中，则清空selectedStoreRows实现全不选
      this.selectedWarehouseRows.clear();
      this.isWarehouseAllSelected = false; // 重置全选标志
    } else {
      // 否则，添加所有项到selectedStoreRows中实现全选
      this.warehouseData.forEach((item: { ProductID: number; }) => this.selectedWarehouseRows.add(item.ProductID));
      this.isWarehouseAllSelected = true; // 设置全选标志
    }
  }

  toggleStoreSelection(Item_Nbr: number) {
    if (this.selectedStoreRows.has(Item_Nbr)) {
      this.selectedStoreRows.delete(Item_Nbr);
    } else {
      this.selectedStoreRows.add(Item_Nbr);
    }
  }

  toggleWarehouseSelection(Item_Nbr: number) {
    if (this.selectedWarehouseRows.has(Item_Nbr)) {
      this.selectedWarehouseRows.delete(Item_Nbr);
    } else {
      this.selectedWarehouseRows.add(Item_Nbr);
    }
  }


  confirm() {
    //if selected rows has item
    if (this.selectedStoreRows.size != 0 || this.selectedWarehouseRows.size != 0) {

      //add each row to groupDetails
      this.selectedStoreRows.forEach((item) =>
        this.groupDetails.push({ ProduceGroupID: this.groupID, OrganizationID: 2, Item_Nbr: item })
      )

      this.selectedWarehouseRows.forEach((item) =>
        this.groupDetails.push({ ProduceGroupID: this.groupID, OrganizationID: 1, Item_Nbr: item })
      )

      this.insertGroupDetails(this.groupDetails);
    }
  }

  insertGroupDetails(groupDetails: GroupDetails[]) {
    this.dataService.insertGroupDetails(groupDetails).subscribe({
      next: (data) => {
        this.groupDetail();
        this.groupDetails = [];
        this.selectedStoreRows.clear();
        this.selectedWarehouseRows.clear();
      },
      error: (error) => {
        let errorMessage = error.error.message || 'insertGroupDetails error occurred!';
        alert(errorMessage);
      }
    });
  }

  drop(event: CdkDragDrop<GroupDetails[]>) {
    var insertData = [];

    if (event.previousContainer === event.container) {
      // 同一个表格内的移动
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      // 从一个表格拖到另一个表格
      if (event.previousContainer.id == "StoreTable1" && event.container.id == "StoreTable2") {
        insertData.push({ ProduceGroupID: this.groupID, OrganizationID: 2, Item_Nbr: event.item.data.Item_Nbr })
        this.insertGroupDetails(insertData);
      } else if (event.previousContainer.id == "WarehouseTable1" && event.container.id == "WarehouseTable2") {
        insertData.push({ ProduceGroupID: this.groupID, OrganizationID: 1, Item_Nbr: event.item.data.ProductID })
        this.insertGroupDetails(insertData);
      }

      // transferArrayItem(event.previousContainer.data,
      //   event.container.data,
      //   event.previousIndex,
      //   event.currentIndex);

    }


  }



}
