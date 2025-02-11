import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom, BehaviorSubject } from 'rxjs';
import { DataService } from 'src/app/services/data.service';
import { Permission } from 'src/assets/model/Permission';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(private dataService: DataService, private router: Router) {
    this.setupInactivityTimer();
  }

  // async getRegPrice(ProductID: string, StoreID: number) {
  //   try {
  //     const observable = this.dataService.getRegPrice(ProductID.trim(), StoreID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // async getProductInfo(ProductID: string) {
  //   try {
  //     const observable = this.dataService.getProductInfo(ProductID.trim());
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // async getLastPODetails(ProductID: string, StoreID: number) {

  //   try {
  //     const observable = this.dataService.getLastPODetails(ProductID.trim(), StoreID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }

  // }

  // async getNormalizedID(ProductID: string) {
  //   try {
  //     const observable = this.dataService.getNormalizedID(ProductID.trim());
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  //=======================================INVENTORY===========================================================
  // async getReturnInfo(ProductID: string, StoreID: number) {
  //   try {
  //     const observable = this.dataService.getReturnInfo(ProductID.trim(), StoreID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // 将日期格式化为 SQL Server 能接受的 YYYY-MM-DD 格式
  formatDateForSQL(selectedDate: Date) {
    if (selectedDate) {
      const date = new Date(selectedDate);
      const formattedDate = date.toISOString().split('T')[0]; // 只获取 YYYY-MM-DD 部分
      return formattedDate;
    }
    return null;
  }

  loadUser() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      return JSON.parse(userString);
    }
  }







  async getItemByProductID(upc: string) {
    if (upc !== "") {
      try {
        const observable = this.dataService.getItemByProductID(upc);
        return await firstValueFrom(observable);
      } catch (error: any) {
        alert(error.message);
      }
    }
  }


  // async getSupplierByName(supplier: string) {
  //   if (supplier !== "") {
  //     try {
  //       const observable = this.dataService.getSupplierByName(supplier);
  //       return await firstValueFrom(observable);
  //     } catch (error: any) {
  //       alert(error.message);
  //     }
  //   }
  // }

  async getSupplierRequest(CompanyName: string) {
    try {
      const observable = this.dataService.getSupplierRequest(CompanyName);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getProductGroup(name: string) {
    try {
      const observable = this.dataService.getProductGroup(name);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  async getItemByName(name: string) {
    if (name !== "") {
      try {
        const observable = this.dataService.getItemByName(name);
        return await firstValueFrom(observable);
      } catch (error: any) {
        alert(error.message);
      }
    }
  }

  async getItemNotInDetails(OrganizationID: number, name: string) {
    if (name !== "") {
      try {
        const observable = this.dataService.getItemNotInDetails(OrganizationID, name);
        return await firstValueFrom(observable);
      } catch (error: any) {
        alert(error.message);
      }
    }
  }

  async getItemFromGroupDetails(ProduceGroupID?: number) {
    try {
      const observable = this.dataService.getItemFromGroupDetails(ProduceGroupID);
      return await firstValueFrom(observable);
    } catch (error: any) {
      alert(error.message);
    }
  }

  // async getPOByCompanyName(CompanyName: string, Store_ID: number) {
  //   try {
  //     const observable = this.dataService.getPOByCompanyName(CompanyName, Store_ID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // async getPODetails(PO_ID: string) {
  //   try {
  //     const observable = this.dataService.getPODetails(PO_ID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }


  // async getLastReceiving(Product_ID: string) {
  //   try {
  //     const observable = this.dataService.getLastReceiving(Product_ID);
  //     return await firstValueFrom(observable);
  //   } catch (error: any) {
  //     alert(error.message);
  //   }
  // }

  // getSupplier(supplier: string) {
  //   let data: any;
  //   if (supplier != "") {
  //     this.dataService.getSupplier(supplier).subscribe({
  //       next: (data) => {
  //         data = data;
  //         //console.log('Get successful:', data);

  //       },
  //       error: (error) => {
  //         alert(error.message);
  //       }
  //     });
  //   }
  //   return data;
  // }

  private sharedData: any;

  private sharedPOData: any;
  private dataSubject = new BehaviorSubject<any>('Initial Data');
  public observableData = this.dataSubject.asObservable();

  permission: Permission = new Permission;

  getPermission() {
    const userString = sessionStorage.getItem('user');
    if (userString) {
      const user = JSON.parse(userString);

      // Define a mapping of roles to permissions
      const rolePermissions: { [key: string]: (keyof Permission)[] } = {
        admin: ['displaySupplier', 'displaySupplierApproval', 'displayWarehouse', 'displayHMR', 'displayGroup', 'displayItem', 'displayItemApproval', 'displayPO', 'displayInventory', 'displayOnline','approvePriceChange'],
        store: ['displayItem', 'displayInventory', 'displayPO'],
        warehouse: ['displayWarehouse', 'displayHMR', 'displayGroup', 'displayPO'],
        po: ['displayPO'],
        manager: ['displayItem', 'displayInventory']
      };

      // Initialize permissions
      Object.keys(this.permission).forEach((key) => {
        this.permission[key as keyof Permission] = false; // Reset permissions
      });

      // Set permissions based on the user's role
      Object.entries(rolePermissions).forEach(([role, permissions]) => {
        if (user.Role.includes(role)) {
          permissions.forEach((permission) => {
            this.permission[permission] = true;
          });
        }
      });
    }
    return this.permission;
  }


  setPOData(data: any) {
    this.sharedPOData = data;
    this.dataSubject.next(data);
  }

  getPOData() {
    return this.sharedPOData;
  }

  setMyData(data: any) {
    this.sharedData = data;
    this.dataSubject.next(data);
  }

  getMyData() {
    return this.sharedData;
  }



  private inactivityTimer: any;
  private setupInactivityTimer() {
    // 监听用户活动
    window.addEventListener('mousemove', () => this.resetInactivityTimer());
    window.addEventListener('keydown', () => this.resetInactivityTimer());
    window.addEventListener('click', () => this.resetInactivityTimer());

    // 初始化定时器
    this.resetInactivityTimer();
  }

  private resetInactivityTimer() {
    clearTimeout(this.inactivityTimer);
    this.inactivityTimer = setTimeout(() => this.logoutUser(), 1000 * 60 * 15); // 1000毫秒*秒*分
  }

  private logoutUser() {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }


}
