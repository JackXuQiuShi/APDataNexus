namespace APWeb.Dtos
{
    public class ProductItemDto
    {
        public int? ItemID { get; set; }
        public string ProductID { get; set; }  // nchar(15) - Nullable
        public string ProductName { get; set; }  // nvarchar(50) - Nullable
        public int SupplierID { get; set; }
    }
}
