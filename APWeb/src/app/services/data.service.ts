import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from "@angular/common/http";
import { Item } from 'src/assets/model/Item';
import { BehaviorSubject, Observable, catchError, firstValueFrom, tap, throwError } from 'rxjs';
import { Supplier } from 'src/assets/model/Supplier';
import { ProduceGroup } from 'src/assets/model/ProduceGroup';
import { GroupDetails } from 'src/assets/model/GroupDetails';
import { PO } from 'src/assets/model/PO';
import { PODetails } from 'src/assets/model/PODetails';
import { Inventory } from 'src/assets/model/Inventory';
import { InventoryReturn } from 'src/assets/model/InventoryReturn';
import { Warehouse } from 'src/assets/model/Warehouse';
import { User } from 'src/assets/model/User';
import { HMR } from 'src/assets/model/HMR';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private config: any;
  ip = "localhost";
  port = 9001;

  UserUrl !: string;
  ItemUrl !: string;
  SupplierUrl !: string;
  GroupUrl !: string;
  POUrl !: string;
  InventoryUrl !: string;
  WarehouseUrl !: string;
  GeneralUrl !: string;
  PriceChangeUrl !: string;

  private tokenSubject = new BehaviorSubject<string | null>(null);
  public token = this.tokenSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadConfig();
  }

  // Options = {
  //   headers: new HttpHeaders({
  //     'Content-Type': 'application/json'
  //   }),
  // };
  // private getOptions(): { headers: HttpHeaders, withCredentials: boolean } {
  //   const token = this.tokenSubject.value;
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     'Authorization': token ? `Bearer ${token}` : ''
  //   });
  //   const options = { headers, withCredentials: true };
  //   console.log('token:', token);
  //   return options;
  // }

  async loadConfig(): Promise<void> {
    try {
      this.config = await firstValueFrom(this.http.get('/assets/config.json'));

      // const userAgent = window.navigator.userAgent.toLowerCase();
      // if (/mobile|android|iphone|ipad|ipod|blackberry|iemobile|opera mini/.test(userAgent)) {
      //   console.log('Mobile Device')
      //   this.ip = this.config.apiUrl;
      // } else {
      //   console.log('Desktop Device')
      //   this.ip = this.config.desktopUrl;
      // }

      this.ip = this.config.apiUrl;
      this.port = this.config.port;

      this.UserUrl = `http://${this.ip}:${this.port}/api/User`;
      this.ItemUrl = `http://${this.ip}:${this.port}/api/Item`;
      this.SupplierUrl = `http://${this.ip}:${this.port}/api/Supplier`;
      this.GroupUrl = `http://${this.ip}:${this.port}/api/Group`;
      this.POUrl = `http://${this.ip}:${this.port}/api/PO`;
      this.InventoryUrl = `http://${this.ip}:${this.port}/api/Inventory`;
      this.WarehouseUrl = `http://${this.ip}:${this.port}/api/Warehouse`;
      this.GeneralUrl = `http://${this.ip}:${this.port}/api/General`;
      this.PriceChangeUrl = `http://${this.ip}:${this.port}/api/PurchaseCostChange`;

      console.log(this.UserUrl);

    } catch (error) {
      console.error('Failed to load configuration:', error);
    }
  }

  domainLogin() {
    return this.http.get<any>(this.UserUrl + "/domainLogin", { withCredentials: true });
  }

  accountLogin(user: User): Observable<any> {
    return this.http.post<any>(this.UserUrl + "/accountLogin", user, { withCredentials: true })
  }

  // login(user: User): Observable<any> {
  //   return this.http.post<any>(`${this.UserUrl}/login`, user)
  //     .pipe(
  //       tap(response => {
  //         if (response.token) {
  //           this.tokenSubject.next(response.token);
  //           localStorage.setItem('token', response.token); // Save token to local storage
  //         }
  //       }),
  //       catchError(this.handleError)
  //     );
  // }

  // private handleError(error: any) {
  //   console.error('An error occurred:', error);
  //   return throwError(error);
  // }

  // logout() {
  //   return this.http.get<any>(this.UserUrl + "/logout");
  // }
  //=======================================GENERAL==========================================================

  async getRegPrice(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getRegPrice?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductInfoByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getProductInfoByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductStoresInfoByProductID(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getProductStoresInfoByProductID?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductInfoByProductName(ProductName: string) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getProductInfoByProductName?ProductName=" + ProductName);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getLastPODetails(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getLastPODetails?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getNormalizedID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getNormalizedID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getMaxMinCost(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getMaxMinCost?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  // getLastPOCost(ProductID: string, StoreID?: number) {
  //   if (StoreID == null) {
  //     return this.http.get<any>(this.GeneralUrl + "/getLastPOCost?ProductID=" + ProductID);
  //   } else {
  //     return this.http.get<any>(this.GeneralUrl + "/getLastPOCost?ProductID=" + ProductID + "&StoreID=" + StoreID);
  //   }
  // }

  // getIpAddress() {
  //   return this.http.get<any>(this.GeneralUrl + "/getIpAddress");
  // }
  //=======================================ITEM==========================================================

  async getItemBuyer(StoreID?: number) {
    let observable;
    try {
      if (StoreID == null) {
        observable = this.http.get<any>(this.ItemUrl + "/getItemBuyer");
      }
      else {
        observable = this.http.get<any>(this.ItemUrl + "/getItemBuyer?StoreID=" + StoreID);
      }
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getItemRequest(StatusID: number, RequestStoreID?: number) {
    let observable;
    try {
      if (RequestStoreID == null) {
        observable = this.http.get<any>(this.ItemUrl + "/getItemRequest?StatusID=" + StatusID);
      }
      else {
        observable = this.http.get<any>(this.ItemUrl + "/getItemRequest?RequestStoreID=" + RequestStoreID + "&StatusID=" + StatusID);
      }
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }


  async isItemExist(ProductID: string, RequestStoreID: number) {
    try {
      const observable = this.http.get<any>(this.ItemUrl + "/isItemExist?ProductID=" + ProductID + "&RequestStoreID=" + RequestStoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getPrice(ProductID: string) {
    try{
      const observable = this.http.get<any>(this.PriceChangeUrl + "/getPrice?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getPriceHistory(ProductID: string) {
    try{
      const observable = this.http.get<any>(this.PriceChangeUrl + "/getPriceHistory?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  draftChangeRequest(ProductID: string, CostType: string, NewPrice: number){
    return this.http.post(this.PriceChangeUrl + "/draftNewRequest?productId=" + ProductID + "&costType=" + CostType + "&newCost=" + NewPrice + "&draftUserId=" + 1, { withCredentials: true });
  }

  async getDraft(userId: number){
    try{
      const observable = this.http.get<any>(this.PriceChangeUrl + "/getDraft?draftUserId=" + userId);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  submitCostChange(productId: string, changeId: number, userId: number){
    return this.http.post(this.PriceChangeUrl + "/submitCostChange?productId=" + productId + "&changeId=" + changeId + "&userId=" + userId, { withCredentials: true });
  }

  editCostChange(ProductID: string, changeId: number, NewPrice: number){
    return this.http.post(this.PriceChangeUrl + "/editCostChange?productId=" + ProductID + "&changeId=" + changeId + "&newCost=" + NewPrice, { withCredentials: true });
  }

  deletCostChange(productId: string, changeId: number){
    return this.http.post(this.PriceChangeUrl + "/deletCostChange?productId="+productId + "&changeId=" + changeId, { withCredentials: true })
  }

  approveCostChange(productId: string, changeId: number, userId: number, newPrice: number) {
    return this.http.post(this.PriceChangeUrl + "/approveCostChange?productId="+productId + "&changeId=" + changeId + "&userId=" + userId + "&newPrice=" + newPrice, { withCredentials: true })
  }

  rejectCostChange(productId: string, changeId: number, userId: number) {
    return this.http.post(this.PriceChangeUrl + "/rejectCostChange?productId="+productId + "&changeId=" + changeId + "&userId=" + userId, { withCredentials: true })
  }

  getItemByProductID(ProductID: string) {
    return this.http.get<any>(this.ItemUrl + "/getItemByProductID?ProductID=" + ProductID);
  }

  getItemByProductName(ProductName: string) {
    return this.http.get<any>(`${this.ItemUrl}/getItemByProductName?ProductName=${ProductName}`);
  }


  // getItemRequest() {
  //   return this.http.get<any>(this.ItemUrl + "/getItemRequest");
  // }

  // getItemDraft(RequestStoreID: number) {
  //   return this.http.get<any>(this.ItemUrl + "/getItemDraft?RequestStoreID=" + RequestStoreID);
  // }

  getCurrentCost(ProductID: string) {
    return this.http.get<any>(this.ItemUrl + "/getCurrentCost?ProductID=" + ProductID);
  }





  insertItemRequest(item: Item): Observable<any> {
    return this.http.post(this.ItemUrl + "/insertItemRequest", item);
  }

  insertItemUpdateRequest(item: Item): Observable<any> {
    return this.http.post(this.ItemUrl + "/insertItemUpdateRequest", item);
  }

  updateItemByID(item: Item): Observable<any> {
    return this.http.put<any>(this.ItemUrl + "/UpdateItemByID", item);
  }

  updateItemDraft(item: Item): Observable<any> {
    return this.http.post<any>(this.ItemUrl + "/updateItemDraft", item);
  }

  deleteItemDraft(ProductID: string): Observable<any> {
    return this.http.post(this.ItemUrl + "/deleteItemDraft?ProductID=" + ProductID, { withCredentials: true });
  }

  submitItemDraft(ProductID: string[]): Observable<any> {
    return this.http.post(this.ItemUrl + "/submitItemDraft", ProductID);
  }

  approveNewProducts(ProductID: string[]): Observable<any> {
    return this.http.post(this.ItemUrl + "/approveNewProducts", ProductID);
  }

  rejectNewProducts(ProductID: string[]): Observable<any> {
    return this.http.post(this.ItemUrl + "/rejectNewProducts", ProductID);
  }
  //=======================================INVENTORY===========================================================

  async getReturnInfo(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getReturnInfo?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getLastScanInfo(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getLastScanInfo?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getInfoFromPOS306090(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getInfoFromPOS306090?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      console.log(error.message);
    }
  }

  async getInventoryByLocation(Location: string) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/GetInventoryByLocation?Location=" + Location);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductLocation(Location: string) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getProductLocation?ProductID=" + Location);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  // async getNeedReturnListSuppliers() {
  //   try {
  //     const observable = this.http.get<any>(this.InventoryUrl + "/getNeedReturnListSuppliers");
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  async getReturnSuppliers(StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getReturnSuppliers?StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getCreditList(StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getCreditList?StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getCreditByCreditNumber(CreditNumber: string) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getCreditByCreditNumber?CreditNumber=" + CreditNumber);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductInspectionResultSuppliers(StoreID: number) {
    try {
      const observable = this.http.get<any>(this.GeneralUrl + "/getProductInspectionResultSuppliers?StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductInspectionResult(StoreID: number, SupplierID?: number) {
    try {
      if (SupplierID) {
        const observable = this.http.get<any>(this.GeneralUrl + "/getProductInspectionResult?SupplierID=" + SupplierID + "&StoreID=" + StoreID);
        return await firstValueFrom(observable);
      } else {
        const observable = this.http.get<any>(this.GeneralUrl + "/getProductInspectionResult?StoreID=" + StoreID);
        return await firstValueFrom(observable);
      }
    } catch (error: any) {
      alert(error.message);
    }
  }


  async getReturnData(SupplierID: number, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getReturnData?SupplierID=" + SupplierID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getIfLocationExist(Location: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.InventoryUrl + "/getIfLocationExist?Location=" + Location + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  updateProductInspectionStatus(inventoryReturn: InventoryReturn): Observable<any> {
    return this.http.post<any>(this.GeneralUrl + "/updateProductInspectionStatus", inventoryReturn);
  }

  insertInventory(inventory: Inventory): Observable<any> {
    return this.http.post(this.InventoryUrl + "/insertInventory", inventory);
  }

  updateReturnQuantity(inventoryReturn: InventoryReturn): Observable<any> {
    return this.http.post<any>(this.InventoryUrl + "/updateReturnQuantity", inventoryReturn);
  }

  generateCreditNote(returnIDs: number[], StoreID: number): Observable<any> {
    return this.http.post<any>(this.InventoryUrl + "/generateCreditNote?StoreID=" + StoreID, returnIDs);
  }

  submitReturnItem(inventoryReturn: InventoryReturn): Observable<any> {
    return this.http.post<any>(this.InventoryUrl + "/submitReturnItem", inventoryReturn);
  }

  submitStorageLocation(inventory: Inventory): Observable<any> {
    return this.http.post<any>(this.InventoryUrl + "/submitStorageLocation", inventory);
  }

  insertReturnQuantity(inventoryReturn: InventoryReturn): Observable<any> {
    return this.http.post<any>(this.InventoryUrl + "/insertReturnQuantity", inventoryReturn);
  }

  // deleteReturnItem(ReturnID: number): Observable<any> {
  //   return this.http.post(this.InventoryUrl + "/deleteReturnItem?ReturnID=", ReturnID);
  // }

  //=======================================Warehouse===========================================================

  async getCaseQty(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getCaseQty?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  // async getWarehouseProductByProductID(ProductID: string) {
  //   try {
  //     const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseProductByProductID?ProductID=" + ProductID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // async getWarehouseProductByProductName(ProductName: string) {
  //   try {
  //     const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseProductByProductName?ProductName=" + ProductName);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  async getWarehouseProductQty(ProductID: string, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/GetWarehouseProductQty?ProductID=" + ProductID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  // async getWarehouseInventoryByProductID(ProductID: string) {
  //   try {
  //     const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseInventoryByProductID?ProductID=" + ProductID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  async getWarehouseTransactionByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseTransactionByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getWarehouseTransactionSummary() {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseTransactionSummary");
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getWarehouseInventory() {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseInventory");
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getWarehouseTotalQtyByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseTotalQtyByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getWarehouseDrafts(StoreID: number) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getWarehouseDrafts?StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }



  // createWarehouseDraft(warehouse: Warehouse): Observable<any> {
  //   return this.http.post<any>(this.WarehouseUrl + "/createWarehouseDraft", warehouse);
  // }

  addInWarehouse(ProductData: Warehouse) {
    return this.http.post<any>(this.WarehouseUrl + "/addInWarehouse", ProductData);
  }

  takeOutWarehouse(ProductData: Warehouse) {
    return this.http.post<any>(this.WarehouseUrl + "/takeOutWarehouse", ProductData);
  }

  addInWarehouseBatch(POData: any) {
    return this.http.post<any>(this.WarehouseUrl + "/addInWarehouseBatch", POData);
  }

  takeOutWarehouseBatch(POData: any) {
    return this.http.post<any>(this.WarehouseUrl + "/takeOutWarehouseBatch", POData);
  }


  getWarehouseStorage() {
    return this.http.get<any>(this.WarehouseUrl + "/getWarehouseStorage");
  }

  submitWarehouseItem(warehouse: Warehouse): Observable<any> {
    return this.http.post<any>(this.WarehouseUrl + "/submitWarehouseItem", warehouse);
  }
  //=======================================HMR============================================================


  async getHMRProductByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getHMRProductByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getHMRProductByProductName(ProductName: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getHMRProductByProductName?ProductName=" + ProductName);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getHMRInventoryByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getHMRInventoryByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getHMRTransactionByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getHMRTransactionByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getHMRTotalQtyByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.WarehouseUrl + "/getHMRTotalQtyByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  addInHMR(ProductData: HMR) {
    return this.http.post<any>(this.WarehouseUrl + "/addInHMR", ProductData);
  }

  takeOutHMR(ProductData: HMR) {
    return this.http.post<any>(this.WarehouseUrl + "/takeOutHMR", ProductData);
  }

  //=======================================GROUP==========================================================
  getProductGroup(name: string) {
    return this.http.get<any>(this.GroupUrl + "/getProductGroup?name=" + name);
  }

  insertGroup(produceGroup: ProduceGroup): Observable<any> {
    return this.http.post(this.GroupUrl + "/insertGroup", produceGroup);
  }

  updateGroup(produceGroup: ProduceGroup): Observable<any> {
    return this.http.post(this.GroupUrl + "/updateGroup", produceGroup);
  }

  deleteGroupDetails(groupDetails: GroupDetails): Observable<any> {
    return this.http.post(this.GroupUrl + "/deleteGroupDetails", groupDetails);
  }

  getItemByName(item_name: string) {
    return this.http.get<any>(this.GroupUrl + "/getItemByName?item_name=" + item_name);
  }

  getItemFromWarehouse(item_name: string) {
    return this.http.get<any>(this.GroupUrl + "/getItemFromWarehouse?item_name=" + item_name);
  }

  getItemNotInDetails(OrganizationID: number, itemName: string) {
    return this.http.get<any>(this.GroupUrl + "/getItemNotInDetails?OrganizationID=" + OrganizationID + "&itemName=" + itemName);
  }

  insertGroupDetails(groupDetails: GroupDetails[]): Observable<any> {
    return this.http.post(this.GroupUrl + "/insertGroupDetails", groupDetails);
  }

  getItemFromGroupDetails(ProduceGroupID?: number) {
    if (ProduceGroupID == null) {
      return this.http.get<any>(this.GroupUrl + "/getItemFromGroupDetails");
    } else {
      return this.http.get<any>(this.GroupUrl + "/getItemFromGroupDetails?ProduceGroupID=" + ProduceGroupID);
    }
  }





  // deleteItemByID(Item_ID: number): Observable<any> {
  //   return this.http.post(this.APIUrl + "/DeleteItemByID?Item_Id=" + Item_ID);
  // }

  // getSupplierByCFIA(CFIA: string) {
  //   return this.http.get<any>(this.SupplierUrl + "/getSupplier?SafetyLicense=" + CFIA);
  // }

  //=======================================Supplier==========================================================
  // getSupplierByName(CompanyName: string) {
  //   return this.http.get<any>(this.SupplierUrl + "/getSupplierByName?CompanyName=" + CompanyName);
  // }

  async getSupplierByName(CompanyName: string) {
    if (CompanyName !== "") {
      try {
        const observable = this.http.get<any>(this.SupplierUrl + "/getSupplierByName?CompanyName=" + CompanyName);
        return await firstValueFrom(observable);
      } catch (error: any) {
        alert(error.message);
      }
    }
  }

  getSupplierRequest(CompanyName: string) {
    return this.http.get<any>(this.SupplierUrl + "/getSupplierRequest?CompanyName=" + CompanyName);
  }

  insertSupplierRequest(supplier: Supplier): Observable<any> {
    return this.http.post(this.SupplierUrl + "/insertSupplierRequest", supplier);
  }

  updateSupplierRequest(CompanyName: string, Status: number): Observable<any> {
    return this.http.post(this.SupplierUrl + "/updateSupplierRequest?CompanyName=" + CompanyName + "&Status=" + Status, { withCredentials: true });
  }


  //=======================================PO==========================================================

  async getPOByPOID(POID: string, Source?: number) {
    try {
      let params = new HttpParams().set('POID', POID);
      // 如果 Source 参数存在，则添加到查询参数中
      if (Source !== undefined && Source !== null) {
        params = params.set('Source', Source.toString());
      }
      const observable = this.http.get<any>(this.POUrl + "/getPOByPOID", { params });
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getOrderByOrderID(OrderID: string) {
    try {
      const observable = this.http.get<any>(this.POUrl + "/getOrderByOrderID?OrderID=" + OrderID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getOrdersSuppliersByStatusID(StatusID: number, StoreID: number) {
    try {
      const observable = this.http.get<any>(this.POUrl + "/getOrdersSuppliersByStatusID?StatusID=" + StatusID + "&StoreID=" + StoreID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }


  async getOrdersByStatusIDAndStoreID(StatusID: number, StoreID: number, SupplierID?: number) {
    try {
      let url = `${this.POUrl}/getOrdersByStatusIDAndStoreID?StatusID=${StatusID}&StoreID=${StoreID}`;
      
      // 如果 SupplierID 存在，则添加到 URL 中
      if (SupplierID !== undefined) {
        url += `&SupplierID=${SupplierID}`;
      }
  
      const observable = this.http.get<any>(url);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  
  async getSuppliersByProductID(ProductID: string) {
    try {
      const observable = this.http.get<any>(this.POUrl + "/getSuppliersByProductID?ProductID=" + ProductID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async isProductItemExist(ProductID: string, SupplierID: number) {
    try {
      const observable = this.http.get<any>(this.POUrl + "/isProductItemExist?productID=" + ProductID + "&supplierID=" + SupplierID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  updateProductMovementItem(PODetails: PODetails): Observable<any> {
    return this.http.post(this.POUrl + "/updateProductMovementItem", PODetails);
  }

  draftOrderItem(PODetails: PODetails): Observable<any> {
    return this.http.post(this.POUrl + "/draftOrderItem", PODetails);
  }

  confirmOrder(OrderID: number): Observable<any> {
    return this.http.post(this.POUrl + "/confirmOrder?OrderID=" + OrderID, { withCredentials: true });
  }

  deleteOrderItem(PODetails: PODetails): Observable<any> {
    return this.http.post(this.POUrl + "/deleteOrderItem", PODetails);
  }

 

  // getPOByCompanyName(CompanyName: string, Store_ID: number) {
  //   // if (Store == null) {
  //   //   return this.http.get<any>(this.POUrl + "/getPOByCompanyName?CompanyName=" + CompanyName);
  //   // } else {
  //   //   return this.http.get<any>(this.POUrl + "/getPOByCompanyName?CompanyName=" + CompanyName);
  //   // }
  //   return this.http.get<any>(this.POUrl + "/getPOByCompanyName?CompanyName=" + CompanyName + "&Store_ID=" + Store_ID);
  // }

  // getPODetails(PO_ID: string) {
  //   return this.http.get<any>(this.POUrl + "/getPODetails?PO_ID=" + PO_ID);
  // }

  // getLastReceiving(Product_ID: string) {
  //   return this.http.get<any>(this.POUrl + "/getLastReceiving?Product_ID=" + Product_ID);
  // }

  // insertPO(po: PO): Observable<any> {
  //   return this.http.post(this.POUrl + "/insertPO", po);
  // }

  // insertPODetails(PODetails: PODetails): Observable<any> {
  //   return this.http.post(this.POUrl + "/insertPODetails", PODetails);
  // }

  // updatePODetails(PODetails: PODetails, state: string): Observable<any> {
  //   return this.http.post(this.POUrl + "/updatePODetails?state=" + state, PODetails);
  // }

  // updatePOToOrdered(PO_ID: string): Observable<any> {
  //   return this.http.post(this.POUrl + "/UpdatePOToOrdered?PO_ID=" + PO_ID, { withCredentials: true });
  // }

  // updatePOToReceived(PO: PO): Observable<any> {
  //   return this.http.post(this.POUrl + "/updatePOToReceived", PO);
  // }

  // updatePODetailsReceivedStatus(PODetails: PODetails[]): Observable<any> {
  //   return this.http.post(this.POUrl + "/updatePODetailsReceivedStatus", PODetails);
  // }

  // deletePODetails(PODetails: PODetails): Observable<any> {
  //   return this.http.post(this.POUrl + "/deletePODetails", PODetails);
  // }





  // getItem() {
  //   return this.http.get<any>(this.APIUrl +"/GetItem");
  // }

  // insertItem(Item: Item): Observable<any> { 
  //   return this.http.post(this.APIUrl + "/InsertItem", Item);
  // }

  // deleteItemByID(Item_ID: number): Observable<any> {
  //   return this.http.post(this.APIUrl + "/DeleteItemByID?Item_Id=" + Item_ID);
  // }

  // updateItemByID(Item_Id: number, Item_Name: string): Observable<any> {
  //   return this.http.post(this.APIUrl + "/UpdateItemByID?Item_Id=" + Item_Id + "&Item=" + Item_Name );
  // }

}
