namespace APWeb.Dtos
{
    public class ProductMovementItemRequest
    {
        public string OrderID { get; set; }
        public int MovementID { get; set; }
        public int ProductItemID { get; set; }
        public int SourceWarehouseLocationID { get; set; }
        public int DestinationWarehouseLocationID { get; set; }
        public decimal UnitQty { get; set; }
        public decimal UnitCost { get; set; }
        public int ProductMovementItemStatusID { get; set; }

    }
}
