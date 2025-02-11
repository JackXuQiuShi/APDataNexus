namespace APWeb.Dtos
{
    public class DraftOrderItemDto
    {
        public int SupplierID { get; set; }

        public string ProductID { get; set; }

        public decimal UnitQty { get; set; }

        public decimal UnitCost { get; set; }

        public int UnitsPerPackage { get; set; }

        public decimal TaxRate { get; set; }

        public int StoreID { get; set; }
    }
}
