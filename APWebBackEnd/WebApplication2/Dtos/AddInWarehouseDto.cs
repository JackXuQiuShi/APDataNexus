using System;
#nullable enable

namespace APWeb.Dtos
{
    public class AddInWarehouseDto
    {
        public string ProductID { get; set; } = default!;
        public double UnitQty { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
        public int StoreID { get; set; }
        public string? POID { get; set; }
    }
}
