export class PO {
  PO_ID!: string;
  Buyer_ID!: number;
  Invoice!: number;
  Store_ID!: number;
  Ordered_By: string = "";
  SupplierID!: number;
  CompanyName: string = "";
  PODraftDate: string = "";
}