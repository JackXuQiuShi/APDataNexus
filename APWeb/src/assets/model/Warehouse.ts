
export class Warehouse {
  SupplierID!: number;
  ProductID: string = "";
  ProductName: string = "";
  StoreID!: number;
  BuyerID!: number;
  SupplierName: string = "";
  Applicant: string = "";
  Location: string = "";
  Invoice: string = "";
  UnitPrice: number | undefined;
  CaseCost: number | undefined;
  CaseQty: number | undefined;
  UnitsPerPackage: number | undefined;
  UnitCost: number | undefined;
  UnitQty: number | undefined;
  ProductType: string = "";
  
  ProductDesc: string = "";
  UnitOfMeasure: string = "";
  Date: string = "";
  SellTo: string = "";
  Action: string = "";
  UPC: string = "";

  DepartmentID: string = "";
  Department: string = "";

  TransactionID!: number;
  Source!: number;
}

