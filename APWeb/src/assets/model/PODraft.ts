export class PODraft {
  PO_ID!: string;
  ProductID: string = "";
  StoreID!: number;
  TaxRate: number = 0;
  orderedBy!: string;
  ReceivedStatus: number = 0;
  
  Prod_Alias!: string;

  ProductName!: string;

  RegPrice: number | undefined;
  OrderingDate: string = "";
  SupplierID: number | undefined;
  CompanyName: string = "";
  CaseCost: number | undefined;
  CaseQty: number | undefined;
  UnitsPerPackage: number | undefined;
  UnitCost: number | undefined;
  UnitQty: number | undefined;
  UnitPrice: number | undefined;
  Department!: string;
}