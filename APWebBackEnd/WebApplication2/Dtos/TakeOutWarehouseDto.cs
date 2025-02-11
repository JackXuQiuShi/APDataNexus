#nullable enable

namespace APWeb.Dtos
{
    public class TakeOutWarehouseDto
    {
        public string ProductID { get; set; } = default!;
        public double UnitQty { get; set; } 
        public int StoreID { get; set; }
        public string? POID { get; set; }
        public int Source { get; set; } = default!;
    }
}
