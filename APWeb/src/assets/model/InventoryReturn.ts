export class InventoryReturn {
  ProductID: string = "";
  StoreID: number = 0;
  ReturnQuantity!: number | undefined;
  Location!: string;
  ProductName!: string;
  SupplierID!: number | undefined;
  ReturnID!: number | undefined;
  Tax: number = 0;
  UnitCost!: number | undefined;
}