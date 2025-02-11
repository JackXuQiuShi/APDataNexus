export class PODetails {
  POID!: string;
  ProductID: string = "";
  ProductName!: string;
  StoreID!: number;
  TaxRate: number = 0;
  orderedBy!: string;
  ReceivedStatus: number = 0;
  Prod_Alias!: string;
  UnitsPerPackage: number = 0;
  OrderedUnitQty: number = 0;
  UnitQty: number = 0;
  UnitCost: number = 0;
  CaseQty: number = 0;
  CaseCost: number = 0;
  
  UnitsReceived!: number;
  PriceReceived!: number;
  Department!: string;
  Barcode!: string;
  SupplierID!: number;
  SupplierName!: string;
  Tax1App: number = 0;
  Tax2App: number = 0;

}