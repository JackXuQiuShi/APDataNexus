export class Inventory {
  ProductID!: string;
  StoreID!: number;
  Product_ID!: string;
  Store_ID!: number;
  Location!: string;
  LocationType!: string;
  ReturnQuantity!: number | undefined;
  Units!: number | undefined;
  UnitQty!: number | undefined;
  InspectedBy!: string;
  InspectedDate!: string;
  ProductName!: string;
  StatusID!: number | undefined;
  Department!: string;
  Barcode: string = "";
}