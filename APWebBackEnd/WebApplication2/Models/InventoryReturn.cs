using System;

namespace APWeb.Models
{
    public class InventoryReturn
    {

        public int StoreID { get; set; }
        public string ProductID { get; set; }
        public string Location { get; set; }
        public int Units { get; set; }
        public string InspectedBy { get; set; }
        public int ReturnQuantity { get; set; }
        public int SupplierID { get; set; }
        public int ReturnID { get; set; }
        public string ProductName { get; set; }
        public float UnitCost { get; set; }
        public int Tax { get; set; }
    }
}
