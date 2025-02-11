export class Item {
  RequestStoreID!: number;
  Applicant: string = "";
  DepartmentID!: number;
  ProductID: string = "";
  CheckDigit!: number;
  ProductFullName: string = "";
  ProductName: string = "";
  ProductAlias: string = "";
  PackageSpec: string = "";
  Measure: string = "";
  NumPerPack!: number;
  Tax1App: number = 0;
  Tax2App: number = 0;
  UnitCost!: number;
  RetailPrice!: number;
  PromotionPrice!: number;
  VolumeUnitCost!: number;
  MinBulkVolume!: number;
  CountryOfOrigin: string = "";
  Ethnic: string = "";
  SupplierID!: number;
  CompanyName: string = "";
  UnitSize!: number;
  UnitSizeUom: string = "";
  BuyerID!: number;
}