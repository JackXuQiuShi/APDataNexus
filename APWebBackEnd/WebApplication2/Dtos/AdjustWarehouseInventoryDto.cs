namespace APWeb.Dtos
{
    public class AdjustWarehouseInventoryDto
    {
        public string ProductID { get; set; } = default!;
        public double UnitQty { get; set; }
        public int? SupplierID { get; set; }
        public int StoreID { get; set; }
    }
}
